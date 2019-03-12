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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;

namespace UXC.Plugins.UXR.Models.Uploads.Design
{
    public class DesignUploader : IUploader
    {
        private readonly UploadsQueue _queue;
        public DesignUploader(UploadsQueue queue)
        {
            _queue = queue;
            _queue.UploadsChanged += queue_UploadsChanged;
        }

        private void queue_UploadsChanged(object sender, CollectionChangedEventArgs<Upload> e)
        {
            if (e.AddedItems.Any() && CurrentUpload == null)
            {
                CurrentUpload = e.AddedItems.First();
                CurrentUploadChanged?.Invoke(this, CurrentUpload);
            }
        }

        public Upload CurrentUpload { get; private set; }

        public bool IsEnabled => true;

        public bool IsWorking => true;

        public event EventHandler<Upload> CurrentUploadChanged;
        public event EventHandler<bool> IsEnabledChanged { add { } remove { } }
        public event EventHandler<bool> IsWorkingChanged { add { } remove { } }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
