using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.Timeline.Executors
{
    public class ShowDesktopStepActionExecutor : SessionStepActionExecutor<ShowDesktopActionSettings>
    {
        protected override void Execute(SessionRecording recording, ShowDesktopActionSettings settings)
        {
            if (settings.MinimizeAll)
            {
                TryMinimizeAllWindows();
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
                    typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, objShell, null); // Call function MinimizeAll
                }
            }
            catch (Exception ex)
            {
                // TODO LOG ex
            }
        }
    }
}
