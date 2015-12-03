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
    [AOCSolution("Day 1, part 2")]
    public partial class Solution2 : SolutionBase
    {
        public Solution2()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int floor = 0;
            for (; i < this.InputTextBox.Text.Length; i++)
            {
                switch (this.InputTextBox.Text[i])
                {
                    case '(':
                        ++floor;
                        break;
                    case ')':
                        --floor;
                        break;
                }

                if (floor < 0)
                {
                    break;
                }
            }
            this.ResultTextBox.Text = (i + 1).ToString();
        }
    }
}
