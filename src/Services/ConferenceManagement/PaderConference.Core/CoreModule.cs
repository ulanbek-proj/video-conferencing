using Autofac;
using FluentValidation;
using PaderConference.Core.Interfaces;
using PaderConference.Core.NewServices.Permissions;
using PaderConference.Core.Services;
using PaderConference.Core.Services.ConferenceControl;

namespace PaderConference.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IUseCaseRequestHandler<,>)).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ThisAssembly).AssignableTo<IConferenceService>().AsSelf()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(AbstractValidator<>))
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ConferenceScheduler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly).AssignableTo<IPermissionLayerProvider>()
                .As<IPermissionLayerProvider>().InstancePerDependency();
            builder.RegisterType<PermissionLayersAggregator>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<ParticipantPermissions>().AsImplementedInterfaces().InstancePerDependency();
        }
    }
}
