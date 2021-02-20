﻿using MediatR;

namespace PaderConference.Core.Services.ConferenceControl.Notifications
{
    public record ParticipantKickedNotification(Participant Participant, string? ConnectionId,
        ParticipantKickedReason Reason) : INotification;

    public enum ParticipantKickedReason
    {
        ByModerator,
        NewSessionConnected,
    }
}
