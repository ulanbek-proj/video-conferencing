﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PaderConference.Core.Services.Permissions.Gateways;

namespace PaderConference.Core.Services.Permissions.PermissionLayers
{
    public class TemporaryPermissionLayerProvider : IPermissionLayerProvider
    {
        private readonly ITemporaryPermissionRepository _temporaryPermissionRepository;

        public TemporaryPermissionLayerProvider(ITemporaryPermissionRepository temporaryPermissionRepository)
        {
            _temporaryPermissionRepository = temporaryPermissionRepository;
        }

        public async ValueTask<IEnumerable<PermissionLayer>> FetchPermissionsForParticipant(Participant participant)
        {
            var permissions = await _temporaryPermissionRepository.FetchTemporaryPermissions(participant);
            return new List<PermissionLayer> {CommonPermissionLayers.Temporary(permissions)};
        }
    }
}
