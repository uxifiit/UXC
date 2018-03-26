using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public interface IUploadsSource
    {
        IEnumerable<SessionRecordingData> Load();
        bool Save(IEnumerable<SessionRecordingData> recordings);
    }
}
