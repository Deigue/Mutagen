﻿using Loqui;
using Loqui.Xml;
using Mutagen.Bethesda.Binary;
using Mutagen.Bethesda.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutagen.Bethesda
{
    public interface IGroupCommon<T>
        where T : ILoquiObject<T>, IMajorRecordInternal, IXmlItem, IBinaryItem
    {
        IMod SourceMod { get; }
    }

    public static class IGroupCommonExt
    {
        public static T AddNew<T>(this IGroupCommon<T> group)
            where T : ILoquiObject<T>, IMajorRecordInternal, IXmlItem, IBinaryItem
        {
            return MajorRecordInstantiator<T>.Activator(group.SourceMod.GetNextFormKey());
        }
    }
}