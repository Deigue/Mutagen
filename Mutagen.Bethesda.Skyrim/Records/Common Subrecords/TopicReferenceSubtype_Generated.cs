/*
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 * Autogenerated by Loqui.  Do not manually change.
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
*/
#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loqui;
using Loqui.Internal;
using Noggog;
using Mutagen.Bethesda.Skyrim.Internals;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Mutagen.Bethesda.Skyrim;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Mutagen.Bethesda.Binary;
using System.Buffers.Binary;
using Mutagen.Bethesda.Internals;
#endregion

#nullable enable
namespace Mutagen.Bethesda.Skyrim
{
    #region Class
    public partial class TopicReferenceSubtype :
        ATopicReference,
        ITopicReferenceSubtype,
        ILoquiObjectSetter<TopicReferenceSubtype>,
        IEquatable<ITopicReferenceSubtypeGetter>
    {
        #region Ctor
        public TopicReferenceSubtype()
        {
            CustomCtor();
        }
        partial void CustomCtor();
        #endregion

        #region Subtype
        public RecordType Subtype { get; set; } = RecordType.Null;
        #endregion

        #region To String

        public override void ToString(
            FileGeneration fg,
            string? name = null)
        {
            TopicReferenceSubtypeMixIn.ToString(
                item: this,
                name: name);
        }

        #endregion

        #region Equals and Hash
        public override bool Equals(object? obj)
        {
            if (!(obj is ITopicReferenceSubtypeGetter rhs)) return false;
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).Equals(this, rhs);
        }

        public bool Equals(ITopicReferenceSubtypeGetter? obj)
        {
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).Equals(this, obj);
        }

        public override int GetHashCode() => ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).GetHashCode(this);

        #endregion

        #region Mask
        public new class Mask<TItem> :
            ATopicReference.Mask<TItem>,
            IMask<TItem>,
            IEquatable<Mask<TItem>>
        {
            #region Ctors
            public Mask(TItem Subtype)
            : base()
            {
                this.Subtype = Subtype;
            }

            #pragma warning disable CS8618
            protected Mask()
            {
            }
            #pragma warning restore CS8618

            #endregion

            #region Members
            public TItem Subtype;
            #endregion

            #region Equals
            public override bool Equals(object? obj)
            {
                if (!(obj is Mask<TItem> rhs)) return false;
                return Equals(rhs);
            }

            public bool Equals(Mask<TItem>? rhs)
            {
                if (rhs == null) return false;
                if (!base.Equals(rhs)) return false;
                if (!object.Equals(this.Subtype, rhs.Subtype)) return false;
                return true;
            }
            public override int GetHashCode()
            {
                var hash = new HashCode();
                hash.Add(this.Subtype);
                hash.Add(base.GetHashCode());
                return hash.ToHashCode();
            }

            #endregion

            #region All
            public override bool All(Func<TItem, bool> eval)
            {
                if (!base.All(eval)) return false;
                if (!eval(this.Subtype)) return false;
                return true;
            }
            #endregion

            #region Any
            public override bool Any(Func<TItem, bool> eval)
            {
                if (base.Any(eval)) return true;
                if (eval(this.Subtype)) return true;
                return false;
            }
            #endregion

            #region Translate
            public new Mask<R> Translate<R>(Func<TItem, R> eval)
            {
                var ret = new TopicReferenceSubtype.Mask<R>();
                this.Translate_InternalFill(ret, eval);
                return ret;
            }

            protected void Translate_InternalFill<R>(Mask<R> obj, Func<TItem, R> eval)
            {
                base.Translate_InternalFill(obj, eval);
                obj.Subtype = eval(this.Subtype);
            }
            #endregion

            #region To String
            public override string ToString()
            {
                return ToString(printMask: null);
            }

            public string ToString(TopicReferenceSubtype.Mask<bool>? printMask = null)
            {
                var fg = new FileGeneration();
                ToString(fg, printMask);
                return fg.ToString();
            }

            public void ToString(FileGeneration fg, TopicReferenceSubtype.Mask<bool>? printMask = null)
            {
                fg.AppendLine($"{nameof(TopicReferenceSubtype.Mask<TItem>)} =>");
                fg.AppendLine("[");
                using (new DepthWrapper(fg))
                {
                    if (printMask?.Subtype ?? true)
                    {
                        fg.AppendItem(Subtype, "Subtype");
                    }
                }
                fg.AppendLine("]");
            }
            #endregion

        }

        public new class ErrorMask :
            ATopicReference.ErrorMask,
            IErrorMask<ErrorMask>
        {
            #region Members
            public Exception? Subtype;
            #endregion

            #region IErrorMask
            public override object? GetNthMask(int index)
            {
                TopicReferenceSubtype_FieldIndex enu = (TopicReferenceSubtype_FieldIndex)index;
                switch (enu)
                {
                    case TopicReferenceSubtype_FieldIndex.Subtype:
                        return Subtype;
                    default:
                        return base.GetNthMask(index);
                }
            }

            public override void SetNthException(int index, Exception ex)
            {
                TopicReferenceSubtype_FieldIndex enu = (TopicReferenceSubtype_FieldIndex)index;
                switch (enu)
                {
                    case TopicReferenceSubtype_FieldIndex.Subtype:
                        this.Subtype = ex;
                        break;
                    default:
                        base.SetNthException(index, ex);
                        break;
                }
            }

            public override void SetNthMask(int index, object obj)
            {
                TopicReferenceSubtype_FieldIndex enu = (TopicReferenceSubtype_FieldIndex)index;
                switch (enu)
                {
                    case TopicReferenceSubtype_FieldIndex.Subtype:
                        this.Subtype = (Exception?)obj;
                        break;
                    default:
                        base.SetNthMask(index, obj);
                        break;
                }
            }

            public override bool IsInError()
            {
                if (Overall != null) return true;
                if (Subtype != null) return true;
                return false;
            }
            #endregion

            #region To String
            public override string ToString()
            {
                var fg = new FileGeneration();
                ToString(fg, null);
                return fg.ToString();
            }

            public override void ToString(FileGeneration fg, string? name = null)
            {
                fg.AppendLine($"{(name ?? "ErrorMask")} =>");
                fg.AppendLine("[");
                using (new DepthWrapper(fg))
                {
                    if (this.Overall != null)
                    {
                        fg.AppendLine("Overall =>");
                        fg.AppendLine("[");
                        using (new DepthWrapper(fg))
                        {
                            fg.AppendLine($"{this.Overall}");
                        }
                        fg.AppendLine("]");
                    }
                    ToString_FillInternal(fg);
                }
                fg.AppendLine("]");
            }
            protected override void ToString_FillInternal(FileGeneration fg)
            {
                base.ToString_FillInternal(fg);
                fg.AppendItem(Subtype, "Subtype");
            }
            #endregion

            #region Combine
            public ErrorMask Combine(ErrorMask? rhs)
            {
                if (rhs == null) return this;
                var ret = new ErrorMask();
                ret.Subtype = this.Subtype.Combine(rhs.Subtype);
                return ret;
            }
            public static ErrorMask? Combine(ErrorMask? lhs, ErrorMask? rhs)
            {
                if (lhs != null && rhs != null) return lhs.Combine(rhs);
                return lhs ?? rhs;
            }
            #endregion

            #region Factory
            public static new ErrorMask Factory(ErrorMaskBuilder errorMask)
            {
                return new ErrorMask();
            }
            #endregion

        }
        public new class TranslationMask :
            ATopicReference.TranslationMask,
            ITranslationMask
        {
            #region Members
            public bool Subtype;
            #endregion

            #region Ctors
            public TranslationMask(bool defaultOn)
                : base(defaultOn)
            {
                this.Subtype = defaultOn;
            }

            #endregion

            protected override void GetCrystal(List<(bool On, TranslationCrystal? SubCrystal)> ret)
            {
                base.GetCrystal(ret);
                ret.Add((Subtype, null));
            }

            public static implicit operator TranslationMask(bool defaultOn)
            {
                return new TranslationMask(defaultOn);
            }

        }
        #endregion

        #region Mutagen
        public static readonly RecordType GrupRecordType = TopicReferenceSubtype_Registration.TriggeringRecordType;
        #endregion

        #region Binary Translation
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override object BinaryWriteTranslator => TopicReferenceSubtypeBinaryWriteTranslation.Instance;
        void IBinaryItem.WriteToBinary(
            MutagenWriter writer,
            RecordTypeConverter? recordTypeConverter = null)
        {
            ((TopicReferenceSubtypeBinaryWriteTranslation)this.BinaryWriteTranslator).Write(
                item: this,
                writer: writer,
                recordTypeConverter: recordTypeConverter);
        }
        #region Binary Create
        public new static TopicReferenceSubtype CreateFromBinary(
            MutagenFrame frame,
            RecordTypeConverter? recordTypeConverter = null)
        {
            var ret = new TopicReferenceSubtype();
            ((TopicReferenceSubtypeSetterCommon)((ITopicReferenceSubtypeGetter)ret).CommonSetterInstance()!).CopyInFromBinary(
                item: ret,
                frame: frame,
                recordTypeConverter: recordTypeConverter);
            return ret;
        }

        #endregion

        public static bool TryCreateFromBinary(
            MutagenFrame frame,
            out TopicReferenceSubtype item,
            RecordTypeConverter? recordTypeConverter = null)
        {
            var startPos = frame.Position;
            item = CreateFromBinary(frame, recordTypeConverter);
            return startPos != frame.Position;
        }
        #endregion

        void IPrintable.ToString(FileGeneration fg, string? name) => this.ToString(fg, name);

        void IClearable.Clear()
        {
            ((TopicReferenceSubtypeSetterCommon)((ITopicReferenceSubtypeGetter)this).CommonSetterInstance()!).Clear(this);
        }

        internal static new TopicReferenceSubtype GetNew()
        {
            return new TopicReferenceSubtype();
        }

    }
    #endregion

    #region Interface
    public partial interface ITopicReferenceSubtype :
        ITopicReferenceSubtypeGetter,
        IATopicReference,
        ILoquiObjectSetter<ITopicReferenceSubtype>
    {
        new RecordType Subtype { get; set; }
    }

    public partial interface ITopicReferenceSubtypeGetter :
        IATopicReferenceGetter,
        ILoquiObject<ITopicReferenceSubtypeGetter>,
        IBinaryItem
    {
        static new ILoquiRegistration Registration => TopicReferenceSubtype_Registration.Instance;
        RecordType Subtype { get; }

    }

    #endregion

    #region Common MixIn
    public static partial class TopicReferenceSubtypeMixIn
    {
        public static void Clear(this ITopicReferenceSubtype item)
        {
            ((TopicReferenceSubtypeSetterCommon)((ITopicReferenceSubtypeGetter)item).CommonSetterInstance()!).Clear(item: item);
        }

        public static TopicReferenceSubtype.Mask<bool> GetEqualsMask(
            this ITopicReferenceSubtypeGetter item,
            ITopicReferenceSubtypeGetter rhs,
            EqualsMaskHelper.Include include = EqualsMaskHelper.Include.All)
        {
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).GetEqualsMask(
                item: item,
                rhs: rhs,
                include: include);
        }

        public static string ToString(
            this ITopicReferenceSubtypeGetter item,
            string? name = null,
            TopicReferenceSubtype.Mask<bool>? printMask = null)
        {
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).ToString(
                item: item,
                name: name,
                printMask: printMask);
        }

        public static void ToString(
            this ITopicReferenceSubtypeGetter item,
            FileGeneration fg,
            string? name = null,
            TopicReferenceSubtype.Mask<bool>? printMask = null)
        {
            ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).ToString(
                item: item,
                fg: fg,
                name: name,
                printMask: printMask);
        }

        public static bool Equals(
            this ITopicReferenceSubtypeGetter item,
            ITopicReferenceSubtypeGetter rhs)
        {
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).Equals(
                lhs: item,
                rhs: rhs);
        }

        public static void DeepCopyIn(
            this ITopicReferenceSubtype lhs,
            ITopicReferenceSubtypeGetter rhs,
            out TopicReferenceSubtype.ErrorMask errorMask,
            TopicReferenceSubtype.TranslationMask? copyMask = null)
        {
            var errorMaskBuilder = new ErrorMaskBuilder();
            ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)lhs).CommonSetterTranslationInstance()!).DeepCopyIn(
                item: lhs,
                rhs: rhs,
                errorMask: errorMaskBuilder,
                copyMask: copyMask?.GetCrystal(),
                deepCopy: false);
            errorMask = TopicReferenceSubtype.ErrorMask.Factory(errorMaskBuilder);
        }

        public static void DeepCopyIn(
            this ITopicReferenceSubtype lhs,
            ITopicReferenceSubtypeGetter rhs,
            ErrorMaskBuilder? errorMask,
            TranslationCrystal? copyMask)
        {
            ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)lhs).CommonSetterTranslationInstance()!).DeepCopyIn(
                item: lhs,
                rhs: rhs,
                errorMask: errorMask,
                copyMask: copyMask,
                deepCopy: false);
        }

        public static TopicReferenceSubtype DeepCopy(
            this ITopicReferenceSubtypeGetter item,
            TopicReferenceSubtype.TranslationMask? copyMask = null)
        {
            return ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)item).CommonSetterTranslationInstance()!).DeepCopy(
                item: item,
                copyMask: copyMask);
        }

        public static TopicReferenceSubtype DeepCopy(
            this ITopicReferenceSubtypeGetter item,
            out TopicReferenceSubtype.ErrorMask errorMask,
            TopicReferenceSubtype.TranslationMask? copyMask = null)
        {
            return ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)item).CommonSetterTranslationInstance()!).DeepCopy(
                item: item,
                copyMask: copyMask,
                errorMask: out errorMask);
        }

        public static TopicReferenceSubtype DeepCopy(
            this ITopicReferenceSubtypeGetter item,
            ErrorMaskBuilder? errorMask,
            TranslationCrystal? copyMask = null)
        {
            return ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)item).CommonSetterTranslationInstance()!).DeepCopy(
                item: item,
                copyMask: copyMask,
                errorMask: errorMask);
        }

        #region Binary Translation
        public static void CopyInFromBinary(
            this ITopicReferenceSubtype item,
            MutagenFrame frame,
            RecordTypeConverter? recordTypeConverter = null)
        {
            ((TopicReferenceSubtypeSetterCommon)((ITopicReferenceSubtypeGetter)item).CommonSetterInstance()!).CopyInFromBinary(
                item: item,
                frame: frame,
                recordTypeConverter: recordTypeConverter);
        }

        #endregion

    }
    #endregion

}

namespace Mutagen.Bethesda.Skyrim.Internals
{
    #region Field Index
    public enum TopicReferenceSubtype_FieldIndex
    {
        Subtype = 0,
    }
    #endregion

    #region Registration
    public partial class TopicReferenceSubtype_Registration : ILoquiRegistration
    {
        public static readonly TopicReferenceSubtype_Registration Instance = new TopicReferenceSubtype_Registration();

        public static ProtocolKey ProtocolKey => ProtocolDefinition_Skyrim.ProtocolKey;

        public static readonly ObjectKey ObjectKey = new ObjectKey(
            protocolKey: ProtocolDefinition_Skyrim.ProtocolKey,
            msgID: 313,
            version: 0);

        public const string GUID = "dcc1faed-8bae-45b8-bff2-a195b884c570";

        public const ushort AdditionalFieldCount = 1;

        public const ushort FieldCount = 1;

        public static readonly Type MaskType = typeof(TopicReferenceSubtype.Mask<>);

        public static readonly Type ErrorMaskType = typeof(TopicReferenceSubtype.ErrorMask);

        public static readonly Type ClassType = typeof(TopicReferenceSubtype);

        public static readonly Type GetterType = typeof(ITopicReferenceSubtypeGetter);

        public static readonly Type? InternalGetterType = null;

        public static readonly Type SetterType = typeof(ITopicReferenceSubtype);

        public static readonly Type? InternalSetterType = null;

        public const string FullName = "Mutagen.Bethesda.Skyrim.TopicReferenceSubtype";

        public const string Name = "TopicReferenceSubtype";

        public const string Namespace = "Mutagen.Bethesda.Skyrim";

        public const byte GenericCount = 0;

        public static readonly Type? GenericRegistrationType = null;

        public static readonly RecordType TriggeringRecordType = RecordTypes.PDTO;
        public static readonly Type BinaryWriteTranslation = typeof(TopicReferenceSubtypeBinaryWriteTranslation);
        #region Interface
        ProtocolKey ILoquiRegistration.ProtocolKey => ProtocolKey;
        ObjectKey ILoquiRegistration.ObjectKey => ObjectKey;
        string ILoquiRegistration.GUID => GUID;
        ushort ILoquiRegistration.FieldCount => FieldCount;
        ushort ILoquiRegistration.AdditionalFieldCount => AdditionalFieldCount;
        Type ILoquiRegistration.MaskType => MaskType;
        Type ILoquiRegistration.ErrorMaskType => ErrorMaskType;
        Type ILoquiRegistration.ClassType => ClassType;
        Type ILoquiRegistration.SetterType => SetterType;
        Type? ILoquiRegistration.InternalSetterType => InternalSetterType;
        Type ILoquiRegistration.GetterType => GetterType;
        Type? ILoquiRegistration.InternalGetterType => InternalGetterType;
        string ILoquiRegistration.FullName => FullName;
        string ILoquiRegistration.Name => Name;
        string ILoquiRegistration.Namespace => Namespace;
        byte ILoquiRegistration.GenericCount => GenericCount;
        Type? ILoquiRegistration.GenericRegistrationType => GenericRegistrationType;
        ushort? ILoquiRegistration.GetNameIndex(StringCaseAgnostic name) => throw new NotImplementedException();
        bool ILoquiRegistration.GetNthIsEnumerable(ushort index) => throw new NotImplementedException();
        bool ILoquiRegistration.GetNthIsLoqui(ushort index) => throw new NotImplementedException();
        bool ILoquiRegistration.GetNthIsSingleton(ushort index) => throw new NotImplementedException();
        string ILoquiRegistration.GetNthName(ushort index) => throw new NotImplementedException();
        bool ILoquiRegistration.IsNthDerivative(ushort index) => throw new NotImplementedException();
        bool ILoquiRegistration.IsProtected(ushort index) => throw new NotImplementedException();
        Type ILoquiRegistration.GetNthType(ushort index) => throw new NotImplementedException();
        #endregion

    }
    #endregion

    #region Common
    public partial class TopicReferenceSubtypeSetterCommon : ATopicReferenceSetterCommon
    {
        public new static readonly TopicReferenceSubtypeSetterCommon Instance = new TopicReferenceSubtypeSetterCommon();

        partial void ClearPartial();
        
        public void Clear(ITopicReferenceSubtype item)
        {
            ClearPartial();
            item.Subtype = RecordType.Null;
            base.Clear(item);
        }
        
        public override void Clear(IATopicReference item)
        {
            Clear(item: (ITopicReferenceSubtype)item);
        }
        
        #region Binary Translation
        public virtual void CopyInFromBinary(
            ITopicReferenceSubtype item,
            MutagenFrame frame,
            RecordTypeConverter? recordTypeConverter = null)
        {
            UtilityTranslation.SubrecordParse(
                record: item,
                frame: frame,
                recordTypeConverter: recordTypeConverter,
                fillStructs: TopicReferenceSubtypeBinaryCreateTranslation.FillBinaryStructs);
        }
        
        public override void CopyInFromBinary(
            IATopicReference item,
            MutagenFrame frame,
            RecordTypeConverter? recordTypeConverter = null)
        {
            CopyInFromBinary(
                item: (TopicReferenceSubtype)item,
                frame: frame,
                recordTypeConverter: recordTypeConverter);
        }
        
        #endregion
        
    }
    public partial class TopicReferenceSubtypeCommon : ATopicReferenceCommon
    {
        public new static readonly TopicReferenceSubtypeCommon Instance = new TopicReferenceSubtypeCommon();

        public TopicReferenceSubtype.Mask<bool> GetEqualsMask(
            ITopicReferenceSubtypeGetter item,
            ITopicReferenceSubtypeGetter rhs,
            EqualsMaskHelper.Include include = EqualsMaskHelper.Include.All)
        {
            var ret = new TopicReferenceSubtype.Mask<bool>(false);
            ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).FillEqualsMask(
                item: item,
                rhs: rhs,
                ret: ret,
                include: include);
            return ret;
        }
        
        public void FillEqualsMask(
            ITopicReferenceSubtypeGetter item,
            ITopicReferenceSubtypeGetter rhs,
            TopicReferenceSubtype.Mask<bool> ret,
            EqualsMaskHelper.Include include = EqualsMaskHelper.Include.All)
        {
            if (rhs == null) return;
            ret.Subtype = item.Subtype == rhs.Subtype;
            base.FillEqualsMask(item, rhs, ret, include);
        }
        
        public string ToString(
            ITopicReferenceSubtypeGetter item,
            string? name = null,
            TopicReferenceSubtype.Mask<bool>? printMask = null)
        {
            var fg = new FileGeneration();
            ToString(
                item: item,
                fg: fg,
                name: name,
                printMask: printMask);
            return fg.ToString();
        }
        
        public void ToString(
            ITopicReferenceSubtypeGetter item,
            FileGeneration fg,
            string? name = null,
            TopicReferenceSubtype.Mask<bool>? printMask = null)
        {
            if (name == null)
            {
                fg.AppendLine($"TopicReferenceSubtype =>");
            }
            else
            {
                fg.AppendLine($"{name} (TopicReferenceSubtype) =>");
            }
            fg.AppendLine("[");
            using (new DepthWrapper(fg))
            {
                ToStringFields(
                    item: item,
                    fg: fg,
                    printMask: printMask);
            }
            fg.AppendLine("]");
        }
        
        protected static void ToStringFields(
            ITopicReferenceSubtypeGetter item,
            FileGeneration fg,
            TopicReferenceSubtype.Mask<bool>? printMask = null)
        {
            ATopicReferenceCommon.ToStringFields(
                item: item,
                fg: fg,
                printMask: printMask);
            if (printMask?.Subtype ?? true)
            {
                fg.AppendItem(item.Subtype, "Subtype");
            }
        }
        
        public static TopicReferenceSubtype_FieldIndex ConvertFieldIndex(ATopicReference_FieldIndex index)
        {
            switch (index)
            {
                default:
                    throw new ArgumentException($"Index is out of range: {index.ToStringFast_Enum_Only()}");
            }
        }
        
        #region Equals and Hash
        public virtual bool Equals(
            ITopicReferenceSubtypeGetter? lhs,
            ITopicReferenceSubtypeGetter? rhs)
        {
            if (lhs == null && rhs == null) return false;
            if (lhs == null || rhs == null) return false;
            if (!base.Equals((IATopicReferenceGetter)lhs, (IATopicReferenceGetter)rhs)) return false;
            if (lhs.Subtype != rhs.Subtype) return false;
            return true;
        }
        
        public override bool Equals(
            IATopicReferenceGetter? lhs,
            IATopicReferenceGetter? rhs)
        {
            return Equals(
                lhs: (ITopicReferenceSubtypeGetter?)lhs,
                rhs: rhs as ITopicReferenceSubtypeGetter);
        }
        
        public virtual int GetHashCode(ITopicReferenceSubtypeGetter item)
        {
            var hash = new HashCode();
            hash.Add(item.Subtype);
            hash.Add(base.GetHashCode());
            return hash.ToHashCode();
        }
        
        public override int GetHashCode(IATopicReferenceGetter item)
        {
            return GetHashCode(item: (ITopicReferenceSubtypeGetter)item);
        }
        
        #endregion
        
        
        public override object GetNew()
        {
            return TopicReferenceSubtype.GetNew();
        }
        
        #region Mutagen
        public IEnumerable<FormKey> GetLinkFormKeys(ITopicReferenceSubtypeGetter obj)
        {
            foreach (var item in base.GetLinkFormKeys(obj))
            {
                yield return item;
            }
            yield break;
        }
        
        public void RemapLinks(ITopicReferenceSubtypeGetter obj, IReadOnlyDictionary<FormKey, FormKey> mapping) => throw new NotImplementedException();
        #endregion
        
    }
    public partial class TopicReferenceSubtypeSetterTranslationCommon : ATopicReferenceSetterTranslationCommon
    {
        public new static readonly TopicReferenceSubtypeSetterTranslationCommon Instance = new TopicReferenceSubtypeSetterTranslationCommon();

        #region DeepCopyIn
        public void DeepCopyIn(
            ITopicReferenceSubtype item,
            ITopicReferenceSubtypeGetter rhs,
            ErrorMaskBuilder? errorMask,
            TranslationCrystal? copyMask,
            bool deepCopy)
        {
            base.DeepCopyIn(
                (IATopicReference)item,
                (IATopicReferenceGetter)rhs,
                errorMask,
                copyMask,
                deepCopy: deepCopy);
            if ((copyMask?.GetShouldTranslate((int)TopicReferenceSubtype_FieldIndex.Subtype) ?? true))
            {
                item.Subtype = rhs.Subtype;
            }
        }
        
        
        public override void DeepCopyIn(
            IATopicReference item,
            IATopicReferenceGetter rhs,
            ErrorMaskBuilder? errorMask,
            TranslationCrystal? copyMask,
            bool deepCopy)
        {
            this.DeepCopyIn(
                item: (ITopicReferenceSubtype)item,
                rhs: (ITopicReferenceSubtypeGetter)rhs,
                errorMask: errorMask,
                copyMask: copyMask,
                deepCopy: deepCopy);
        }
        
        #endregion
        
        public TopicReferenceSubtype DeepCopy(
            ITopicReferenceSubtypeGetter item,
            TopicReferenceSubtype.TranslationMask? copyMask = null)
        {
            TopicReferenceSubtype ret = (TopicReferenceSubtype)((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).GetNew();
            ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)ret).CommonSetterTranslationInstance()!).DeepCopyIn(
                item: ret,
                rhs: item,
                errorMask: null,
                copyMask: copyMask?.GetCrystal(),
                deepCopy: true);
            return ret;
        }
        
        public TopicReferenceSubtype DeepCopy(
            ITopicReferenceSubtypeGetter item,
            out TopicReferenceSubtype.ErrorMask errorMask,
            TopicReferenceSubtype.TranslationMask? copyMask = null)
        {
            var errorMaskBuilder = new ErrorMaskBuilder();
            TopicReferenceSubtype ret = (TopicReferenceSubtype)((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).GetNew();
            ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)ret).CommonSetterTranslationInstance()!).DeepCopyIn(
                ret,
                item,
                errorMask: errorMaskBuilder,
                copyMask: copyMask?.GetCrystal(),
                deepCopy: true);
            errorMask = TopicReferenceSubtype.ErrorMask.Factory(errorMaskBuilder);
            return ret;
        }
        
        public TopicReferenceSubtype DeepCopy(
            ITopicReferenceSubtypeGetter item,
            ErrorMaskBuilder? errorMask,
            TranslationCrystal? copyMask = null)
        {
            TopicReferenceSubtype ret = (TopicReferenceSubtype)((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)item).CommonInstance()!).GetNew();
            ((TopicReferenceSubtypeSetterTranslationCommon)((ITopicReferenceSubtypeGetter)ret).CommonSetterTranslationInstance()!).DeepCopyIn(
                item: ret,
                rhs: item,
                errorMask: errorMask,
                copyMask: copyMask,
                deepCopy: true);
            return ret;
        }
        
    }
    #endregion

}

namespace Mutagen.Bethesda.Skyrim
{
    public partial class TopicReferenceSubtype
    {
        #region Common Routing
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILoquiRegistration ILoquiObject.Registration => TopicReferenceSubtype_Registration.Instance;
        public new static TopicReferenceSubtype_Registration Registration => TopicReferenceSubtype_Registration.Instance;
        [DebuggerStepThrough]
        protected override object CommonInstance() => TopicReferenceSubtypeCommon.Instance;
        [DebuggerStepThrough]
        protected override object CommonSetterInstance()
        {
            return TopicReferenceSubtypeSetterCommon.Instance;
        }
        [DebuggerStepThrough]
        protected override object CommonSetterTranslationInstance() => TopicReferenceSubtypeSetterTranslationCommon.Instance;

        #endregion

    }
}

#region Modules
#region Binary Translation
namespace Mutagen.Bethesda.Skyrim.Internals
{
    public partial class TopicReferenceSubtypeBinaryWriteTranslation :
        ATopicReferenceBinaryWriteTranslation,
        IBinaryWriteTranslator
    {
        public new readonly static TopicReferenceSubtypeBinaryWriteTranslation Instance = new TopicReferenceSubtypeBinaryWriteTranslation();

        public static void WriteEmbedded(
            ITopicReferenceSubtypeGetter item,
            MutagenWriter writer)
        {
            Mutagen.Bethesda.Binary.RecordTypeBinaryTranslation.Instance.Write(
                writer: writer,
                item: item.Subtype);
        }

        public void Write(
            MutagenWriter writer,
            ITopicReferenceSubtypeGetter item,
            RecordTypeConverter? recordTypeConverter = null)
        {
            WriteEmbedded(
                item: item,
                writer: writer);
        }

        public override void Write(
            MutagenWriter writer,
            object item,
            RecordTypeConverter? recordTypeConverter = null)
        {
            Write(
                item: (ITopicReferenceSubtypeGetter)item,
                writer: writer,
                recordTypeConverter: recordTypeConverter);
        }

        public override void Write(
            MutagenWriter writer,
            IATopicReferenceGetter item,
            RecordTypeConverter? recordTypeConverter = null)
        {
            Write(
                item: (ITopicReferenceSubtypeGetter)item,
                writer: writer,
                recordTypeConverter: recordTypeConverter);
        }

    }

    public partial class TopicReferenceSubtypeBinaryCreateTranslation : ATopicReferenceBinaryCreateTranslation
    {
        public new readonly static TopicReferenceSubtypeBinaryCreateTranslation Instance = new TopicReferenceSubtypeBinaryCreateTranslation();

        public static void FillBinaryStructs(
            ITopicReferenceSubtype item,
            MutagenFrame frame)
        {
            item.Subtype = Mutagen.Bethesda.Binary.RecordTypeBinaryTranslation.Instance.Parse(frame: frame);
        }

    }

}
namespace Mutagen.Bethesda.Skyrim
{
    #region Binary Write Mixins
    public static class TopicReferenceSubtypeBinaryTranslationMixIn
    {
    }
    #endregion


}
namespace Mutagen.Bethesda.Skyrim.Internals
{
    public partial class TopicReferenceSubtypeBinaryOverlay :
        ATopicReferenceBinaryOverlay,
        ITopicReferenceSubtypeGetter
    {
        #region Common Routing
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ILoquiRegistration ILoquiObject.Registration => TopicReferenceSubtype_Registration.Instance;
        public new static TopicReferenceSubtype_Registration Registration => TopicReferenceSubtype_Registration.Instance;
        [DebuggerStepThrough]
        protected override object CommonInstance() => TopicReferenceSubtypeCommon.Instance;
        [DebuggerStepThrough]
        protected override object CommonSetterTranslationInstance() => TopicReferenceSubtypeSetterTranslationCommon.Instance;

        #endregion

        void IPrintable.ToString(FileGeneration fg, string? name) => this.ToString(fg, name);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override object BinaryWriteTranslator => TopicReferenceSubtypeBinaryWriteTranslation.Instance;
        void IBinaryItem.WriteToBinary(
            MutagenWriter writer,
            RecordTypeConverter? recordTypeConverter = null)
        {
            ((TopicReferenceSubtypeBinaryWriteTranslation)this.BinaryWriteTranslator).Write(
                item: this,
                writer: writer,
                recordTypeConverter: recordTypeConverter);
        }

        public RecordType Subtype => new RecordType(BinaryPrimitives.ReadInt32LittleEndian(_data.Slice(0x0, 0x4)));
        partial void CustomFactoryEnd(
            OverlayStream stream,
            int finalPos,
            int offset);

        partial void CustomCtor();
        protected TopicReferenceSubtypeBinaryOverlay(
            ReadOnlyMemorySlice<byte> bytes,
            BinaryOverlayFactoryPackage package)
            : base(
                bytes: bytes,
                package: package)
        {
            this.CustomCtor();
        }

        public static TopicReferenceSubtypeBinaryOverlay TopicReferenceSubtypeFactory(
            OverlayStream stream,
            BinaryOverlayFactoryPackage package,
            RecordTypeConverter? recordTypeConverter = null)
        {
            var ret = new TopicReferenceSubtypeBinaryOverlay(
                bytes: stream.RemainingMemory.Slice(0, 0x4),
                package: package);
            int offset = stream.Position;
            stream.Position += 0x4;
            ret.CustomFactoryEnd(
                stream: stream,
                finalPos: stream.Length,
                offset: offset);
            return ret;
        }

        public static TopicReferenceSubtypeBinaryOverlay TopicReferenceSubtypeFactory(
            ReadOnlyMemorySlice<byte> slice,
            BinaryOverlayFactoryPackage package,
            RecordTypeConverter? recordTypeConverter = null)
        {
            return TopicReferenceSubtypeFactory(
                stream: new OverlayStream(slice, package),
                package: package,
                recordTypeConverter: recordTypeConverter);
        }

        #region To String

        public override void ToString(
            FileGeneration fg,
            string? name = null)
        {
            TopicReferenceSubtypeMixIn.ToString(
                item: this,
                name: name);
        }

        #endregion

        #region Equals and Hash
        public override bool Equals(object? obj)
        {
            if (!(obj is ITopicReferenceSubtypeGetter rhs)) return false;
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).Equals(this, rhs);
        }

        public bool Equals(ITopicReferenceSubtypeGetter? obj)
        {
            return ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).Equals(this, obj);
        }

        public override int GetHashCode() => ((TopicReferenceSubtypeCommon)((ITopicReferenceSubtypeGetter)this).CommonInstance()!).GetHashCode(this);

        #endregion

    }

}
#endregion

#endregion

