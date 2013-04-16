using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Hx.Extensions.FluentMapper
{
    public class TypeMutation<TSource> where TSource : class
    {
        #region Constants and Variables
        private readonly IMutationService mutationService;
        private readonly IEnumerable<TSource> source;
        #endregion Constants and Variables

        #region Constructors
        
        internal TypeMutation(IEnumerable<TSource> source)
        {
            this.mutationService = new AutoMapperMutatationService();
            this.source = source;
        }
        #endregion Constructors

        #region Methods

        public IEnumerable<TResult> Into<TResult>()
            where TResult : class, new()
        {
            if (typeof(TResult).IsAssignableFrom(typeof(TSource)))
            {
                // If a simple cast will work, use it
                return this.source.OfType<TResult>();
            }
            else
            {
                return this.source.Select(x => this.mutationService.Mutate<TSource, TResult>(x));
            }
        }
        #endregion Methods
    }
}
