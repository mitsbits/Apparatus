using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Borg
{
    public unsafe static class Transmute
    {
        public static unsafe void* GetObjectAddress(this object obj) => *(void**)Unsafe.AsPointer(ref obj);

        public static void TransmuteTo(this object target, object source)
        {
            if (target.GetType() == source.GetType()) return;
            var s = (void**)source.GetObjectAddress();
            var t = (void**)target.GetObjectAddress();
            *t = *s;
            if (target.GetType() != source.GetType())
                throw new AccessViolationException($"Illegal write to address {new IntPtr(t)}");
        }

        public static T TransmuteTo<T>(this object target, T source)
        {
            target.TransmuteTo((object)source);
            return (T)target;
        }

        public static T TransmuteTo<T>(this object target) where T : new() => target.TransmuteTo(new T());
    }

    public static class Weird
    {
        public static List<TResult> Add<TSource, TItem, TResult>(this List<TSource> list, TItem item)
            where TSource : class, TResult
            where TItem : class, TResult
        {
            var e = list.TransmuteTo<List<TResult>>();
            typeof(List<TResult>)
                .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(e)
                .TransmuteTo(new TResult[0]);
            e.Add(item);
            return e;
        }
    }
}