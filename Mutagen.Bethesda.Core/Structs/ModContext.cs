using System;
using System.Diagnostics.CodeAnalysis;

namespace Mutagen.Bethesda
{
    public interface IModContext
    {
        ModKey ModKey { get; }
        IModContext? Parent { get; }
        object? Record { get; }
    }

    public interface IModContext<T> : IModContext
    {
        new T Record { get; }
    }

    public interface IModContext<TModSetter, TMajorSetter, TMajorGetter> : IModContext<TMajorGetter>
        where TModSetter : IModGetter
        where TMajorSetter : IMajorRecordCommon, TMajorGetter
        where TMajorGetter : IMajorRecordCommonGetter
    {
        TMajorSetter GetOrAddAsOverride(TModSetter mod);
        bool TryGetParentContext<TTargetMajorSetter, TTargetMajorGetter>([MaybeNullWhen(false)] out IModContext<TModSetter, TTargetMajorSetter, TTargetMajorGetter> parent)
            where TTargetMajorSetter : IMajorRecordCommon, TTargetMajorGetter
            where TTargetMajorGetter : IMajorRecordCommonGetter;
    }

    public class ModContext<T> : IModContext<T>
    {
        public ModKey ModKey { get; set; }

        public IModContext? Parent { get; set; }

        public T Record { get; set; }
        object? IModContext.Record => Record;

        public ModContext(ModKey modKey, IModContext? parent, T record)
        {
            ModKey = modKey;
            Parent = parent;
            Record = record;
        }
    }

    /// <summary>
    /// A pairing of a record as well as the logic and knowledge of where it came from in its parent mod.
    /// This allows a context to insert the record into a new mod, using the knowledge to properly insert and find the appropriate
    /// location within the new mod. <br />
    /// <br />
    /// This is typically only useful for deeper nested records such as Cell/PlacedObjects/Navmeshes/etc
    /// </summary>
    /// <typeparam name="TModSetter">The setter interface of the mod type to target</typeparam>
    /// <typeparam name="TMajorSetter">The setter interface of the contained record</typeparam>
    /// <typeparam name="TMajorGetter">The getter interface of the contained record</typeparam>
    public class ModContext<TModSetter, TMajorSetter, TMajorGetter> : IModContext<TModSetter, TMajorSetter, TMajorGetter>
        where TModSetter : IModGetter
        where TMajorSetter : IMajorRecordCommon, TMajorGetter
        where TMajorGetter : IMajorRecordCommonGetter
    {
        private readonly Func<TModSetter, TMajorGetter, TMajorSetter> _getOrAddAsOverride;

        /// <summary>
        /// The contained record
        /// </summary>
        public TMajorGetter Record { get; }
        object IModContext.Record => Record;

        /// <summary>
        /// The source ModKey the record originated from
        /// </summary>
        public ModKey ModKey { get; }

        /// <summary>
        /// Parent context, if any
        /// </summary>
        public IModContext? Parent { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modKey">ModKey the record is originating from</param>
        /// <param name="record">The record to wrap</param>
        /// <param name="getter">Logic for how to navigate a mod and insert a copy of the wrapped record</param>
        /// <param name="parent">Optional parent context</param>
        public ModContext(
            ModKey modKey,
            TMajorGetter record,
            Func<TModSetter, TMajorGetter, TMajorSetter> getter,
            IModContext? parent = null)
        {
            ModKey = modKey;
            Record = record;
            _getOrAddAsOverride = getter;
            Parent = parent;
        }

        public static implicit operator TMajorGetter(ModContext<TModSetter, TMajorSetter, TMajorGetter> context)
        {
            return context.Record;
        }

        /// <summary>
        /// Searches a mod for an existing override of the record wrapped by this context. <br/>
        /// If one is found, it is returned. <br/>
        /// Otherwise, this contexts knowledge is used to insert a copy into the target mod, which is then returned.
        /// </summary>
        /// <param name="mod">Mod to search/insert into</param>
        /// <returns>An override of the wrapped record, which is sourced from the target mod</returns>
        public TMajorSetter GetOrAddAsOverride(TModSetter mod)
        {
            try
            {
                return _getOrAddAsOverride(mod, Record);
            }
            catch (Exception ex)
            {
                throw RecordException.Factory(ex, ModKey, Record);
            }
        }

        public bool TryGetParentContext<TTargetMajorSetter, TTargetMajorGetter>([MaybeNullWhen(false)] out IModContext<TModSetter, TTargetMajorSetter, TTargetMajorGetter> parent)
            where TTargetMajorSetter : IMajorRecordCommon, TTargetMajorGetter
            where TTargetMajorGetter : IMajorRecordCommonGetter
        {
            var targetContext = this.Parent;
            while (targetContext != null)
            {
                if (targetContext.Record is TTargetMajorGetter)
                {
                    parent = (ModContext<TModSetter, TTargetMajorSetter, TTargetMajorGetter>)targetContext;
                    return true;
                }
                targetContext = targetContext.Parent;
            }
            parent = default;
            return false;
        }
    }

    public static class ModContextExt
    {
        public static bool IsUnderneath<T>(this IModContext context)
        {
            return TryGetParent<T>(context, out _);
        }

        public static bool TryGetParent<T>(this IModContext context, [MaybeNullWhen(false)] out T item)
        {
            var targetContext = context.Parent;
            while (targetContext != null)
            {
                if (targetContext.Record is T t)
                {
                    item = t;
                    return true;
                }
                targetContext = targetContext.Parent;
            }
            item = default;
            return false;
        }

        public static ModContext<TModSetter, RMajorSetter, RMajorGetter> AsType<TModSetter, TMajorSetter, TMajorGetter, RMajorSetter, RMajorGetter>(this IModContext<TModSetter, TMajorSetter, TMajorGetter> context)
            where TModSetter : IModGetter
            where TMajorSetter : IMajorRecordCommon, TMajorGetter
            where TMajorGetter : IMajorRecordCommonGetter
            where RMajorSetter : TMajorSetter, RMajorGetter
            where RMajorGetter : TMajorGetter
        {
            return new ModContext<TModSetter, RMajorSetter, RMajorGetter>(
                context.ModKey,
                (RMajorGetter)context.Record,
                (mod, rec) => (RMajorSetter)context.GetOrAddAsOverride(mod),
                context.Parent);
        }
    }
}
