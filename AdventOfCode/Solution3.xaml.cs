using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    [AOCSolution("Day 2, part 1")]
    public partial class Solution3 : SolutionBase
    {
        public Solution3()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"(\d+)x(\d+)x(\d+)", RegexOptions.Compiled);

            this.ResultTextBox.Text = this.InputTextBox
                                          .Text
                                          .Split('\n', '\r')
                                          .Where(s => !string.IsNullOrWhiteSpace(s))
                                          .Select(s => regex.Match(s))
                                          .Select(m => new Box { Width = int.Parse(m.Groups[1].Value), Height = int.Parse(m.Groups[2].Value), Length = int.Parse(m.Groups[3].Value) })
                                          .Sum(b => b.Wrapping)
                                          .ToString();
        }

        private class Box
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int Length { get; set; }

            public int Wrapping
            {
                get
                {
                    int[] surface = new int[]
                    {
                        this.Height * this.Length,
                        this.Width * this.Length,
                        this.Width * this.Height
                    };

                    return surface.Sum() * 2 + surface.Min();
                }
            }
        }
    }
}
