using System.Collections.Generic;
using System.Linq;

namespace UXC.Sessions.Timeline.Actions
{
    public class QuestionaryActionSettings : ContentActionSettingsBase
    {
        public string Id { get; set; }

        public List<QuestionActionSettings> Questions { get; set; }

        public override SessionStepActionSettings Clone()
        {
            var clone = (QuestionaryActionSettings)base.Clone();

            clone.Questions = Questions?.Select(q => (QuestionActionSettings)q.Clone()).ToList();

            return clone;
        }
    }

   
   
}
