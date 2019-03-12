/**
 * UXC.Plugins.UXR
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using UXC.Plugins.UXR.Models.Uploads;
using UXI.Common;
using UXI.Common.UI;

namespace UXC.Plugins.UXR.ViewModels.Uploads
{
    public class UploadsViewModel
    {
        private readonly UploadsQueue _queue;
        private readonly IUploadsSource _uploads;
        private readonly Dispatcher _dispatcher;

        public UploadsViewModel(UploadsQueue queue, IUploadsSource uploads, Dispatcher dispatcher)
        {
            _queue = queue;
            _queue.UploadsChanged += uploads_UploadsChanged;

            _uploads = uploads;

            _dispatcher = dispatcher;

            Uploads = new ObservableCollection<UploadViewModel>(_queue.Uploads.Select(u => new UploadViewModel(u, _dispatcher)).ToList());

            Refresh();
        }

        public ObservableCollection<UploadViewModel> Uploads { get; }


        private void uploads_UploadsChanged(object sender, CollectionChangedEventArgs<Upload> e)
        {
            var addedUploads = e.AddedItems.ToList();
            var removedUploads = e.RemovedItems.ToList();

            _dispatcher.Invoke(() => UpdateUploads(addedUploads, removedUploads));
        }


        private void UpdateUploads(IEnumerable<Upload> addedUploads, IEnumerable<Upload> removedUploads)
        {
            if (addedUploads.Any())
            {
                var existingUploads = _queue.Uploads.ToList();
                foreach (var addedUpload in addedUploads.Where(u => existingUploads.Contains(u)))
                {
                    Uploads.Add(new UploadViewModel(addedUpload, _dispatcher));
                }
            }

            if (removedUploads.Any())
            {
                UploadViewModel upload;
                foreach (var removedUpload in removedUploads)
                {
                    if (Uploads.TryGet(u => u.Upload == removedUpload, out upload))
                    {
                        Uploads.Remove(upload);
                    }
                }
            }

            Save();
        }


        private RelayCommand refreshCommand = null;
        public RelayCommand RefreshCommand => refreshCommand
            ?? (refreshCommand = new RelayCommand(Refresh));

        public void Refresh()
        {
            var recordings = _uploads.Load();
            if (recordings.Any())
            {
                _queue.TryEnqueueRange(recordings);
            }
        }


        public void Save()
        {
            _uploads.Save(_queue.Uploads.Select(u => u.Recording).ToList());
        }



        //private RelayCommand<UploadViewModel> deleteCommand = null;
        //public RelayCommand<UploadViewModel> DeleteCommand => deleteCommand
        //    ?? (deleteCommand = new RelayCommand<UploadViewModel>(Delete, CanDelete));

        //private void Delete(UploadViewModel upload)
        //{

        //}

        //private bool CanDelete(UploadViewModel upload)
        //{

        //}
    }
}
