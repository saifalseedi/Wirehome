﻿using HA4IoT.Contracts.Core;
using HA4IoT.Contracts.Hardware.I2C;
using HA4IoT.Contracts.Logging;
using HA4IoT.Contracts.Messaging;
using HA4IoT.Contracts.Services;
using HA4IoT.Hardware.Drivers.I2CHardwareBridge;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace HA4IoT.Extensions.I2C
{
    public class I2CService : II2CService
    {
        private readonly ILogger _logService;
        private readonly IMessageBrokerService _messageBroker;
        private readonly II2CBusService _i2cServiceBus;
        private readonly IDeviceRegistryService _deviceService;
        private I2CHardwareBridge _bridge;
        private readonly List<IMessageHandler> _messageHandlers = new List<IMessageHandler>();

        public I2CService(ILogService logService, IMessageBrokerService messageBroker, II2CBusService i2CBusService, IDeviceRegistryService deviceService, IEnumerable<IMessageHandler> handlers)
        {
            _logService = logService.CreatePublisher(nameof(I2CService));
            _messageBroker = messageBroker;
            _i2cServiceBus = i2CBusService;
            _deviceService = deviceService;
            _messageHandlers.AddRange(handlers);
 
        }

        public void Startup()
        {
            _bridge = _deviceService.GetDevice<I2CHardwareBridge>();
   
            _messageHandlers.ForEach(handler =>
            {
                _messageBroker.Subscribe(new MessageSubscription
                {
                    Id = Guid.NewGuid().ToString(),
                    PayloadType = handler.SupportedMessageType().Name,
                    Topic = typeof(I2CService).Name,
                    Callback = MessageHandler
                });
            });
        }

        public void MessageHandler(Message<JObject> message)
        {
            _messageHandlers.ForEach(handler =>
            {
                if(handler.CanHandleI2C(message.Payload.Type))
                {
                    try
                    {
                        var package = handler.PrepareI2cPackage(message.Payload.Content);
                        _i2cServiceBus.Write(_bridge.Address, package);
                    }
                    catch(Exception ex)
                    {
                        _logService.Error(ex, $"Handler of type {handler.SupportedMessageType()} failed to process message");
                    }
                }
            });
        }
    }
}