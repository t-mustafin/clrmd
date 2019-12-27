﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Diagnostics.Runtime.Utilities;

namespace Microsoft.Diagnostics.Runtime.DacInterface
{
    public sealed unsafe class SOSHandleEnum : CallableCOMWrapper
    {
        private static readonly Guid IID_ISOSHandleEnum = new Guid("3E269830-4A2B-4301-8EE2-D6805B29B2FA");

        private readonly Next _next;

        public SOSHandleEnum(DacLibrary library, IntPtr pUnk)
            : base(library?.OwningLibrary, IID_ISOSHandleEnum, pUnk)
        {
            ref readonly ISOSHandleEnumVTable vtable = ref Unsafe.AsRef<ISOSHandleEnumVTable>(_vtable);
            InitDelegate(ref _next, vtable.Next);
        }

        public int ReadHandles(HandleData[] handles)
        {
            if (handles is null)
                throw new ArgumentNullException(nameof(handles));

            fixed (HandleData* ptr = handles)
            {
                int hr = _next(Self, handles.Length, ptr, out int read);
                return hr >= S_OK ? read : 0;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Next(
            IntPtr self,
            int count,
            HandleData* handles,
            out int pNeeded);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct ISOSHandleEnumVTable
    {
        private readonly IntPtr Skip;
        private readonly IntPtr Reset;
        private readonly IntPtr GetCount;
        public readonly IntPtr Next;
    }
}