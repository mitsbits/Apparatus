using Borg.Infrastructure.Core.Strings.Services;
using System;

namespace Borg
{
    public static class IJsonConverterExtensions
    {
        public static bool TryDeSerialize<T>(this IJsonConverter serializer, string source, out T instanse)
        {
            instanse = default(T);
            try
            {
                instanse = serializer.DeSerialize<T>(source);
                if (instanse.Equals(default(T))) return false;
                return true;
            }
            catch (Exception) //TODO: hmmmmmmm
            {
                return false;
            }
        }
    }
}