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
    [AOCSolution("Day 5, part 1")]
    public partial class Solution9 : SolutionBase
    {
        private const int MINIMUM_VOWELS = 3;
        private const int MINIMUM_DUPLICATES = 2;

        public Solution9()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            ToggleUi(false);

            char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
            string[] naughties = new string[] { "ab", "cd", "pq", "xy" };

            this.ResultTextBox.Text = this.InputTextBox
                                          .Text
                                          .Split('\n', '\r')
                                          .Select(s => s.Trim())
                                          .Where(s => !string.IsNullOrWhiteSpace(s) &&
                                                      !naughties.Any(n => s.Contains(n)) &&
                                                      s.Where(c => vowels.Contains(c)).Count() >= MINIMUM_VOWELS &&
                                                      ContainsDuplicates(s))
                                          .Count()
                                          .ToString();

            ToggleUi(true);
        }

        private void ToggleUi(bool toggle)
        {
            this.ProcessButton.IsEnabled = toggle;
            this.ResultTextBox.IsEnabled = toggle;
            this.InputTextBox.IsEnabled = toggle;
        }

        private bool ContainsDuplicates(string check)
        {
            char previous = ' ';
            int duplicatedState = 1;
            foreach (char c in check)
            {
                if (c == previous)
                {
                    if (++duplicatedState >= MINIMUM_DUPLICATES)
                    {
                        return true;
                    }
                }
                else
                {
                    duplicatedState = 1;
                }
                previous = c;
            }
            return false;
        }
    }
}
