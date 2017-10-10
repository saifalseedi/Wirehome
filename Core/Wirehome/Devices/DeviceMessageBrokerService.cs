﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using Wirehome.Contracts.Api;
using Wirehome.Contracts.Hardware.DeviceMessaging;
using Wirehome.Contracts.Hardware.Mqtt;
using Wirehome.Contracts.Logging;
using Wirehome.Contracts.Scripting;
using Wirehome.Contracts.Services;
using MQTTnet;
using MQTTnet.Core;
using MQTTnet.Core.Client;
using MQTTnet.Core.Diagnostics;
using MQTTnet.Core.Packets;
using MQTTnet.Core.Protocol;
using MQTTnet.Core.Server;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Wirehome.Devices
{
    [ApiServiceClass(typeof(IDeviceMessageBrokerService))]
    public class DeviceMessageBrokerService : ServiceBase, IDeviceMessageBrokerService
    {
        //TODO Fix after new version of MQQT
        private readonly IMqttServer _server;
        private readonly IMqttClient _client;
        private readonly ILogger _log;
        private readonly MqttCommunicationAdapter _clientCommunicationAdapter;

        public event EventHandler<DeviceMessageReceivedEventArgs> MessageReceived;

        public void Publish(string topic, byte[] payload, MqttQosLevel qosLevel)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string topicPattern, Action<DeviceMessage> callback)
        {
            throw new NotImplementedException();
        }

        //    public DeviceMessageBrokerService(ILogService logService, IScriptingService scriptingService)
        //    {
        //        if (scriptingService == null) throw new ArgumentNullException(nameof(scriptingService));
        //        _log = logService?.CreatePublisher(nameof(DeviceMessageBrokerService)) ?? throw new ArgumentNullException(nameof(logService));

        //        MqttTrace.TraceMessagePublished += (s, e) =>
        //        {
        //            if (e.Level == MqttTraceLevel.Warning)
        //            {
        //                _log.Warning(e.Exception, e.Message);
        //            }
        //            else if (e.Level == MqttTraceLevel.Error)
        //            {
        //                _log.Error(e.Exception, e.Message);
        //            }
        //        };

        //        var channelA = new MqttCommunicationAdapter();
        //        _clientCommunicationAdapter = new MqttCommunicationAdapter();
        //        channelA.Partner = _clientCommunicationAdapter;
        //        _clientCommunicationAdapter.Partner = channelA;

        //        var mqttClientOptions = new MqttClientOptions { ClientId = "Wirehome.Loopback", KeepAlivePeriod = TimeSpan.FromHours(1) };
        //        _client = new MqttClient(mqttClientOptions, channelA);
        //        _client.ApplicationMessageReceived += ProcessIncomingMessage; 

        //        var mqttServerOptions = new MqttServerOptions();
        //        _server = new MqttServerFactory().CreateMqttServer(mqttServerOptions);
        //        _server.ClientConnected += (s, e) => _log.Info($"MQTT client '{e.Identifier}' connected.");

        //        scriptingService.RegisterScriptProxy(s => new DeviceMessageBrokerScriptProxy(this, s));
        //    }

        //    private void ProcessIncomingMessage(object sender, MQTTnet.Core.Client.MqttApplicationMessageReceivedEventArgs e)
        //    {
        //        _log.Verbose($"Broker received message '{e.ApplicationMessage.Topic}' [{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}].");

        //        var message = new DeviceMessage
        //        {
        //            Topic = e.ApplicationMessage.Topic,
        //            Payload = e.ApplicationMessage.Payload,
        //            QosLevel = (MqttQosLevel)e.ApplicationMessage.QualityOfServiceLevel
        //        };

        //        MessageReceived?.Invoke(this, new DeviceMessageReceivedEventArgs(message));
        //    }

        //    public event EventHandler<DeviceMessageReceivedEventArgs> MessageReceived;

        //    public override async Task Initialize()
        //    {
        //        await _server.StartAsync();
        //        _server.InjectClient("Wirehome.Loopback", _clientCommunicationAdapter);

        //        _client.ConnectAsync().Wait();
        //        _client.SubscribeAsync(new TopicFilter("#", MqttQualityOfServiceLevel.AtMostOnce)).Wait();

        //        _log.Info("MQTT client (loopback) connected.");

        //        return Task.CompletedTask;
        //    }

        //    [ApiMethod]
        //    public void GetConnectedClients(IApiCall apiCall)
        //    {
        //        var connectedClients = _server.GetConnectedClients();
        //        apiCall.Result["ConnectedClients"] = JToken.FromObject(connectedClients);
        //    }

        //    [ApiMethod]
        //    public void Publish(IApiCall apiCall)
        //    {
        //        var deviceMessage = apiCall.Parameter.ToObject<DeviceMessage>();
        //        Publish(deviceMessage.Topic, deviceMessage.Payload, deviceMessage.QosLevel);
        //    }

        //    public void Publish(string topic, byte[] payload, MqttQosLevel qosLevel)
        //    {
        //        try
        //        {
        //            var message = new MqttApplicationMessage(topic, payload, (MqttQualityOfServiceLevel)qosLevel, false);
        //            _client.PublishAsync(message).Wait();

        //            _log.Verbose($"Published message '{topic}' [{Encoding.UTF8.GetString(payload)}].");
        //        }
        //        catch (Exception exception)
        //        {
        //            _log.Error(exception, $"Failed to publish message '{topic}' [{Encoding.UTF8.GetString(payload)}].");
        //        }
        //    }

        //    public void Subscribe(string topicPattern, Action<DeviceMessage> callback)
        //    {
        //        if (topicPattern == null) throw new ArgumentNullException(nameof(topicPattern));
        //        if (callback == null) throw new ArgumentNullException(nameof(callback));

        //        MessageReceived += (s, e) =>
        //        {
        //            if (Regex.IsMatch(e.Message.Topic, topicPattern, RegexOptions.IgnoreCase))
        //            {
        //                callback(e.Message);
        //            }
        //        };
        //    }


    }
}
