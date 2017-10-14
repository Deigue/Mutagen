﻿using Noggog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutagen.Binary
{
    public abstract class TypicalBinaryTranslation<T> : IBinaryTranslation<T, Exception>
        where T : class
    {
        protected abstract T ParseBytes(byte[] bytes);

        protected abstract T ParseValue(BinaryReader reader, int length);

        public TryGet<T> Parse(BinaryReader reader, int length, bool doMasks, out Exception errorMask)
        {
            try
            {
                var parse = ParseValue(reader, length);
                if (parse == null)
                {
                    throw new ArgumentException("Value was unexpectedly null.");
                }
                errorMask = null;
                return TryGet<T>.Succeed(parse);
            }
            catch (Exception ex)
            {
                if (doMasks)
                {
                    errorMask = ex;
                    return TryGet<T>.Failure;
                }
                throw;
            }
        }

        protected abstract void WriteValue(BinaryWriter writer, T item);

        TryGet<T> IBinaryTranslation<T, Exception>.Parse(BinaryReader reader, int length, bool doMasks, out Exception errorMask)
        {
            return Parse(reader, length: length, doMasks: doMasks, errorMask: out errorMask);
        }

        public void Write(BinaryWriter writer, T item, bool doMasks, out Exception errorMask)
        {
            try
            {
                WriteValue(writer, item);
                errorMask = null;
            }
            catch (Exception ex)
            when (doMasks)
            {
                errorMask = ex;
            }
        }

        void IBinaryTranslation<T, Exception>.Write(BinaryWriter writer, T item, int length, bool doMasks, out Exception maskObj)
        {
            Write(writer, item, doMasks, out maskObj);
        }

        public void Write(
            BinaryWriter writer,
            T item,
            RecordType header,
            bool nullable,
            bool doMasks,
            out Exception errorMask)
        {
            if (item == null)
            {
                if (nullable)
                {
                    errorMask = null;
                    return;
                }
                throw new ArgumentException("Non optional string was null.");
            }
            try
            {
                using (HeaderExport.ExportHeader(writer, header, ObjectType.Subrecord))
                {
                    WriteValue(writer, item);
                    errorMask = null;
                }
            }
            catch (Exception ex)
            when (doMasks)
            {
                errorMask = ex;
            }
        }
    }
}
