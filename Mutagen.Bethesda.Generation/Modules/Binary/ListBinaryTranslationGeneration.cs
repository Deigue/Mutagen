﻿using Loqui;
using Loqui.Generation;
using Noggog;
using Mutagen.Bethesda.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mutagen.Bethesda.Generation
{
    public enum ListBinaryType
    {
        SubTrigger,
        Trigger,
        CounterRecord,
        PrependCount,
        Frame
    }

    public class ListBinaryTranslationGeneration : BinaryTranslationGeneration
    {
        public virtual string TranslatorName => $"ListBinaryTranslation";

        const string AsyncItemKey = "ListAsyncItem";
        const string ThreadKey = "ListThread";
        public const string CounterRecordType = "ListCounterRecordType";
        public const string PrependCountType = "ListPrependCountType";

        public override string GetTranslatorInstance(TypeGeneration typeGen, bool getter)
        {
            var list = typeGen as ListType;
            if (!Module.TryGetTypeGeneration(list.SubTypeGeneration.GetType(), out var subTransl))
            {
                throw new ArgumentException("Unsupported type generator: " + list.SubTypeGeneration);
            }

            var subMaskStr = subTransl.MaskModule.GetMaskModule(list.SubTypeGeneration.GetType()).GetErrorMaskTypeStr(list.SubTypeGeneration);
            return $"{TranslatorName}<{list.SubTypeGeneration.TypeName(getter)}, {subMaskStr}>.Instance";
        }

        public override bool IsAsync(TypeGeneration gen, bool read)
        {
            var listType = gen as ListType;
            if (listType.CustomData.TryGetValue(ThreadKey, out var val) && ((bool)val)) return true;
            if (this.Module.TryGetTypeGeneration(listType.SubTypeGeneration.GetType(), out var keyGen)
                && keyGen.IsAsync(listType.SubTypeGeneration, read)) return true;
            return false;
        }

        public override void Load(ObjectGeneration obj, TypeGeneration field, XElement node)
        {
            var listType = field as ListType;
            listType.CustomData[ThreadKey] = node.GetAttribute<bool>("thread", false);
            listType.CustomData[CounterRecordType] = node.GetAttribute("counterRecType", null);
            listType.CustomData[PrependCountType] = node.GetAttribute("prependCount", false);
            var asyncItem = node.GetAttribute<bool>("asyncItems", false);
            if (asyncItem && listType.SubTypeGeneration is LoquiType loqui)
            {
                loqui.CustomData[LoquiBinaryTranslationGeneration.AsyncOverrideKey] = asyncItem;
            }
        }

        private ListBinaryType GetListType(
            ListType list,
            MutagenFieldData data,
            MutagenFieldData subData)
        {
            if (list.CustomData.TryGetValue(CounterRecordType, out var counterRecTypeObj)
                && counterRecTypeObj is string counterRecType
                && !string.IsNullOrWhiteSpace(counterRecType))
            {
                return ListBinaryType.CounterRecord;
            }
            if (list.CustomData.TryGetValue(PrependCountType, out var prependCountRecType)
                && prependCountRecType is bool prependCount
                && prependCount)
            {
                return ListBinaryType.PrependCount;
            }
            if (subData.HasTrigger)
            {
                return ListBinaryType.SubTrigger;
            }
            if (data.HasTrigger)
            {
                return ListBinaryType.Trigger;
            }
            return ListBinaryType.Frame;
        }

        protected virtual string GetWriteAccessor(Accessor itemAccessor)
        {
            return itemAccessor.PropertyOrDirectAccess;
        }

        public override void GenerateWrite(
            FileGeneration fg,
            ObjectGeneration objGen,
            TypeGeneration typeGen,
            Accessor writerAccessor,
            Accessor itemAccessor,
            Accessor errorMaskAccessor,
            Accessor translationMaskAccessor)
        {
            var list = typeGen as ListType;
            if (!this.Module.TryGetTypeGeneration(list.SubTypeGeneration.GetType(), out var subTransl))
            {
                throw new ArgumentException("Unsupported type generator: " + list.SubTypeGeneration);
            }

            if (typeGen.TryGetFieldData(out var data)
                && data.MarkerType.HasValue)
            {
                fg.AppendLine($"using (HeaderExport.ExportHeader(writer, {objGen.RegistrationName}.{data.MarkerType.Value.Type}_HEADER, ObjectType.Subrecord)) {{ }}");
            }

            var subData = list.SubTypeGeneration.GetFieldData();

            ListBinaryType listBinaryType = GetListType(list, data, subData);

            var allowDirectWrite = subTransl.AllowDirectWrite(objGen, typeGen);
            var isLoqui = list.SubTypeGeneration is LoquiType;
            var listOfRecords = !isLoqui
                && listBinaryType == ListBinaryType.SubTrigger
                && allowDirectWrite;

            var typeName = list.SubTypeGeneration.TypeName(getter: true);
            if (list.SubTypeGeneration is LoquiType loqui)
            {
                typeName = loqui.TypeName(getter: true, internalInterface: true);
            }

            using (var args = new ArgsWrapper(fg,
                $"{this.Namespace}ListBinaryTranslation<{typeName}>.Instance.Write"))
            {
                args.Add($"writer: {writerAccessor}");
                args.Add($"items: {GetWriteAccessor(itemAccessor)}");
                if (listBinaryType == ListBinaryType.Trigger)
                {
                    args.Add($"recordType: {data.TriggeringRecordSetAccessor}");
                }
                if (listOfRecords)
                {
                    args.Add($"recordType: {subData.TriggeringRecordSetAccessor}");
                }
                if (this.Module.TranslationMaskParameter)
                {
                    args.Add($"translationMask: {translationMaskAccessor}");
                }
                if (allowDirectWrite)
                {
                    args.Add($"transl: {subTransl.GetTranslatorInstance(list.SubTypeGeneration, getter: true)}.Write");
                }
                else
                {
                    args.Add((gen) =>
                    {
                        var listTranslMask = this.MaskModule.GetMaskModule(list.SubTypeGeneration.GetType()).GetTranslationMaskTypeStr(list.SubTypeGeneration);
                        gen.AppendLine($"transl: (MutagenWriter subWriter, {typeName} subItem) =>");
                        using (new BraceWrapper(gen))
                        {
                            subTransl.GenerateWrite(
                                fg: gen,
                                objGen: objGen,
                                typeGen: list.SubTypeGeneration,
                                writerAccessor: "subWriter",
                                translationAccessor: "listTranslMask",
                                itemAccessor: new Accessor($"subItem"),
                                errorMaskAccessor: null);
                        }
                    });
                }
            }
        }

        public override void GenerateCopyIn(
            FileGeneration fg,
            ObjectGeneration objGen,
            TypeGeneration typeGen,
            Accessor nodeAccessor,
            Accessor itemAccessor,
            Accessor errorMaskAccessor,
            Accessor translationMaskAccessor)
        {
            var list = typeGen as ListType;
            var data = list.GetFieldData();
            var subData = list.SubTypeGeneration.GetFieldData();
            if (!this.Module.TryGetTypeGeneration(list.SubTypeGeneration.GetType(), out var subTransl))
            {
                throw new ArgumentException("Unsupported type generator: " + list.SubTypeGeneration);
            }

            var isAsync = subTransl.IsAsync(list.SubTypeGeneration, read: true);
            ListBinaryType listBinaryType = GetListType(list, data, subData);

            if (typeGen.CustomData.TryGetValue(CounterRecordType, out var counterRecObj)
                && counterRecObj is string counterRecType)
            {
                fg.AppendLine("var amount = BinaryPrimitives.ReadInt32LittleEndian(frame.MetaData.ReadSubRecordFrame(frame).Content);");
            }
            else if (data.MarkerType.HasValue)
            {
                fg.AppendLine($"frame.Position += frame.{nameof(MutagenFrame.MetaData)}.{nameof(GameConstants.SubConstants)}.{nameof(GameConstants.SubConstants.HeaderLength)} + contentLength; // Skip marker");
            }
            else if (listBinaryType == ListBinaryType.Trigger)
            {
                fg.AppendLine($"frame.Position += frame.{nameof(MutagenBinaryReadStream.MetaData)}.{nameof(GameConstants.SubConstants)}.{nameof(RecordHeaderConstants.HeaderLength)};");
            }

            bool threading = list.CustomData.TryGetValue(ThreadKey, out var t)
                && (bool)t;

            using (var args = new ArgsWrapper(fg,
                $"{Loqui.Generation.Utility.Await(isAsync)}{this.Namespace}List{(isAsync ? "Async" : null)}BinaryTranslation<{list.SubTypeGeneration.TypeName(getter: false)}>.Instance.ParseRepeatedItem",
                suffixLine: Loqui.Generation.Utility.ConfigAwait(isAsync)))
            {
                if (list is ArrayType arr
                    && arr.FixedSize.HasValue)
                {
                    args.AddPassArg($"frame");
                    args.Add($"amount: {arr.FixedSize.Value}");
                }
                else
                {
                    switch (listBinaryType)
                    {
                        case ListBinaryType.SubTrigger:
                            args.AddPassArg($"frame");
                            args.Add($"triggeringRecord: {subData.TriggeringRecordSetAccessor}");
                            break;
                        case ListBinaryType.Trigger:
                            args.Add($"frame: frame.SpawnWithLength(contentLength)");
                            break;
                        case ListBinaryType.CounterRecord:
                            args.AddPassArg($"frame");
                            args.AddPassArg($"amount");
                            if (subData.HasTrigger)
                            {
                                args.Add($"triggeringRecord: {subData.TriggeringRecordSetAccessor}");
                            }
                            break;
                        case ListBinaryType.PrependCount:
                            args.Add("amount: frame.ReadInt32()");
                            args.AddPassArg($"frame");
                            break;
                        case ListBinaryType.Frame:
                            args.AddPassArg($"frame");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                if (threading)
                {
                    args.Add($"thread: true");
                }
                if (list.SubTypeGeneration is FormLinkType)
                {
                    args.Add($"masterReferences: masterReferences");
                }
                args.Add($"item: {itemAccessor.PropertyOrDirectAccess}");
                if (list.CustomData.TryGetValue("lengthLength", out object len))
                {
                    args.Add($"lengthLength: {len}");
                }
                else if (listBinaryType != ListBinaryType.CounterRecord 
                    && list.SubTypeGeneration.GetFieldData().HasTrigger)
                {
                    if (list.SubTypeGeneration is MutagenLoquiType loqui)
                    {
                        switch (loqui.GetObjectType())
                        {
                            case ObjectType.Subrecord:
                                args.Add($"lengthLength: frame.{nameof(MutagenFrame.MetaData)}.{nameof(GameConstants.SubConstants)}.{nameof(GameConstants.SubConstants.LengthLength)}");
                                break;
                            case ObjectType.Group:
                                args.Add($"lengthLength: frame.{nameof(MutagenFrame.MetaData)}.{nameof(GameConstants.GroupConstants)}.{nameof(GameConstants.SubConstants.LengthLength)}");
                                break;
                            case ObjectType.Record:
                                args.Add($"lengthLength: frame.{nameof(MutagenFrame.MetaData)}.{nameof(GameConstants.MajorConstants)}.{nameof(GameConstants.SubConstants.LengthLength)}");
                                break;
                            case ObjectType.Mod:
                            default:
                                throw new ArgumentException();
                        }
                    }
                    else
                    {
                        args.Add($"lengthLength: frame.{nameof(MutagenFrame.MetaData)}.{nameof(GameConstants.SubConstants)}.{nameof(GameConstants.SubConstants.LengthLength)}");
                    }
                }
                var subGenTypes = subData.GenerationTypes.ToList();
                var subGen = this.Module.GetTypeGeneration(list.SubTypeGeneration.GetType());
                if (subGenTypes.Count <= 1 && subTransl.AllowDirectParse(
                    objGen,
                    typeGen: typeGen,
                    squashedRepeatedList: listBinaryType == ListBinaryType.Trigger))
                {
                    if (list.SubTypeGeneration is LoquiType loqui
                        && !loqui.CanStronglyType)
                    {
                        args.Add($"transl: {subTransl.GetTranslatorInstance(list.SubTypeGeneration, getter: false)}.Parse<{loqui.TypeName(getter: false)}>");
                    }
                    else
                    {
                        args.Add($"transl: {subTransl.GetTranslatorInstance(list.SubTypeGeneration, getter: false)}.Parse");
                    }
                }
                else
                {
                    args.Add((gen) =>
                    {
                        gen.AppendLine($"transl: {Loqui.Generation.Utility.Async(isAsync)}(MutagenFrame r{(subGenTypes.Count <= 1 ? string.Empty : ", RecordType header")}{(isAsync ? null : $", out {list.SubTypeGeneration.TypeName(getter: false)} listSubItem")}) =>");
                        using (new BraceWrapper(gen))
                        {
                            if (subGenTypes.Count <= 1)
                            {
                                subGen.GenerateCopyInRet(
                                    fg: gen,
                                    objGen: objGen,
                                    targetGen: list.SubTypeGeneration,
                                    typeGen: list.SubTypeGeneration,
                                    readerAccessor: "r",
                                    translationAccessor: "listTranslMask",
                                    retAccessor: "return ",
                                    outItemAccessor: new Accessor("listSubItem"),
                                    asyncMode: isAsync ? AsyncMode.Async : AsyncMode.Off,
                                    errorMaskAccessor: "listErrMask");
                            }
                            else
                            {
                                gen.AppendLine("switch (header.TypeInt)");
                                using (new BraceWrapper(gen))
                                {
                                    foreach (var item in subGenTypes)
                                    {
                                        foreach (var trigger in item.Key)
                                        {
                                            gen.AppendLine($"case 0x{trigger.TypeInt.ToString("X")}: // {trigger.Type}");
                                        }
                                        LoquiType targetLoqui = list.SubTypeGeneration as LoquiType;
                                        LoquiType specificLoqui = item.Value as LoquiType;
                                        using (new DepthWrapper(gen))
                                        {
                                            subGen.GenerateCopyInRet(
                                                fg: gen,
                                                objGen: objGen,
                                                targetGen: list.SubTypeGeneration,
                                                typeGen: item.Value,
                                                readerAccessor: "r",
                                                translationAccessor: "listTranslMask",
                                                retAccessor: "return ",
                                                outItemAccessor: new Accessor("listSubItem"),
                                                asyncMode: AsyncMode.Async,
                                                errorMaskAccessor: $"listErrMask");
                                        }
                                    }
                                    gen.AppendLine("default:");
                                    using (new DepthWrapper(gen))
                                    {
                                        gen.AppendLine("throw new NotImplementedException();");
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }

        public override void GenerateCopyInRet(
            FileGeneration fg,
            ObjectGeneration objGen,
            TypeGeneration targetGen,
            TypeGeneration typeGen,
            Accessor nodeAccessor,
            AsyncMode asyncMode,
            Accessor retAccessor,
            Accessor outItemAccessor,
            Accessor errorMaskAccessor,
            Accessor translationAccessor)
        {
            throw new NotImplementedException();
        }

        public override void GenerateWrapperFields(
            FileGeneration fg,
            ObjectGeneration objGen,
            TypeGeneration typeGen,
            Accessor dataAccessor,
            int? currentPosition,
            DataType dataType = null)
        {
            ListType list = typeGen as ListType;
            var data = list.GetFieldData();
            switch (data.BinaryOverlayFallback)
            {
                case BinaryGenerationType.Normal:
                    break;
                case BinaryGenerationType.DoNothing:
                case BinaryGenerationType.NoGeneration:
                    return;
                case BinaryGenerationType.Custom:
                    using (var args = new ArgsWrapper(fg,
                        $"partial void {typeGen.Name}CustomParse"))
                    {
                        args.Add($"{nameof(BinaryMemoryReadStream)} stream");
                        args.Add($"long finalPos");
                        args.Add($"int offset");
                        args.Add($"{nameof(RecordType)} type");
                        args.Add($"int? lastParsed");
                    }
                    return;
                default:
                    throw new NotImplementedException();
            }
            var subGen = this.Module.GetTypeGeneration(list.SubTypeGeneration.GetType());
            if (list.SubTypeGeneration is LoquiType loqui)
            {
                var typeName = this.Module.BinaryOverlayClassName(loqui);
                fg.AppendLine($"public {list.Interface(getter: true, internalInterface: true)} {typeGen.Name} {{ get; private set; }} = EmptySetList<{typeName}>.Instance;");
            }
            else if (data.HasTrigger)
            {
                fg.AppendLine($"public {list.Interface(getter: true, internalInterface: true)} {typeGen.Name} {{ get; private set; }} = EmptySetList<{list.SubTypeGeneration.TypeName(getter: true)}>.Instance;");
            }
            else
            {
                fg.AppendLine($"public {list.Interface(getter: true, internalInterface: true)} {typeGen.Name} => BinaryOverlaySetList<{list.SubTypeGeneration.TypeName(getter: true)}>.FactoryByStartIndex({dataAccessor}.Slice({currentPosition}), _package, {subGen.ExpectedLength(objGen, list.SubTypeGeneration)}, (s, p) => {subGen.GenerateForTypicalWrapper(objGen, list.SubTypeGeneration, "s", "p")});");
            }
        }

        public override int? ExpectedLength(ObjectGeneration objGen, TypeGeneration typeGen)
        {
            return null;
        }

        public override async Task GenerateWrapperRecordTypeParse(
            FileGeneration fg,
            ObjectGeneration objGen,
            TypeGeneration typeGen,
            Accessor locationAccessor,
            Accessor packageAccessor,
            Accessor converterAccessor)
        {
            ListType list = typeGen as ListType;
            var data = list.GetFieldData();
            switch (data.BinaryOverlayFallback)
            {
                case BinaryGenerationType.Normal:
                    break;
                case BinaryGenerationType.DoNothing:
                case BinaryGenerationType.NoGeneration:
                    return;
                case BinaryGenerationType.Custom:
                    using (var args = new ArgsWrapper(fg,
                        $"{typeGen.Name}CustomParse"))
                    {
                        args.AddPassArg($"stream");
                        args.AddPassArg($"finalPos");
                        args.AddPassArg($"offset");
                        args.AddPassArg($"type");
                        args.AddPassArg($"lastParsed");
                    }
                    return;
                default:
                    throw new NotImplementedException();
            }

            if (data.MarkerType.HasValue)
            {
                fg.AppendLine($"stream.Position += {packageAccessor}.Meta.SubConstants.HeaderLength; // Skip marker");
            }
            var subData = list.SubTypeGeneration.GetFieldData();
            var subGenTypes = subData.GenerationTypes.ToList();
            ListBinaryType listBinaryType = GetListType(list, data, subData);
            var subGen = this.Module.GetTypeGeneration(list.SubTypeGeneration.GetType());
            string typeName;
            LoquiType loqui = list.SubTypeGeneration as LoquiType;
            if (loqui != null)
            {
                typeName = this.Module.BinaryOverlayClassName(loqui);
            }
            else
            {
                typeName = list.SubTypeGeneration.TypeName(getter: true);
            }
            switch (listBinaryType)
            {
                case ListBinaryType.SubTrigger:
                    if (loqui != null)
                    {
                        if (loqui.TargetObjectGeneration.IsTypelessStruct())
                        {
                            using (var args = new ArgsWrapper(fg,
                                $"this.{typeGen.Name} = this.{nameof(BinaryOverlay.ParseRepeatedTypelessSubrecord)}<{typeName}>"))
                            {
                                args.AddPassArg("stream");
                                args.Add($"recordTypeConverter: {converterAccessor}");
                                args.Add($"trigger: {subData.TriggeringRecordSetAccessor}");
                                if (subGenTypes.Count <= 1)
                                {
                                    args.Add($"factory:  {this.Module.BinaryOverlayClassName(loqui)}.{loqui.TargetObjectGeneration.Name}Factory");
                                }
                                else
                                {
                                    args.Add((subFg) =>
                                    {
                                        subFg.AppendLine("factory: (s, r, p, recConv) =>");
                                        using (new BraceWrapper(subFg))
                                        {
                                            subFg.AppendLine("switch (r.TypeInt)");
                                            using (new BraceWrapper(subFg))
                                            {
                                                foreach (var item in subGenTypes)
                                                {
                                                    foreach (var trigger in item.Key)
                                                    {
                                                        subFg.AppendLine($"case 0x{trigger.TypeInt.ToString("X")}: // {trigger.Type}");
                                                    }
                                                    using (new DepthWrapper(subFg))
                                                    {
                                                        LoquiType specificLoqui = item.Value as LoquiType;
                                                        subFg.AppendLine($"return {this.Module.BinaryOverlayClassName(specificLoqui.TargetObjectGeneration)}.{specificLoqui.TargetObjectGeneration.Name}Factory(s, p);");
                                                    }
                                                }
                                                subFg.AppendLine("default:");
                                                using (new DepthWrapper(subFg))
                                                {
                                                    subFg.AppendLine("throw new NotImplementedException();");
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        else
                        {
                            using (var args = new ArgsWrapper(fg,
                                $"this.{typeGen.Name} = BinaryOverlaySetList<{typeName}>.FactoryByArray"))
                            {
                                args.Add($"mem: stream.RemainingMemory");
                                args.Add($"package: _package");
                                args.Add($"recordTypeConverter: {converterAccessor}");
                                args.Add($"getter: (s, p, recConv) => {typeName}.{loqui.TargetObjectGeneration.Name}Factory(new {nameof(BinaryMemoryReadStream)}(s), p, recConv)");
                                args.Add(subFg =>
                                {
                                    using (var subArgs = new FunctionWrapper(subFg,
                                        $"locs: {nameof(BinaryOverlay.ParseRecordLocations)}"))
                                    {
                                        subArgs.AddPassArg("stream");
                                        subArgs.AddPassArg("finalPos");
                                        subArgs.Add("trigger: type");
                                        switch (loqui.TargetObjectGeneration.GetObjectType())
                                        {
                                            case ObjectType.Subrecord:
                                                subArgs.Add($"constants: _package.Meta.{nameof(GameConstants.SubConstants)}");
                                                break;
                                            case ObjectType.Record:
                                                subArgs.Add($"constants: _package.Meta.{nameof(GameConstants.MajorConstants)}");
                                                break;
                                            case ObjectType.Group:
                                                subArgs.Add($"constants: _package.Meta.{nameof(GameConstants.GroupConstants)}");
                                                break;
                                            case ObjectType.Mod:
                                            default:
                                                throw new NotImplementedException();
                                        }
                                        subArgs.Add("skipHeader: false");
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        using (var args = new ArgsWrapper(fg,
                            $"this.{typeGen.Name} = BinaryOverlaySetList<{typeName}>.FactoryByArray"))
                        {
                            args.Add($"mem: stream.RemainingMemory");
                            args.Add($"package: _package");
                            args.Add($"getter: (s, p) => {subGen.GenerateForTypicalWrapper(objGen, list.SubTypeGeneration, "s", "p")}");
                            args.Add(subFg =>
                            {
                                using (var subArgs = new FunctionWrapper(subFg,
                                    $"locs: {nameof(BinaryOverlay.ParseRecordLocations)}"))
                                {
                                    subArgs.AddPassArg("stream");
                                    subArgs.AddPassArg("finalPos");
                                    subArgs.Add($"constants: _package.Meta.{nameof(GameConstants.SubConstants)}");
                                    subArgs.Add("trigger: type");
                                    subArgs.Add("skipHeader: true");
                                }
                            });
                        }
                    }
                    break;
                case ListBinaryType.Trigger:
                    fg.AppendLine("var subMeta = _package.Meta.ReadSubRecord(stream);");
                    fg.AppendLine("var subLen = subMeta.RecordLength;");
                    var expectedLen = subGen.ExpectedLength(objGen, list.SubTypeGeneration);
                    if (expectedLen.HasValue)
                    {
                        using (var args = new ArgsWrapper(fg,
                            $"this.{typeGen.Name} = BinaryOverlaySetList<{typeName}>.FactoryByStartIndex"))
                        {
                            args.Add($"mem: stream.RemainingMemory.Slice(0, subLen)");
                            args.Add($"package: _package");
                            args.Add($"itemLength: {subGen.ExpectedLength(objGen, list.SubTypeGeneration)}");
                            if (subGenTypes.Count <= 1)
                            {
                                args.Add($"getter: (s, p) => {subGen.GenerateForTypicalWrapper(objGen, list.SubTypeGeneration, "s", "p")}");
                            }
                            else
                            {
                                args.Add((subFg) =>
                                {
                                    subFg.AppendLine("getter: (s, r, p) =>");
                                    using (new BraceWrapper(subFg))
                                    {
                                        subFg.AppendLine("switch (r.TypeInt)");
                                        using (new BraceWrapper(subFg))
                                        {
                                            foreach (var item in subGenTypes)
                                            {
                                                foreach (var trigger in item.Key)
                                                {
                                                    subFg.AppendLine($"case 0x{trigger.TypeInt.ToString("X")}: // {trigger.Type}");
                                                }
                                                using (new DepthWrapper(subFg))
                                                {
                                                    LoquiType specificLoqui = item.Value as LoquiType;
                                                    subFg.AppendLine($"return {subGen.GenerateForTypicalWrapper(objGen, specificLoqui, "s", "p")}");
                                                }
                                            }
                                            subFg.AppendLine("default:");
                                            using (new DepthWrapper(subFg))
                                            {
                                                subFg.AppendLine("throw new NotImplementedException();");
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        using (var args = new ArgsWrapper(fg,
                            $"this.{typeGen.Name} = BinaryOverlaySetList<{typeName}>.FactoryByLazyParse"))
                        {
                            args.Add($"mem: stream.RemainingMemory.Slice(0, subLen)");
                            args.Add($"package: _package");
                            if (subGenTypes.Count <= 1)
                            {
                                args.Add($"getter: (s, p) => {subGen.GenerateForTypicalWrapper(objGen, list.SubTypeGeneration, "s", "p")}");
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                    fg.AppendLine("stream.Position += subLen;");
                    break;
                case ListBinaryType.CounterRecord:
                    fg.AppendLine("var subMeta = _package.Meta.ReadSubRecord(stream);");
                    fg.AppendLine("var subLen = subMeta.RecordLength;");
                    if (!subData.HasTrigger)
                    {
                        throw new NotImplementedException();
                    }
                    using (var args = new ArgsWrapper(fg,
                        $"this.{typeGen.Name} = BinaryOverlaySetList<{typeName}>.FactoryByStartIndex"))
                    {
                        args.Add($"mem: stream.RemainingMemory.Slice(0, subLen)");
                        args.Add($"package: _package");
                        args.Add($"itemLength: {subGen.ExpectedLength(objGen, list.SubTypeGeneration)}");
                        if (subGenTypes.Count <= 1)
                        {
                            args.Add($"getter: (s, p) => {subGen.GenerateForTypicalWrapper(objGen, list.SubTypeGeneration, "s", "p")}");
                        }
                        else
                        {
                            args.Add((subFg) =>
                            {
                                subFg.AppendLine("getter: (s, r, p) =>");
                                using (new BraceWrapper(subFg))
                                {
                                    subFg.AppendLine("switch (r.TypeInt)");
                                    using (new BraceWrapper(subFg))
                                    {
                                        foreach (var item in subGenTypes)
                                        {
                                            foreach (var trigger in item.Key)
                                            {
                                                subFg.AppendLine($"case 0x{trigger.TypeInt.ToString("X")}: // {trigger.Type}");
                                            }
                                            using (new DepthWrapper(subFg))
                                            {
                                                LoquiType specificLoqui = item.Value as LoquiType;
                                                subFg.AppendLine($"return {subGen.GenerateForTypicalWrapper(objGen, specificLoqui, "s", "p")};");
                                            }
                                        }
                                        subFg.AppendLine("default:");
                                        using (new DepthWrapper(subFg))
                                        {
                                            subFg.AppendLine("throw new NotImplementedException();");
                                        }
                                    }
                                }
                            });
                        }
                    }
                    fg.AppendLine("stream.Position += subLen;");
                    break;
                case ListBinaryType.PrependCount:
                    fg.AppendLine("throw new NotImplementedException();");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
