﻿using MediatR;

namespace PaderConference.Core.Services.ConferenceControl.Notifications
{
    public record ParticipantKickedNotification(string ParticipantId, string ConferenceId,
        ParticipantKickedReason Reason) : INotification;

    public enum ParticipantKickedReason
    {
        ByModerator,
        NewSessionConnected,
    }
}
