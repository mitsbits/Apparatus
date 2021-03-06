﻿using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borg
{
    public static class CompositeKeyExtendions
    {
        private static readonly Lazy<ConcurrentDictionary<(Type type, CacheFragment fragment), object>> _cache = new Lazy<ConcurrentDictionary<(Type type, CacheFragment fragment), object>>(() => new ConcurrentDictionary<(Type type, CacheFragment fragment), object>());
        private static ConcurrentDictionary<(Type type, CacheFragment fragment), object> Cache => _cache.Value;

        public async static ValueTask<Expression<Func<T, bool>>> ToPredicate<T>(this CompositeKey key, CancellationToken cancellationToken = default) where T : IIdentifiable
        {
            Expression<Func<T, bool>> predicate = null;
            var cachekey = (type: typeof(T), fragment: CacheFragment.Predicate);
            if (Cache.TryGetValue(cachekey, out var cachehit))
            {
                predicate = cachehit as Expression<Func<T, bool>>;
            }

            if (predicate != null)
            {
                return predicate;
            }
            var exprBuilder = new StringBuilder("x=> ");

            foreach (var prop in typeof(T).GetProperties())
            {
                if (key.TryGetValue(prop.Name, out var value))
                {
                    if (prop.PropertyType.Equals(typeof(string)))
                    {
                        exprBuilder.Append($"x.{prop.Name} == ").Append($"\"{value}\"");
                    }
                    else if (prop.PropertyType.Equals(typeof(bool)))
                    {
                        Func<int> localValue = () => (bool)value ? 1 : 0;

                        exprBuilder.Append($"x.{prop.Name} == {localValue.Invoke()}");
                    }
                    else
                    {
                        exprBuilder.Append($"x.{prop.Name} == {value}");
                    }
                    exprBuilder.Append(" && ");
                }
            }

            var exp = exprBuilder.ToString();
            exp = exp.Substring(0, exp.Length - " && ".Length);
            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);
            predicate = await CSharpScript.EvaluateAsync<Expression<Func<T, bool>>>(exp, options);
            Cache.TryAdd(cachekey, predicate);
            return predicate;
        }

        public async static ValueTask<Expression<Func<T, bool>>> ToPrimaryKey<T>(this CompositeKey key, CancellationToken cancellationToken = default) where T : IIdentifiable
        {
            Expression<Func<T, bool>> expression = null;
            var cachekey = (type: typeof(T), fragment: CacheFragment.Predicate);
            if (Cache.TryGetValue(cachekey, out var cachehit))
            {
                expression = cachehit as Expression<Func<T, bool>>;
            }

            if (expression != null)
            {
                return expression;
            }
            var exprBuilder = new StringBuilder("x=> new { ");

            foreach (var prop in typeof(T).GetProperties())
            {
                if (key.ContainsKey(prop.Name))
                {
                    exprBuilder.Append($" x.{prop.Name}, ");
                }
            }
            var localresult = exprBuilder.ToString();
            var exp = exprBuilder.ToString().Substring(0, localresult.Length - 2) + " }";
            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);
            expression = await CSharpScript.EvaluateAsync<Expression<Func<T, bool>>>(exp, options);
            return expression;
        }

        private enum CacheFragment
        {
            Predicate,
            PrimaryKey,
        }
    }
}