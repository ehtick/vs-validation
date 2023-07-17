﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using Microsoft;
using Xunit;

public class VerifyTests
{
    [Fact]
    public void Operation()
    {
        Verify.Operation(true, "Should not throw");
        Verify.Operation(true, "Should not throw", "arg1");
        Verify.Operation(true, "Should not throw", "arg1", "arg2");
        Verify.Operation(true, "Should not throw", "arg1", "arg2", "arg3");

        Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, "throw"));
        Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, "throw", "arg1"));
        Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, "throw", "arg1", "arg2"));
        Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, "throw", "arg1", "arg2", "arg3"));
    }

#if NET6_0_OR_GREATER

    [Fact]
    public void Operation_InterpolatedString()
    {
        int formatCount = 0;
        string FormattingMethod()
        {
            formatCount++;
            return "generated string";
        }

        Verify.Operation(true, $"Some {FormattingMethod()} method.");
        Assert.Equal(0, formatCount);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, $"Some {FormattingMethod()} method."));
        Assert.Equal(1, formatCount);
        Assert.StartsWith("Some generated string method.", ex.Message);
    }

#endif

    [Fact]
    public void OperationWithHelp()
    {
        Verify.OperationWithHelp(true, "message", "helpLink");
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => Verify.OperationWithHelp(false, "message", "helpLink"));
        Assert.Equal("message", ex.Message);
        Assert.Equal("helpLink", ex.HelpLink);
    }

    [Fact]
    public void NotDisposed()
    {
        Verify.NotDisposed(true, "message");
        ObjectDisposedException actualException = Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(false, "message"));
        Assert.Equal(string.Empty, actualException.ObjectName);
        Assert.Equal("message", actualException.Message);

        Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(false, null));
        Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(false, (object?)null));

        actualException = Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(false, "hi", "message"));
        string expectedObjectName = typeof(string).FullName!;
        Assert.Equal(expectedObjectName, actualException.ObjectName);
        Assert.Equal(new ObjectDisposedException(expectedObjectName, "message").Message, actualException.Message);

        actualException = Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(false, new object()));
        Assert.Equal(typeof(object).FullName, actualException.ObjectName);
    }

    [Fact]
    public void NotDisposed_Observable()
    {
        var observable = new Disposable();
        Verify.NotDisposed(observable);
        observable.Dispose();
        Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(observable));
        Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(observable, "message"));
    }

    [Fact]
    public void FailOperation_ParamsFormattingArgs()
    {
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => Verify.FailOperation("a{0}c", "b"));
        Assert.Equal("abc", ex.Message);
    }

    [Fact]
    public void FailOperation_String()
    {
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => Verify.FailOperation("a{0}c"));
        Assert.Equal("a{0}c", ex.Message);
    }

    [Fact]
    public void HResult()
    {
        const int E_INVALIDARG = unchecked((int)0x80070057);
        const int E_FAIL = unchecked((int)0x80004005);
        Verify.HResult(0);
        Assert.Throws<ArgumentException>(() => Verify.HResult(E_INVALIDARG));
        Assert.Throws<COMException>(() => Verify.HResult(E_FAIL));
        Assert.Throws<ArgumentException>(() => Verify.HResult(E_INVALIDARG, ignorePreviousComCalls: true));
        Assert.Throws<COMException>(() => Verify.HResult(E_FAIL, ignorePreviousComCalls: true));
    }

    private class Disposable : IDisposableObservable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
