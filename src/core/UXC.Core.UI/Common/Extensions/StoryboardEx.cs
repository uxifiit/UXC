using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using UXI.Common.Helpers;

namespace UXC.Core.Common.Extensions
{
    public static class StoryboardEx
    {
        public static async Task RunAsync(this Storyboard storyboard, CancellationToken cancellationToken)
        {
            try
            {
                await AsyncHelper.InvokeAsync<EventHandler>(
                        () => storyboard.Begin(),
                        handler => storyboard.Completed += handler,
                        handler => storyboard.Completed -= handler,
                        tcs => (s, e) => tcs.TrySetResult(true),
                        cancellationToken);
            }
            catch (OperationCanceledException)
            {
                storyboard.Stop();
                throw;
            }
        }


        public static Task<bool> RunAsync(Storyboard storyboard, TimeSpan timeout)
        {
            var storyboardTask = RunAsync(storyboard, CancellationToken.None);
            var timeoutTask = Task.Delay(timeout);

            return Task.WhenAny(storyboardTask, timeoutTask)
                       .ContinueWith(t => t.Result.IsCompleted 
                                       && t.Result == storyboardTask,
                                     CancellationToken.None,
                                     TaskContinuationOptions.OnlyOnRanToCompletion, 
                                     TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
