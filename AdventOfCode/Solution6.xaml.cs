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
    [AOCSolution("Day 3, part 2")]
    public partial class Solution6 : SolutionBase
    {
        public Solution6()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            Position[] actual = new Position[] { new Position(), new Position() };
            List<Position> positions = new List<Position> { new Position() };

            for (int i = 0; i < this.InputTextBox.Text.Length; ++i)
            {
                char movement = this.InputTextBox.Text[i];
                Position santa = actual[i % actual.Length];
                switch (movement)
                {
                    case '^':
                        ++santa.Latitude;
                        break;
                    case 'v':
                        --santa.Latitude;
                        break;
                    case '>':
                        ++santa.Longitude;
                        break;
                    case '<':
                        --santa.Longitude;
                        break;
                }

                if (!positions.Contains(santa))
                {
                    positions.Add(santa.Clone());
                }
            }

            this.ResultTextBox.Text = positions.Count.ToString();
        }

        private class Position : ICloneable
        {
            public Position()
            {
                this.Latitude = 0;
                this.Longitude = 0;
            }

            public int Latitude { get; set; }
            public int Longitude { get; set; }

            public override bool Equals(object obj)
            {
                Position compare = obj as Position;
                return compare != null && compare.Latitude == this.Latitude && compare.Longitude == this.Longitude;
            }

            public override int GetHashCode()
            {
                return string.Format("{0}|{1}", this.Longitude, this.Latitude).GetHashCode();
            }

            public Position Clone()
            {
                return this.MemberwiseClone() as Position;
            }

            object ICloneable.Clone()
            {
                return this.Clone();
            }
        }
    }
}
