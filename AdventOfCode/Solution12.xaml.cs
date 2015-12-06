using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    [AOCSolution("Day 6, part 2")]
    public partial class Solution12 : SolutionBase
    {
        private const string TURN_OFF = "turn off";
        private const string TURN_ON = "turn on";
        private const string TOGGLE = "toggle";

        private byte[,] _mesh;

        public Solution12()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
            _mesh = new byte[1000, 1000];
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            ToggleUi(false);

            Regex parser = new Regex(@"([\w ]+) (\d+),(\d+) through (\d+),(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (string line in this.InputTextBox.Text.Split('\n', '\r').Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                Match parsed = parser.Match(line);
                string command = parsed.Groups[1].Value;
                Coordinate start = new Coordinate(int.Parse(parsed.Groups[2].Value), int.Parse(parsed.Groups[3].Value));
                Coordinate end = new Coordinate(int.Parse(parsed.Groups[4].Value), int.Parse(parsed.Groups[5].Value));
                Func<byte, byte> function = (state) => { return state; };

                switch (command)
                {
                    case TURN_OFF:
                        function = (state) => { return (byte)(state == 0 ? 0 : state - 1); };
                        break;
                    case TURN_ON:
                        function = (state) => { return (byte)(state + 1); };
                        break;
                    case TOGGLE:
                        function = (state) => { return (byte)(state + 2); };
                        break;
                }

                for (int x = start.X; x <= end.X; ++x)
                {
                    for (int y = start.Y; y <= end.Y; ++y)
                    {
                        _mesh[x, y] = function(_mesh[x, y]);
                    }
                }
            }

            this.ResultTextBox.Text = _mesh.Sum(i => i).ToString();

            ToggleUi(true);
        }

        private void ToggleUi(bool toggle)
        {
            this.ProcessButton.IsEnabled = toggle;
            this.ResultTextBox.IsEnabled = toggle;
            this.InputTextBox.IsEnabled = toggle;
        }

        private class Coordinate
        {
            public Coordinate() { }

            public Coordinate(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
