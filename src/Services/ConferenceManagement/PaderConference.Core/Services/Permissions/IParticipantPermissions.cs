﻿using System.Threading.Tasks;

namespace PaderConference.Core.Services.Permissions
{
    public interface IParticipantPermissions
    {
        ValueTask<IPermissionStack> FetchForParticipant(Participant participant);
    }
}
