using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdventOfCode
{
    public abstract class SolutionBase : Window
    {
        public string GetTitle()
        {
            return (this.GetType().GetCustomAttributes(typeof(AOCSolutionAttribute), true).First() as AOCSolutionAttribute).Name;
        }
    }
}
