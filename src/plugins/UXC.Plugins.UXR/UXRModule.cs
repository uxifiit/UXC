using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using UXC.Core;
using UXC.Plugins.UXR.Configuration;
using UXC.Plugins.UXR.Models;
using UXC.Plugins.UXR.Models.Uploads;
using UXC.Plugins.UXR.Models.Uploads.Design;
using UXC.Plugins.UXR.Services;
using UXC.Plugins.UXR.ViewModels;
using UXC.Plugins.UXR.ViewModels.Uploads;
using UXC.Sessions;
using UXI.Common;
using UXI.Common.UI;
using UXR.Studies.Client;

namespace UXC.Plugins.UXR
{
    public class UXRModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel.GetBindings(typeof(IUXRConfiguration)).Any() == false)
            {
                Bind<IUXRConfiguration>().To<UXRConfiguration>().InSingletonScope();
            }

            Bind<IUXRNodeContext, UXRNodeContext>().To<UXRNodeContext>().InSingletonScope();
            Bind<IOptionsTarget>().To<UXROptionsTarget>();
           

            Bind<IUXRClient>().To<UXRClient>().InSingletonScope()
                .WithConstructorArgument(typeof(Uri), ctx =>
                {
                    Uri endpointUri;
                    if (Uri.TryCreate(ctx.Kernel.Get<IUXRConfiguration>().EndpointAddress, UriKind.Absolute, out endpointUri))
                    {
                        return endpointUri;
                    }
                    return new Uri("http://localhost/");
                });

            Bind<ISessionDefinitionsSource, UXRSessionDefinitionsSource>().To<UXRSessionDefinitionsSource>().InSingletonScope();

            Bind<UXRNodeService>().ToSelf().InSingletonScope();

            Bind<IControlService>().To<UXRSessionsControlService>().InSingletonScope();
            Bind<IControlService>().To<UXRStatusUpdateControlService>().InSingletonScope();

            Bind<UXRNodeViewModel>().ToSelf().InSingletonScope();

            LoadUploads();
        }


        private void LoadUploads()
        {
            Bind<UploadsQueue>().ToSelf().InSingletonScope();

#if DEBUG
            if (DesignTimeHelper.IsDesignTime)
            {
                Bind<IUploadsSource>().To<DesignUploadsSource>().InSingletonScope();
                Bind<IUploader>().To<DesignUploader>().InSingletonScope();
            }
            else
            {
#endif
                Bind<IUploadsSource>().To<UploadsSource>().InSingletonScope();
                Bind<IUploader>().To<Uploader>().InSingletonScope();

                Bind<IControlService>().To<UXRUploaderControlService>().InSingletonScope();
#if DEBUG
            }
#endif

            Bind<UploadsViewModel>().ToSelf().InSingletonScope();
            Bind<UploaderViewModel>().ToSelf().InSingletonScope();
        }
    }
}
