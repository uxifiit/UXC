using System.Threading.Tasks;

namespace UXC.Sessions.ViewModels.Timeline.Preparation
{
    public interface ITimelineStepPreparation
    {
        Task PrepareAsync(SessionRecording recording);

        void Reset();
    }
}