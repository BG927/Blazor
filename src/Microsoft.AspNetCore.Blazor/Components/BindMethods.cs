// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Blazor.Components
{
    /// <summary>
    /// Methods used internally by @bind syntax. Not intended to be used directly.
    /// </summary>
    public static class BindMethods
    {
        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static T GetValue<T>(T value) => value;

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static string GetValue(DateTime value, string format) =>
            value == default ? null
            : (format == null ? value.ToString() : value.ToString(format));

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static string GetEventHandlerValue<T>(string value)
            where T : UIEventArgs
        {
            return value;
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static MulticastDelegate GetEventHandlerValue<T>(Action value)
            where T : UIEventArgs
        {
            return value;
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static MulticastDelegate GetEventHandlerValue<T>(Func<Task> value)
            where T : UIEventArgs
        {
            return value;
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static MulticastDelegate GetEventHandlerValue<T>(Action<T> value)
            where T : UIEventArgs
        {
            return value;
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static MulticastDelegate GetEventHandlerValue<T>(Func<T, Task> value)
            where T : UIEventArgs
        {
            return value;
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<string> setter, string existingValue)
        {
            return _ =>
            {
                // Each of the SetValueHandler overloads needs to call the setter with event handler
                // semantics, rather than just invoking the setter directly. This allows the receipient
                // to run its own event-handler logic around the setter (for example, automatically
                // re-rendering components after the setter is executed).
                // This is something that both framework and developer code needs to do when calling
                // event handler callbacks manually (e.g., child component invoking a callback passed
                // to it as a parameter).
                ComponentEvents.InvokeEventHandler(setter, (string)((UIChangeEventArgs)_).Value);
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<bool> setter, bool existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, (bool)((UIChangeEventArgs)_).Value);
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<int> setter, int existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, int.Parse((string)((UIChangeEventArgs)_).Value));
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<long> setter, long existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, long.Parse((string)((UIChangeEventArgs)_).Value));
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<float> setter, float existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, float.Parse((string)((UIChangeEventArgs)_).Value));
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<double> setter, double existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, double.Parse((string)((UIChangeEventArgs)_).Value));
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<decimal> setter, decimal existingValue)
        {
            return _ =>
            {
                ComponentEvents.InvokeEventHandler(setter, decimal.Parse((string)((UIChangeEventArgs)_).Value));
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<DateTime> setter, DateTime existingValue)
            => SetValueHandler(setter, existingValue, null);

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler(Action<DateTime> setter, DateTime existingValue, string format)
        {
            return _ =>
            {
                var stringValue = (string)((UIChangeEventArgs)_).Value;
                var dateTimeValue = string.IsNullOrEmpty(stringValue) ? default
                    : format != null && DateTime.TryParseExact(stringValue, format, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsedExact) ? parsedExact
                    : DateTime.Parse(stringValue);
                ComponentEvents.InvokeEventHandler(setter, dateTimeValue);
            };
        }

        /// <summary>
        /// Not intended to be used directly.
        /// </summary>
        public static Action<UIEventArgs> SetValueHandler<T>(Action<T> setter, T existingValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"'bind' does not accept values of type {typeof(T).FullName}. To read and write this value type, wrap it in a property of type string with suitable getters and setters.");
            }

            return _ =>
            {
                var value = (string)((UIChangeEventArgs)_).Value;
                var parsed = (T)Enum.Parse(typeof(T), value);
                ComponentEvents.InvokeEventHandler(setter, parsed);
            };
        }
    }
}
