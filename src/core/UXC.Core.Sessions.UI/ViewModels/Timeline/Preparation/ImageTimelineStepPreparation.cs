/**
 * UXC.Core.Sessions.UI
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
using UXC.Sessions.Models;
using UXC.Sessions.Timeline;
using UXC.Sessions.Timeline.Actions;

namespace UXC.Sessions.ViewModels.Timeline.Preparation
{
    class ImageTimelineStepPreparation : ITimelineStepPreparation
    {
        private readonly IImageService _images;

        public ImageTimelineStepPreparation(IImageService images)
        {
            _images = images;
        }


        public Task PrepareAsync(SessionRecording recording)
        {
            var images = ExtractImagePaths(recording.Definition.PostSessionSteps)
                            .Concat(ExtractImagePaths(recording.Definition.SessionSteps))
                            .Concat(ExtractImagePaths(recording.Definition.PostSessionSteps))
                            .Select(s => s.Trim())
                            .Distinct();

            foreach (var image in images)
            {
                _images.Add(image);
            }

            return Task.FromResult(true);
        }

       
        private IEnumerable<string> ExtractImagePaths(IEnumerable<SessionStep> steps)
        {
            return steps?.Select(s => s?.Action)
                  .OfType<ImageActionSettings>()
                  .Where(s => s != null)
                  .Select(s => s.Path)
                  .Where(path => String.IsNullOrWhiteSpace(path) == false)
            ?? Enumerable.Empty<string>();
        }


        public void Reset()
        {
            _images.Clear();
        }
    }
}
