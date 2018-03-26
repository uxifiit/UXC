using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Sessions.Models
{
    public interface ISessionRecordingsDataSource
    {
        IEnumerable<SessionRecordingData> Load();
        bool Save(IEnumerable<SessionRecordingData> recordings);
    }
}
