/**
 * UXC.Core.Sessions
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Helpers;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Executors
{
    public class ShowDesktopStepActionExecutor : SessionStepActionExecutor<ShowDesktopActionSettings>
    {
        private WindowsTaskbarHelper _taskbar;

        protected override void Execute(SessionRecording recording, ShowDesktopActionSettings settings)
        {
            if (settings.MinimizeAll)
            {
                TryMinimizeAllWindows();
            }

            if (settings.ShowTaskbar.HasValue)
            {
                var taskbar = new WindowsTaskbarHelper();
                _taskbar = taskbar;
                if (settings.ShowTaskbar.Value)
                {
                    Task.Run(() => taskbar.Show()).Forget();
                }
                else
                {
                    Task.Run(() => taskbar.Hide()).Forget();
                }
            }
        }

        private static void TryMinimizeAllWindows()
        {
            try
            {
                Type typeShell = Type.GetTypeFromProgID("Shell.Application");
                if (typeShell != null)
                {
                    object objShell = Activator.CreateInstance(typeShell);

                    // Call function MinimizeAll
                    typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, objShell, null); 
                }
            }
            catch (Exception ex)
            {
                // TODO LOG ex
            }
        }

        public override SessionStepResult Complete()
        {
            // reset taskbar state to the previous value
            var taskbar = ObjectEx.GetAndReplace(ref _taskbar, null);
            if (taskbar != null)
            {
                Task.Run(() => taskbar.Reset()).Forget();
            }

            return base.Complete();
        }
    }
}
