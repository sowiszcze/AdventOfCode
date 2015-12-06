using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    [AOCSolution("Day 4, part 1")]
    public partial class Solution7 : SolutionBase
    {
        private const string SEARCH = "00000";

        public Solution7()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            string prefix = this.InputTextBox.Text;
            string result = string.Empty;
            MD5 md5 = MD5.Create();
            string current = string.Empty;

            for (int i = 0; string.IsNullOrWhiteSpace(result); ++i)
            {
                byte[] source = ASCIIEncoding.ASCII.GetBytes(string.Format("{0}{1}", prefix, i));
                current = BitConverter.ToString(md5.ComputeHash(source)).Replace("-", "");
                if (current.StartsWith(SEARCH))
                {
                    result = i.ToString();
                }
            }

            this.ResultTextBox.Text = result;
        }
    }
}
