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
    [AOCSolution("Day 5, part 2")]
    public partial class Solution10 : SolutionBase
    {
        private const int MINIMUM_VOWELS = 3;
        private const int MINIMUM_DUPLICATES = 2;

        public Solution10()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            ToggleUi(false);

            this.ResultTextBox.Text = this.InputTextBox
                                          .Text
                                          .Split('\n', '\r')
                                          .Select(s => s.Trim())
                                          .Where(s => !string.IsNullOrWhiteSpace(s) &&
                                                      Check(s))
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

        private bool Check(string check)
        {
            bool predicate1 = false;
            bool predicate2 = false;
            for (int i = 0; i < check.Length - 1 && !predicate1; ++i)
            {
                string first = check.Substring(i, 2);
                for (int j = i + 2; j < check.Length - 1 && !predicate1; ++j)
                {
                    if (first == check.Substring(j, 2))
                    {
                        predicate1 = true;
                    }
                }
            }

            for (int i = 0; i < check.Length - 2 && !predicate2; ++i)
            {
                if (check[i] == check[i + 2])
                {
                    predicate2 = true;
                }
            }

            return predicate1 && predicate2;
        }
    }
}
