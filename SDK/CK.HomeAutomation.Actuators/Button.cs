﻿using System;
using System.Diagnostics;
using CK.HomeAutomation.Core.Timer;
using CK.HomeAutomation.Hardware;
using CK.HomeAutomation.Networking;
using CK.HomeAutomation.Notifications;

namespace CK.HomeAutomation.Actuators
{
    public class Button : ButtonBase
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        
        public Button(string id, IBinaryInput input, IHttpRequestController httpApiController, INotificationHandler notificationHandler, IHomeAutomationTimer timer)
            : base(id, httpApiController, notificationHandler)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (input == null) throw new ArgumentNullException(nameof(input));
            
            timer.Tick += CheckForTimeout;
            input.StateChanged += HandleInputStateChanged;
        }

        public TimeSpan TimeoutForPressedLongActions { get; set; } = TimeSpan.FromSeconds(1.5);

        private void HandleInputStateChanged(object sender, BinaryStateChangedEventArgs e)
        {
            if (!IsEnabled)
            {
                return;
            }

            bool buttonIsPressed = e.NewState == BinaryState.High;
            bool buttonIsReleased = e.NewState == BinaryState.Low;

            if (buttonIsPressed)
            {
                if (!IsActionForPressedLongAttached)
                {
                    OnPressedShort();
                }
                else
                {
                    _stopwatch.Restart();
                }
            }
            else if (buttonIsReleased)
            {
                if (!_stopwatch.IsRunning)
                {
                    return;
                }

                _stopwatch.Stop();
                if (_stopwatch.Elapsed >= TimeoutForPressedLongActions)
                {
                    OnPressedLong();
                }
                else
                {
                    OnPressedShort();
                }
            }
        }

        private void CheckForTimeout(object sender, TimerTickEventArgs e)
        {
            if (!_stopwatch.IsRunning)
            {
                return;
            }

            if (_stopwatch.Elapsed > TimeoutForPressedLongActions)
            {
                _stopwatch.Stop();
                OnPressedLong();
            }
        }
    }
}