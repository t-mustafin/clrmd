﻿using System;
using System.Runtime.InteropServices;

namespace Microsoft.Diagnostics.Runtime.MacOS.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct UuidLoadCommand
    {
        public LoadCommandHeader Header;
        public fixed byte Uuid[16];

        public byte[] BuildId
        {
            get
            {
                fixed (byte* ptr = Uuid)
                    return new ReadOnlySpan<byte>(ptr, 16).ToArray();
            }
        }
    }
}
