﻿using HA4IoT.Contracts;
using HA4IoT.Contracts.Core;
using HA4IoT.Contracts.Hardware.RemoteSockets.Adapters;
using HA4IoT.Extensions;
using HA4IoT.Extensions.I2C;
using System;
using System.Collections.Generic;

namespace HA4IoT.Controller.Dnf
{
    internal class ContainerConfigurator : IContainerConfigurator
    {
        public ContainerConfigurator()
        {
        }

        public void ConfigureContainer(IContainer containerService)
        {
            if (containerService == null) throw new ArgumentNullException(nameof(containerService));
            
            containerService.RegisterSingleton<IAlexaDispatcherEndpointService, AlexaDispatcherEndpointService>();
            containerService.RegisterSingleton<ISerialService, SerialService>();
            containerService.RegisterSingleton<II2CService, I2CService>();
            containerService.RegisterSingletonCollection(new IMessageHandler[] { new InfraredMessageHandler(), new LPD433MessageHandler(), new InfraredRawMessageHandler() });

        }
    }

}
