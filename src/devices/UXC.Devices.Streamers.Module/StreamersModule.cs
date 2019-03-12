/**
 * UXC.Devices.Streamers.Module
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
using Ninject;
using Ninject.Modules;
using UXC.Devices;
using UXC.Devices.Streamers.Configuration;
using Ninject.Syntax;
using UXC.Core.Devices;

namespace UXC.Devices.Streamers.Module
{
    public class StreamersModule : NinjectModule
    {
        private IBindingToSyntax<T> TryBind<T>()
        {
            if (Kernel.GetBindings(typeof(T)).Any() == false)
            {
                return Bind<T>();
            }
            return null;
        }

        public override void Load()
        {
            TryBind<IFFmpegConfiguration>()?.To<FFmpegConfiguration>().InSingletonScope();

            TryBind<IStreamersConfiguration>()?.To<StreamersConfiguration>().InSingletonScope();
            TryBind<IAudioStreamerConfiguration>()?.To<AudioStreamerConfiguration>().InSingletonScope();
            TryBind<IVideoStreamerConfiguration>()?.To<VideoStreamerConfiguration>().InSingletonScope();
            TryBind<IScreenCastStreamerConfiguration>()?.To<ScreenCastStreamerConfiguration>().InSingletonScope();

            Bind<FFmpegHelper>().ToSelf().InSingletonScope();

            Bind<FFmpegStreamer>().ToSelf().InTransientScope();

            Bind<IDevice>().To<AudioStreamerDevice>().InSingletonScope();
            Bind<IDevice>().To<VideoStreamerDevice>().InSingletonScope();
            Bind<IDevice>().To<ScreenCastStreamerDevice>().InSingletonScope();
        }
    }
}
