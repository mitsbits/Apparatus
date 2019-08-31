using Borg.Infrastructure.Core.DDD.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Borg
{
    public static class IHaveWeightExtensions
    {
        public static IOrderedEnumerable<T> HeavyToLight<T>(this IEnumerable<T> source) where T : IHaveWeight
        {
            return source.OrderByDescending(x => x.Weight);
        }

        public static IOrderedEnumerable<T> LightToHeavy<T>(this IEnumerable<T> source) where T : IHaveWeight
        {
            return source.OrderBy(x => x.Weight);
        }
        public static IOrderedQueryable<T> HeavyToLight<T>(this IQueryable<T> source) where T: IHaveWeight
        {
            return source.OrderByDescending(x => x.Weight);
        }

        public static IOrderedQueryable<T> LightToHeavy<T>(this IQueryable<T> source) where T : IHaveWeight
        {
            return source.OrderBy(x => x.Weight);
        }
    }
}