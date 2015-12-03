using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class AOCSolutionAttribute : Attribute
    {
        public AOCSolutionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
