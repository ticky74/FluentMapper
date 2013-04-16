using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hx.Extensions.FluentMapper
{
    public class AutoMapperMutatationService : IMutationService
    {
        public TResult Mutate<TSource, TResult>(TSource source)
        {
            try
            {
                return AutoMapper.Mapper.Map<TSource, TResult>(source);
            }
            catch (AutoMapper.AutoMapperMappingException)
            {
                // At most this happens once per app cycle so avoiding
                // the check on every invocation.
                AutoMapper.Mapper.CreateMap<TSource, TResult>();
                return AutoMapper.Mapper.Map<TSource, TResult>(source);
            }
        }
    }
}
