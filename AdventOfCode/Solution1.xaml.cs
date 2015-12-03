using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdventOfCode
{
    /// <summary>
    /// Interaction logic for Solution1.xaml
    /// </summary>
    [AOCSolution("Day 1, part 1")]
    public partial class Solution1 : SolutionBase
    {
        public Solution1()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            this.ResultTextBox.Text = (this.InputTextBox.Text.Count(c => c == '(') - this.InputTextBox.Text.Count(c => c == ')')).ToString();
        }
    }
}
