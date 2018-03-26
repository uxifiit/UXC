using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Devices;
using UXI.Common.Helpers;

namespace UXC.Sessions.Recording.Local
{
    public class SessionRecordingPathsBuilder
    {
        private readonly DirectoryInfo _rootDirectory;
        private readonly List<string> _paths = new List<string>();

        public SessionRecordingPathsBuilder(string rootPath)
        {
            _rootDirectory = new DirectoryInfo(rootPath.TrimEnd());
        }

        public SessionRecordingPathsBuilder(string path, SessionRecording session)
        {
            string sessionFolder = FilenameHelper.ReplaceInvalidFileNameChars(session.Id);
            var rootPath = Path.Combine(path, sessionFolder);
            _rootDirectory = new DirectoryInfo(rootPath.TrimEnd());
        }


        public void Setup()
        {
            if (_rootDirectory.Exists == false)
            {
                try
                {
                    _rootDirectory.Create();

                    _rootDirectory.Refresh();
                }
                catch
                {
                    // TODO logging
                }
            }
        }


        public void Clean()
        {
            _rootDirectory.Refresh();

            if (_rootDirectory.Exists)
            {
                try
                {
                    _rootDirectory.Delete(recursive: true);
                }
                catch
                {
                    // TODO logging
                }
            }
        }


        public string RootPath => _rootDirectory.FullName;


        public IEnumerable<string> Paths => _paths.AsEnumerable();


        public string BuildFilePath(string name, string format)
        {
            string filename = $"{name}.{format.Trim('.')}";

            string path = Path.Combine(RootPath, FilenameHelper.ReplaceInvalidFileNameChars(filename));

            _paths.Add(path);

            return path;
        }


        public string BuildDeviceFilePath(DeviceType deviceType, string tag, string extension)
        {
            string filename = $"{deviceType.Code}_{tag}.{extension.Trim('.')}";

            string path = Path.Combine(RootPath, FilenameHelper.ReplaceInvalidFileNameChars(filename));

            _paths.Add(path);

            return path;
        }
    }
}
