using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Hx.Extensions.FluentMapper
{
    public static class MutatorExtension
    {
        public static TypeMutation<TInput> Mutate<TInput>(this IEnumerable<TInput> source) where TInput : class
        {
            return new TypeMutation<TInput>(source);
        }
    }
}
