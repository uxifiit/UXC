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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;

namespace UXC.Plugins.UXR.Models.Uploads
{
    public class UploadsQueue
    {
        private readonly ConcurrentQueue<Upload> _queue = new ConcurrentQueue<Upload>();

        public IEnumerable<Upload> Uploads => _queue.AsEnumerable();

        public event EventHandler<CollectionChangedEventArgs<Upload>> UploadsChanged;

        public bool TryEnqueue(SessionRecordingData recording)
        {
            if (_queue.Any(u => u.Recording.Path.Equals(recording.Path)) == false)
            {
                var upload = new Upload(recording.Clone());

                _queue.Enqueue(upload);

                UploadsChanged?.Invoke(this, CollectionChangedEventArgs<Upload>.CreateForAddedItem(upload));

                return true;
            }

            return false;
        }


        public bool TryPeek(out Upload upload)
        {
            return _queue.TryPeek(out upload);
        }


        public bool TryDequeue(out Upload upload)
        {
            if (_queue.TryDequeue(out upload))
            {
                UploadsChanged?.Invoke(this, CollectionChangedEventArgs<Upload>.CreateForRemovedItem(upload));

                return true;
            }

            return false;
        }


        public IEnumerable<Upload> TryEnqueueRange(IEnumerable<SessionRecordingData> recordings)
        {
            var uploads = recordings.Where(r => _queue.Any(u => u.Recording.Path.Equals(r.Path)) == false)
                                    .Select(r => new Upload(r))
                                    .ToList();

            if (uploads.Any())
            {
                uploads.ForEach(u => _queue.Enqueue(u));

                UploadsChanged?.Invoke(this, CollectionChangedEventArgs<Upload>.CreateForAddedCollection(uploads));
            }

            return uploads;
        }
    }
}
