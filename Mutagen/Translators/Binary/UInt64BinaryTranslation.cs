﻿using Noggog;
using System;
using System.IO;

namespace Mutagen.Binary
{
    public class UInt64BinaryTranslation : PrimitiveBinaryTranslation<ulong>
    {
        public readonly static UInt64BinaryTranslation Instance = new UInt64BinaryTranslation();
        public override byte? ExpectedLength => 8;

        protected override ulong ParseValue(BinaryReader reader)
        {
            return reader.ReadUInt64();
        }

        protected override void WriteValue(BinaryWriter writer, ulong item)
        {
            writer.Write(item);
        }
    }
}
