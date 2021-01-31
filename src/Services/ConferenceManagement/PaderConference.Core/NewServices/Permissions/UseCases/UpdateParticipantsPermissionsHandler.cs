﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PaderConference.Core.NewServices.Permissions.Gateways;
using PaderConference.Core.NewServices.Permissions.Notifications;
using PaderConference.Core.NewServices.Permissions.Requests;
using PermissionsDict = System.Collections.Generic.Dictionary<string, Newtonsoft.Json.Linq.JValue>;

namespace PaderConference.Core.NewServices.Permissions.UseCases
{
    public class UpdateParticipantsPermissionsHandler : IRequestHandler<UpdateParticipantsPermissionsRequest>
    {
        private readonly IAggregatedPermissionRepository _permissionRepository;
        private readonly IPermissionLayersAggregator _permissionLayersAggregator;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateParticipantsPermissionsHandler> _logger;

        public UpdateParticipantsPermissionsHandler(IAggregatedPermissionRepository permissionRepository,
            IPermissionLayersAggregator permissionLayersAggregator, IMediator mediator,
            ILogger<UpdateParticipantsPermissionsHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _permissionLayersAggregator = permissionLayersAggregator;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateParticipantsPermissionsRequest request,
            CancellationToken cancellationToken)
        {
            var (conferenceId, participantIds) = request;

            var newPermissions = new Dictionary<string, PermissionsDict>();
            foreach (var participantId in participantIds)
            {
                newPermissions.Add(participantId,
                    await _permissionLayersAggregator.FetchAggregatedPermissions(conferenceId, participantId));
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogDebug("Update permissions for {count} participants", newPermissions.Count);

            foreach (var (participantId, permissions) in newPermissions)
            {
                await _permissionRepository.SetPermissions(conferenceId, participantId, permissions);
            }

            await _mediator.Publish(new ParticipantPermissionsUpdatedNotification(conferenceId, newPermissions));
            return Unit.Value;
        }
    }
}
