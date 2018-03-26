using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Sessions;
using UXC.Sessions.Models;
using UXC.Sessions.Recording.Local;
using UXI.Common;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels
{
    public class SessionRecordingsDataViewModel : BindableBase
    {
        private readonly ISessionsControl _sessions;
        private readonly ISessionRecordingsDataSource _recordings;

        private IDisposable _subscription;

        public SessionRecordingsDataViewModel(ISessionsControl sessions, ISessionRecordingsDataSource recordings)
        {
            _sessions = sessions;

            _subscription = _sessions.CompletedRecordings
                                     .OfType<LocalSessionRecordingResult>()
                                     .ObserveOnDispatcher()
                                     .Subscribe(AddRecordingToList);

            _recordings = recordings;

            Load();
        }


        public ObservableCollection<SessionRecordingData> Recordings { get; } = new ObservableCollection<SessionRecordingData>();


        public bool HasRecordings => Recordings.Any();


        private void AddRecordingToList(LocalSessionRecordingResult result)
        {
            var data = new SessionRecordingData();
            data.Project = result.Recording.Definition.Project;
            data.Name = result.Recording.Definition.Name;
            data.Source = result.Recording.Definition.Source;
            data.StartTime = result.Recording.StartedAt ?? new DateTime();
            data.EndTime = result.Recording.FinishedAt ?? new DateTime();
            data.Path = result.RootFolder;

            Recordings.Add(data);

            OnPropertyChanged(nameof(HasRecordings));

            Save();
        }


        public void Load()
        {
            var recordings = _recordings.Load();

            Recordings.Clear();

            recordings.ForEach(Recordings.Add);

            OnPropertyChanged(nameof(HasRecordings));
        }


        public void Save()
        {
            _recordings.Save(Recordings);
        }


        private RelayCommand<SessionRecordingData> deleteCommand = null;
        public RelayCommand<SessionRecordingData> DeleteCommand => deleteCommand
            ?? (deleteCommand = new RelayCommand<SessionRecordingData>(Delete));

        private void Delete(SessionRecordingData recording)
        {
            if (Directory.Exists(recording.Path))
            {
                Directory.Delete(recording.Path, recursive: true);
            }

            Recordings.Remove(recording);

            OnPropertyChanged(nameof(HasRecordings));

            Save();
        }
    }
}
