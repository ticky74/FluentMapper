using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hx.Extensions.FluentMapper
{
    public static class TypeMutator
    {
        #region Constants and Variables
        private static IMutationService mutationService;
        #endregion Constants and Variables

        #region Properties
        public static IMutationService MutationService {
            get
            {
                if (TypeMutator.mutationService == null)
                {
                    TypeMutator.mutationService = new AutoMapperMutatationService();
                }
                return TypeMutator.mutationService;
            }
            set
            {
                TypeMutator.mutationService = value;
            }
        }
        #endregion Properties
    }
}
