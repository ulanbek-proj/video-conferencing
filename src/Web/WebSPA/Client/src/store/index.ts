import { Action, configureStore, Middleware, ThunkAction } from '@reduxjs/toolkit';
import createSagaMiddleware from 'redux-saga';
import notifierMiddleware from './notifier/create-middleware';
import rootReducer from './root-reducer';
import rootSaga from './root-saga';
import createMiddleware from './signal/create-middleware';
import { loadState } from './storage';

const { middleware: signalrMiddleware } = createMiddleware();

const sagaMiddleware = createSagaMiddleware();

// configure middlewares
const middlewares: Middleware[] = [signalrMiddleware, sagaMiddleware, notifierMiddleware];

// rehydrate state on app start
const initialState = loadState({});

// create store
const store = configureStore({
   reducer: rootReducer,
   middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(middlewares),
   preloadedState: initialState,
});
// const store = createStore(rootReducer, initialState, enhancer);
// persistState(store, persistInLocalStorage, persistInSessionStorage);

// run redux saga
sagaMiddleware.run(rootSaga);

// export store singleton instance
export default store;

export type AppThunk = ThunkAction<void, RootState, unknown, Action<string>>;
export type RootState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;

// Store persistence
// function persistInLocalStorage(_: RootState): Partial<RootState> {
//    return {};
// }

// function persistInSessionStorage(_: RootState): Partial<RootState> {
//    return {};
// }
