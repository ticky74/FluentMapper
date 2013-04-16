using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hx.Extensions.FluentMapper.Testing
{
    public class UnrelatedThing
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SomeOtherValue { get; set; }
        public int Age { get; set; }

        public UnrelatedThing()
        {
            this.Age = 24;
        }
    }
}
