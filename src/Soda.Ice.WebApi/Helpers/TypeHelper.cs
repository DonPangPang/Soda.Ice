using Soda.Ice.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Soda.Ice.Common.Helpers
{
    public class TypeHelper
    {
        public static IEnumerable<Type> GetEntityTypes()
        {
            return Assembly.Load("Soda.Ice.Domain").GetTypes().Where(x => (x.IsAssignableTo(typeof(IEntity)) || x.IsAssignableTo(typeof(EntityBase))) && !x.IsInterface && !x.IsAbstract);
        }

        public static IEnumerable<Type> GetViewModelTypes()
        {
            return Assembly.Load("Soda.Ice.Shared").GetTypes().Where(x => (x.IsAssignableTo(typeof(IViewModel)) || x.IsAssignableTo(typeof(ViewModel))) && !x.IsInterface && !x.IsAbstract);
        }
    }
}