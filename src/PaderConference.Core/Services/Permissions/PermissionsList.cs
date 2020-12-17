﻿namespace PaderConference.Core.Services.Permissions
{
    public static class PermissionsList
    {
        public static class Conference
        {
            public static readonly PermissionDescriptor<bool> CanOpenAndClose =
                new PermissionDescriptor<bool>("conference.canOpenAndClose");

            public static readonly PermissionDescriptor<bool> CanRaiseHand = new("conference.canRaiseHand");
        }

        public static class Permissions
        {
            public static readonly PermissionDescriptor<bool> CanGiveTemporaryPermission =
                new("permissions.canGiveTemporaryPermission");


            public static readonly PermissionDescriptor<bool> CanSeeAnyParticipantsPermissions =
                new("permissions.canSeeAnyParticipantsPermissions");
        }

        public static class Chat
        {
            public static readonly PermissionDescriptor<bool> CanSendChatMessage =
                new PermissionDescriptor<bool>("chat.canSendMessage");

            public static readonly PermissionDescriptor<bool> CanSendPrivateChatMessage =
                new PermissionDescriptor<bool>("chat.canSendPrivateMessage");

            public static readonly PermissionDescriptor<bool> CanSendAnonymousMessage =
                new PermissionDescriptor<bool>("chat.canSendAnonymousMessage");
        }

        public static class Media
        {
            public static readonly PermissionDescriptor<bool> CanShareAudio =
                new PermissionDescriptor<bool>("media.canShareAudio");

            public static readonly PermissionDescriptor<bool> CanShareScreen =
                new PermissionDescriptor<bool>("media.canShareScreen");

            public static readonly PermissionDescriptor<bool> CanShareWebcam =
                new PermissionDescriptor<bool>("media.canShareWebcam");
        }

        public static class Rooms
        {
            public static readonly PermissionDescriptor<bool> CanCreateAndRemove =
                new PermissionDescriptor<bool>("rooms.canCreateAndRemove");

            public static readonly PermissionDescriptor<bool> CanSwitchRoom =
                new PermissionDescriptor<bool>("rooms.canSwitchRoom");
        }

        public static class Scenes
        {
            public static readonly PermissionDescriptor<bool> CanSetScene =
                new PermissionDescriptor<bool>("scenes.canSetScene");
        }
    }
}
