﻿using System;
using Wirehome.Contracts.Actuators;
using Wirehome.Contracts.Components.Commands;
using Wirehome.Contracts.Components.States;
using Wirehome.Contracts.Messaging;
using Wirehome.Contracts.Sensors;
using Wirehome.Sensors.Buttons;

namespace Wirehome.Actuators.Connectors
{
    public static class RollerShutterWithRollerShutterButtonsConnector
    {
        public static IRollerShutter ConnectWith(
            this IRollerShutter rollerShutter, 
            IButton upButton,
            IButton downButton,
            IMessageBrokerService messageBroker)
        {
            if (rollerShutter == null) throw new ArgumentNullException(nameof(rollerShutter));
            if (upButton == null) throw new ArgumentNullException(nameof(upButton));
            if (downButton == null) throw new ArgumentNullException(nameof(downButton));
            if (messageBroker == null) throw new ArgumentNullException(nameof(messageBroker));

            upButton.CreatePressedShortTrigger(messageBroker).Attach(() => HandleBlindButtonPressedEvent(rollerShutter, VerticalMovingStateValue.MovingUp));
            downButton.CreatePressedShortTrigger(messageBroker).Attach(() => HandleBlindButtonPressedEvent(rollerShutter, VerticalMovingStateValue.MovingDown));

            return rollerShutter;
        }

        private static void HandleBlindButtonPressedEvent(IRollerShutter rollerShutter, VerticalMovingStateValue verticalMovingState)
        {
            if (verticalMovingState == VerticalMovingStateValue.MovingUp && rollerShutter.GetState().Has(VerticalMovingState.MovingUp))
            {
                rollerShutter.ExecuteCommand(new TurnOffCommand());
            }
            else if (verticalMovingState == VerticalMovingStateValue.MovingDown && rollerShutter.GetState().Has(VerticalMovingState.MovingDown))
            {
                rollerShutter.ExecuteCommand(new TurnOffCommand());
            }
            else if (verticalMovingState == VerticalMovingStateValue.MovingDown)
            {
                rollerShutter.ExecuteCommand(new MoveDownCommand());
            }
            else if (verticalMovingState == VerticalMovingStateValue.MovingUp)
            {
                rollerShutter.ExecuteCommand(new MoveUpCommand());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}