import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { PayloadAction } from '@reduxjs/toolkit';
import { Middleware, MiddlewareAPI } from 'redux';
import { events } from 'src/core-hub';
import { ErrorCodes } from 'src/utils/errors';
import * as actions from './actions';
import appHubConn from './app-hub-connection';
import { Options } from './types';

type SignalRResult = {
   middleware: Middleware;
   getConnection: () => HubConnection | undefined;
};

export default (options: Options): SignalRResult => {
   const { signalUrl } = options;

   let connection: HubConnection | undefined;

   // Define the list of handlers, now that we have an instance of ReduxWebSocket.
   const handlers: {
      [s: string]: (middleware: MiddlewareAPI, action: any) => void;
   } = {
      [actions.connectSignal.type]: async ({ dispatch }, { payload: { appData, urlParams, defaultEvents } }) => {
         if (!connection) {
            const queryString = Object.keys(urlParams)
               .map((key) => key + '=' + urlParams[key])
               .join('&');

            const conferenceUrl = signalUrl + '?' + queryString;

            connection = new HubConnectionBuilder()
               .withUrl(conferenceUrl)
               .withAutomaticReconnect()
               .configureLogging(LogLevel.Information)
               .build();

            connection.on(events.onConnectionError, (err) => {
               dispatch(actions.onConnectionError(err));
               dispatch(actions.close());
            });

            for (const eventName of defaultEvents) {
               connection.on(eventName, (args) => dispatch(actions.onEventOccurred(eventName)(args)));
            }

            try {
               await connection.start();
               connection.onclose((error) => {
                  connection = undefined;
                  appHubConn.remove();

                  dispatch(actions.onConnectionClosed(appData, error));
               });
               connection.onreconnected(() => dispatch(actions.onReconnected(appData)));
               connection.onreconnecting((error) => dispatch(actions.onReconnecting(appData, error)));

               appHubConn.register(connection);

               dispatch(actions.onConnected(appData));
            } catch (error) {
               dispatch(
                  actions.onConnectionError({
                     code: ErrorCodes.SignalRConnectionFailed,
                     message: error.toString(),
                     type: 'SignalR',
                  }),
               );

               await connection.stop();
               connection = undefined;
            }
         }
      },
      [actions.subscribeEvent.type]: ({ dispatch }, { payload: { name } }) => {
         if (connection) {
            console.log('subscribe ' + name);
            connection.on(name, (args) => dispatch(actions.onEventOccurred(name)(args)));
         }
      },
      [actions.send.type]: (_, { payload: { payload, name } }) => {
         if (connection) {
            connection.send(name, ...(payload !== null && payload !== undefined ? [payload] : []));
         }
      },
      [actions.invoke.type]: ({ dispatch }, { payload: { payload, name } }) => {
         if (connection) {
            connection.invoke(name, ...(payload ? [payload] : [])).then(
               (returnVal) => {
                  dispatch(actions.onInvokeReturn(name)(returnVal));
               },
               (error) => dispatch(actions.onInvokeFailed(name)(error)),
            );
         }
      },
      [actions.close.type]: async () => {
         if (connection) {
            await connection.stop();
            connection = undefined;
            appHubConn.remove();
         }
      },
   };

   // Middleware function.
   const middleware: Middleware = (store: MiddlewareAPI) => (next) => (action: PayloadAction) => {
      const { type } = action;

      // Check if action type matches prefix
      if (type) {
         const handler = handlers[type];

         if (handler) {
            try {
               handler(store, action);
            } catch (err) {
               console.error(err);
            }
         }
      }

      return next(action);
   };

   const getConnection = () => connection;

   return { middleware, getConnection };
};
