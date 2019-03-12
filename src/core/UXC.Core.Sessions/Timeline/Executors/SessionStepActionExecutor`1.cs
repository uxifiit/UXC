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
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;

namespace UXC.Sessions.Timeline.Executors
{
    public abstract class SessionStepActionExecutor<TSettings> : ISessionStepActionExecutor
        where TSettings : SessionStepActionSettings
    {
        public bool CanExecute(SessionStepActionSettings settings)
        {
            return settings != null && settings.GetType().Equals(typeof(TSettings));
        }


        public void Execute(SessionRecording recording, SessionStepActionSettings settings)
        {
            Execute(recording, (TSettings)settings);
        }


        protected abstract void Execute(SessionRecording recording, TSettings settings);


        public virtual SessionStepResult Complete()
        {
            var result = SessionStepResult.Successful;

            OnCompleted(result);

            return result;
        }


        public event EventHandler<SessionStepResult> Completed;

        protected virtual void OnCompleted(SessionStepResult result)
        {
            Completed?.Invoke(this, result);
        }
    }
}
