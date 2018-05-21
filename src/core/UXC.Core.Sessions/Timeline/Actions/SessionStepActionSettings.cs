using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXC.Core.Data;
using UXC.Sessions.Serialization.Converters.Json;
using UXI.Common.Extensions;

namespace UXC.Sessions.Timeline.Actions
{
    public abstract class SessionStepActionSettings
    {
        private readonly static Dictionary<string, Type> Types = new Dictionary<string, Type>();
        private readonly static Type BaseType = typeof(SessionStepActionSettings);


        private static string CreateActionTypeName(Type type) => type.Name.Replace("ActionSettings", "");


        public static void Register(Type type)
        {
            Register(type, CreateActionTypeName(type));
        }


        public static void Register(Type type, string name)
        {
            type.ThrowIfNull(nameof(type))
                .ThrowIf(t => BaseType.IsAssignableFrom(t) == false, nameof(type), $"Instances of the type {type.FullName} cannot be assigned to the type {BaseType.FullName}.");

            Types.TryAdd(name.ToLower(), type);
        }
 

        public static bool TryResolve(string name, out Type type)
        {
            if (String.IsNullOrWhiteSpace(name) == false)
            {
                return Types.TryGetValue(name.Trim().ToLower(), out type);
            }

            type = null;
            return false;
        }


        private string actionType = null;
        public virtual string ActionType => actionType 
            ?? (actionType = CreateActionTypeName(this.GetType()));   // Used for serialization? 


        public virtual SessionStepActionSettings Clone()
        {
            return (SessionStepActionSettings)this.MemberwiseClone();
        }
    }



    public class ShowDesktopActionSettings : SessionStepActionSettings
    {
        public bool MinimizeAll { get; set; }
    }



    public class LaunchProgramActionSettings : SessionStepActionSettings
    {
        public string Path { get; set; }

        public string WorkingDirectoryPath { get; set; }

        public string Arguments { get; set; }

        public List<string> ArgumentsParameters { get; set; } = new List<string>();

        public bool RunInBackground { get; set; }
    }



    public class QuestionaryActionSettings : ContentActionSettinsBase
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



    public abstract class QuestionActionSettings : SessionStepActionSettings
    {
        public string Id { get; set; }

        public string Question { get; set; }

        public bool IsRequired { get; set; } = true;

        public string HelpText { get; set; }

        public override SessionStepActionSettings Clone()
        {
            return (QuestionActionSettings)this.MemberwiseClone();
        }
    }



    public class ChooseQuestionAnswerActionSettings : QuestionActionSettings
    {
        public List<string> Answers { get; set; }

        public int Limit { get; set; } = 1;

        public override SessionStepActionSettings Clone()
        {
            var clone = (ChooseQuestionAnswerActionSettings)base.Clone();

            clone.Answers = Answers?.ToList();

            return clone;
        }
    }



    public class WriteQuestionAnswerActionSettings : QuestionActionSettings
    {
        public string ValidAnswerRegexPattern { get; set; }
    }



    public class InstructionsActionSettings : ContentActionSettinsBase
    {
        public Text Instructions { get; set; }

        public bool ShowContinue { get; set; }
    }



    public class Text : IEnumerable<string>
    {
        public List<string> Lines { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Lines != null && Lines.Any()
                 ? String.Join(Environment.NewLine, Lines)
                 : String.Empty;
        }
    }



    public class WelcomeActionSettings : QuestionaryActionSettings
    {
        public bool Ignore { get; set; } 

        public string CustomTitle { get; set; }

        public bool HideDescription { get; set; }

        public Text Description { get; set; }

        public bool HideDevices { get; set; }
    }

   
     
    public abstract class ContentActionSettinsBase : SessionStepActionSettings
    {
        public string Background { get; set; }
        public string Foreground { get; set; }
        public int? FontSize { get; set; }
        public virtual bool? ShowCursor { get; set; }
    }
}
