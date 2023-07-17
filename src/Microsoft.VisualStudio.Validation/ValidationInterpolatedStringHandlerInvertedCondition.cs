﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft;

/// <summary>Provides an interpolated string handler for validation functions that only perform formatting if the condition check fails.</summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[InterpolatedStringHandler]
public ref struct ValidationInterpolatedStringHandlerInvertedCondition
{
    /// <summary>The handler we use to perform the formatting.</summary>
    private StringBuilder.AppendInterpolatedStringHandler stringBuilderHandler;
    private StringBuilder? stringBuilder;

    /// <summary>Initializes a new instance of the <see cref="ValidationInterpolatedStringHandlerInvertedCondition"/> struct.</summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="condition">The condition Boolean passed to the method.</param>
    /// <param name="shouldAppend">A value indicating whether formatting should proceed.</param>
    /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.</remarks>
    public ValidationInterpolatedStringHandlerInvertedCondition(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
    {
        if (!condition)
        {
            shouldAppend = false;
        }
        else
        {
            // Only used when failing an assert.  Additional allocation here doesn't matter; just create a new StringBuilder.
            this.stringBuilderHandler = new StringBuilder.AppendInterpolatedStringHandler(literalLength, formattedCount, this.stringBuilder = new StringBuilder());
            shouldAppend = true;
        }
    }

    /// <summary>Writes the specified string to the handler.</summary>
    /// <param name="value">The string to write.</param>
    public void AppendLiteral(string value) => this.stringBuilderHandler.AppendLiteral(value);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value) => this.stringBuilderHandler.AppendFormatted(value);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format string.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value, string? format) => this.stringBuilderHandler.AppendFormatted(value, format);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value, int alignment) => this.stringBuilderHandler.AppendFormatted(value, alignment);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public void AppendFormatted<T>(T value, int alignment, string? format) => this.stringBuilderHandler.AppendFormatted(value, alignment, format);

    /// <summary>Writes the specified character span to the handler.</summary>
    /// <param name="value">The span to write.</param>
    public void AppendFormatted(ReadOnlySpan<char> value) => this.stringBuilderHandler.AppendFormatted(value);

    /// <summary>Writes the specified string of chars to the handler.</summary>
    /// <param name="value">The span to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null) => this.stringBuilderHandler.AppendFormatted(value, alignment, format);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    public void AppendFormatted(string? value) => this.stringBuilderHandler.AppendFormatted(value);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null) => this.stringBuilderHandler.AppendFormatted(value, alignment, format);

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted(object? value, int alignment = 0, string? format = null) => this.stringBuilderHandler.AppendFormatted(value, alignment, format);

    /// <summary>Extracts the built string from the handler.</summary>
    /// <returns>The formatted string.</returns>
    internal string ToStringAndClear()
    {
        string s = this.stringBuilder?.ToString() ?? string.Empty;
        this.stringBuilder = null;
        this.stringBuilderHandler = default;
        return s;
    }
}

#endif
