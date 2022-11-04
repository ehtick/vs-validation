﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft;

/// <summary>
/// Extension methods for exceptions.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Adds data to the Data member of <paramref name="exception"/> before returning the modified exception.
    /// </summary>
    /// <typeparam name="T">The type of exception being modified.</typeparam>
    /// <param name="exception">The exception to add data to.</param>
    /// <param name="key">The key to use for the added data.</param>
    /// <param name="values">The values to add with the given <paramref name="key"/>.</param>
    /// <returns>A reference to the same <paramref name="exception"/>.</returns>
    /// <remarks>
    /// <para>This method should be used to add context (beyond the message and callstack we normally get) to the exception
    /// that would be useful when debugging Watson crashes.</para>
    /// <para>Do not use this method when you expect the exception to be handled.</para>
    /// </remarks>
    public static T AddData<T>(this T exception, string key, params object[]? values)
        where T : Exception
    {
        Requires.NotNull(exception, nameof(exception));

        if (values?.Length > 0)
        {
            exception.Data.Add(key, values);
        }

        return exception;
    }
}
