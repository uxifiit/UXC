/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using System.Threading;

namespace UXC.Devices.Adapters.Commands
{
    // TODO update documentation for the whole class
    /// <summary>
    /// Represents a command that can be executed or undone on a <see cref="IDeviceAdapter"/>.
    /// </summary>
    public interface IDeviceCommand
    {
        /// <summary>
        /// Gets the target state which the current instance of <seealso cref="IDeviceCommand"/> attempts to get the device into.
        /// </summary>
        DeviceState TargetState { get; }

        /// <summary>
        /// Gets the action which the current instance of command represents.
        /// </summary>
        DeviceAction Action { get; }

        /// <summary>
        /// Performs actions on the <seealso cref="Adapter"/> to change its state into <seealso cref="TargetState"/>.
        /// </summary>
        /// <returns>value of <see cref="CommandResult" /> representing the result of this command.</returns>
        Task<CommandResult> ExecuteAsync(IDevice device, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the command can be executed on the device in its current state.
        /// </summary>
        /// <returns>true if the <seealso cref="Adapter"/> is in such state that <seealso cref="Do"/> can be called, otherwise false</returns>
        bool CanExecute(IDevice device);
    }
}
