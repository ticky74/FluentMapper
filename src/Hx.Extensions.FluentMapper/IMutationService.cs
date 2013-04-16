using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hx.Extensions.FluentMapper
{
    public interface IMutationService
    {
        TResult Mutate<TSource, TResult>(TSource source);
    }
}
