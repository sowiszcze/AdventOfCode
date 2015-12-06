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
    [AOCSolution("Day 4, part 2")]
    public partial class Solution8 : SolutionBase
    {
        private const string SEARCH = "000000";

        public Solution8()
        {
            InitializeComponent();
            this.Title = base.GetTitle();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            ToggleUi(false);
            string prefix = this.InputTextBox.Text;
            string result = string.Empty;
            MD5 md5 = MD5.Create();
            string current = string.Empty;
            int i = 0;

            Task task = new Task(() =>
            {
                for (; string.IsNullOrWhiteSpace(result); ++i)
                {
                    byte[] source = ASCIIEncoding.ASCII.GetBytes(string.Format("{0}{1}", prefix, i));
                    current = BitConverter.ToString(md5.ComputeHash(source)).Replace("-", "");
                    if (current.StartsWith(SEARCH))
                    {
                        result = i.ToString();
                    }
                }
            });
            task.Start();
            task.Wait();

            this.ResultTextBox.Text = result;
            ToggleUi(true);
            MessageBox.Show("Processing finished!");
        }

        private void ToggleUi(bool toggle)
        {
            this.ProcessButton.IsEnabled = toggle;
            this.ResultTextBox.IsEnabled = toggle;
            this.InputTextBox.IsEnabled = toggle;
        }
    }
}
