using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
 
namespace Hx.Extensions.FluentMapper
{
    public static class MutationAgent
    {
        #region Constants and Variables
        private static readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private static readonly Dictionary<string, object> serumCache = new Dictionary<string, object>();
        #endregion Constants and Variables

        internal static Func<TSource, TResult> CreateMutationSerum<TSource, TResult>() where TResult : class,new()
        {
            string key = typeof(TSource).FullName + typeof(TResult).FullName;
            return GetLamdaMapping<TSource, TResult>(key);
        }

        private static Func<TSource, TResult> CreateLambdaMap<TSource, TResult>(string key) where TResult : class,new()
        {
            if (serumCache.ContainsKey(key))
            {
                // In case someone wrote while we were entering the write
                return serumCache[key] as Func<TSource, TResult>;
            }
            else
            {
                ParameterExpression source = Expression.Parameter(typeof(TSource), "x");
                Expression expr = null;

                Dictionary<string, PropertyInfo> sourceProps = typeof(TSource).GetProperties().ToDictionary<PropertyInfo, string>(x => x.Name);
                IEnumerable<string> sourcePropertyNames = sourceProps.Values.Select(x => x.Name);
                PropertyInfo[] matchingProperties = typeof(TResult).GetProperties().Where(x => sourcePropertyNames.Contains(x.Name)).ToArray();
                IEnumerable<MemberBinding> bindings = matchingProperties.Select(p => Expression.Bind(p, Expression.Property(source, sourceProps[p.Name]))).OfType<MemberBinding>();

                LambdaExpression map = Expression.Lambda<Func<TSource, TResult>>(Expression.MemberInit(
                                                    Expression.New(typeof(TResult).GetConstructor(Type.EmptyTypes)), bindings), source);
                var lambda = map.Compile();
                serumCache[key] = lambda;
                return lambda as Func<TSource, TResult>;
            }
        }

        private static Func<TSource, TResult> GetLamdaMapping<TSource, TResult>(string key) where TResult : class,new()
        {
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (serumCache.ContainsKey(key))
                {
                    return serumCache[key] as Func<TSource, TResult>;
                }
                else
                {
                    cacheLock.EnterWriteLock();
                    try
                    {
                        return CreateLambdaMap<TSource, TResult>(key);
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }
    }
}
