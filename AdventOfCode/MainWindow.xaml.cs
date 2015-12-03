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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdventOfCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Type> _solutions = new Dictionary<string, Type>();

        public MainWindow()
        {
            InitializeComponent();

            Type solutionType = typeof(SolutionBase);
            _solutions = AppDomain.CurrentDomain
                                  .GetAssemblies()
                                  .SelectMany(a => a.GetTypes()
                                                    .Where(t => t.IsClass &&
                                                                !t.IsAbstract &&
                                                                t.IsSubclassOf(solutionType) &&
                                                                t.GetCustomAttributes(typeof(AOCSolutionAttribute), true).Length > 0))
                                  .ToDictionary(k => (k.GetCustomAttributes(typeof(AOCSolutionAttribute), true).First() as AOCSolutionAttribute).Name,
                                                v => v);

            foreach (string name in _solutions.Keys)
            {
                this.SolutionComboBox.Items.Add(name);
            }
            this.SolutionComboBox.SelectedValue = _solutions.Keys.First();
        }

        private void ShowSolutionWindow(object sender, RoutedEventArgs e)
        {
            string selectedSolutionName = (string)this.SolutionComboBox.SelectedValue;
            Type solutionType = _solutions[selectedSolutionName];
            Window solutionWindow = Activator.CreateInstance(solutionType) as Window;
            solutionWindow.ShowDialog();
        }
    }
}
