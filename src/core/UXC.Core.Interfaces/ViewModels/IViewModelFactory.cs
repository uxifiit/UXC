/**
 * UXC.Core.Interfaces
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU Lesser General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: LGPL-3.0-only
 */
using System;
using UXI.Common.Extensions;

namespace UXC.Core.ViewModels
{
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates a ViewModel for the passed <seealso cref="source"/> object.
        /// </summary>
        /// <param name="source">source object that the ViewModel is created for of the type same as specified in <seealso cref="SourceType"/>.</param>
        /// <returns>ViewModel instance of the type same as specified in <seealso cref="ViewModelType"/>.</returns>
        object Create(object source);

        /// <summary>
        /// Gets the type of ViewModels produced by this <seealso cref="IViewModelFactory"/> implementation.
        /// </summary>
        Type ViewModelType { get; }

        /// <summary>
        /// Gets the type of source objects which this <seealso cref="IViewModelFactory"/> creates ViewModels for.
        /// </summary>
        Type SourceType { get; }
    }


    public abstract class ViewModelFactory<TSource, TResult> : IViewModelFactory
    {
        public Type SourceType { get; } = typeof(TSource);

        public Type ViewModelType { get; } = typeof(TResult);

        public object Create(object source)
        {
            source.ThrowIfNull(nameof(source))
                  .ThrowIf(s => s.GetType().IsInstanceOfType(SourceType), nameof(source), $"The type of the source does not match the source type: {source.GetType()} and {SourceType.FullName}");

            return CreateInternal((TSource)source);
        }

        protected abstract TResult CreateInternal(TSource source);
    }


    public class RelayViewModelFactory<TSource, TResult> : ViewModelFactory<TSource, TResult>
    {
        private readonly Func<TSource, TResult> _activator;

        public RelayViewModelFactory(Func<TSource, TResult> activator)
        {
            _activator = activator;
        }

        protected sealed override TResult CreateInternal(TSource source) => _activator.Invoke(source);
    }
}
