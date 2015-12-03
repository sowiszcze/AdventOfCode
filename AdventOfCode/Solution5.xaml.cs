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
    [AOCSolution("Day 3, part 1")]
    public partial class Solution5 : SolutionBase
    {
        public Solution5()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            Position actual = new Position();
            List<Position> positions = new List<Position> { actual.Clone() };

            foreach (char movement in this.InputTextBox.Text)
            {
                switch (movement)
                {
                    case '^':
                        ++actual.Latitude;
                        break;
                    case 'v':
                        --actual.Latitude;
                        break;
                    case '>':
                        ++actual.Longitude;
                        break;
                    case '<':
                        --actual.Longitude;
                        break;
                }
                
                if (!positions.Contains(actual))
                {
                    positions.Add(actual.Clone());
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
