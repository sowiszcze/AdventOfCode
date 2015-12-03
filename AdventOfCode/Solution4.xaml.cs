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
    [AOCSolution("Day 2, part 2")]
    public partial class Solution4 : SolutionBase
    {
        public Solution4()
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
                                          .Sum(b => b.Ribbon)
                                          .ToString();
        }

        private class Box
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int Length { get; set; }

            public int Ribbon
            {
                get
                {
                    int[] surface = new int[]
                    {
                        this.Height,
                        this.Length,
                        this.Width
                    };

                    return (surface.Sum() - surface.Max()) * 2 + surface.Product();
                }
            }
        }
    }
}
