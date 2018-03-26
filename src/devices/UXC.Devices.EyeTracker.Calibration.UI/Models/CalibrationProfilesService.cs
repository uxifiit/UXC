using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UXC.Devices.EyeTracker.Calibration;
using UXC.Devices.EyeTracker.Models;
using UXI.Common.Extensions;

namespace UXC.Devices.EyeTracker.Models
{
    public class CalibrationProfilesService : ICalibrationProfilesService
    {
        // /devices
        // /devices/ET/calibration
        // /logs
        // /sessions

        private const string ROOT_FOLDER_NAME = "calibrations";

        private const string INDEX_FILE_NAME = "index.json";

        private const string EXTENSION_CALIBRATION = ".clb";

        private static readonly string RootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.Reflection.Assembly.GetEntryAssembly().GetName().Name, "devices", "ET", ROOT_FOLDER_NAME);

        private static readonly string IndexFilePath = Path.Combine(RootFolderPath, INDEX_FILE_NAME);

        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };


        private static void EnsureRootDirectoryExists()
        {
            if (Directory.Exists(RootFolderPath) == false)
            {
                Directory.CreateDirectory(RootFolderPath);
            }
        }


        private static string GetCalibrationFilePath(CalibrationInfo info)
        {
            string filename = info.Id.ToString() + EXTENSION_CALIBRATION;
            string path = Path.Combine(RootFolderPath, filename);

            return path;
        }


        public IEnumerable<CalibrationInfo> GetStoredCalibrations()
        {
            if (File.Exists(IndexFilePath))
            {
                try
                {
                    string indexJson = File.ReadAllText(IndexFilePath);
                    var calibrations = JsonConvert.DeserializeObject<List<CalibrationInfo>>(indexJson, jsonSettings);

                    return calibrations;
                }
                catch 
                {
                }
            }

            return Enumerable.Empty<CalibrationInfo>();
        }


        private bool SaveCalibrationsIndex(IEnumerable<CalibrationInfo> calibrations)
        {
            try
            {
                var list = calibrations?.ToList() ?? new List<Models.CalibrationInfo>();

                EnsureRootDirectoryExists();

                var json = JsonConvert.SerializeObject(list, Formatting.Indented, jsonSettings);
                File.WriteAllText(IndexFilePath, json, new UTF8Encoding(false));

                return true;
            }
            catch
            {

            }

            return false;
        }


        public IEnumerable<CalibrationInfo> GetStoredCalibrations(string deviceFamilyName)
        {
            return GetStoredCalibrations().Where(c => c.DeviceFamilyName.Equals(deviceFamilyName, StringComparison.InvariantCultureIgnoreCase));
        }


        public CalibrationData LoadCalibration(CalibrationInfo calibration)
        {
            calibration.ThrowIfNull(nameof(calibration));

            string filepath = GetCalibrationFilePath(calibration);

            if (File.Exists(filepath))
            {
                byte[] bytes = File.ReadAllBytes(filepath);

                var data = new CalibrationData(calibration.DeviceFamilyName, calibration.DeviceName, bytes);

                return data;
            }

            throw new FileNotFoundException($"Calibration file {calibration.Name} does not exist at the location {filepath}", filepath);
        }


        public bool TrySaveCalibration(string name, CalibrationData calibration, out CalibrationInfo info)
        {
            var calibrations = GetStoredCalibrations();

            info = calibrations.FirstOrDefault(c => c.Name.Equals(name) && c.DeviceFamilyName.Equals(calibration.DeviceFamilyName));
            if (info == null)
            {
                info = CalibrationInfo.Create(name, calibration.DeviceFamilyName, calibration.DeviceName, DateTime.Now);
                calibrations = new List<CalibrationInfo>(calibrations.Prepend(info));
            }
            else
            {
                info.CreatedAt = DateTime.Now;
            }
                                 
            string path = GetCalibrationFilePath(info);

            EnsureRootDirectoryExists();

            File.WriteAllBytes(GetCalibrationFilePath(info), calibration.Data);

            SaveCalibrationsIndex(calibrations);
                
            return true;
        }
    }
}
