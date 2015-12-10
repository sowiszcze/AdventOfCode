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
    [AOCSolution("Day 7, part 2")]
    public partial class Solution14 : SolutionBase
    {
        private const string LEFT_SHIFT = "LSHIFT";
        private const string RIGHT_SHIFT = "RSHIFT";
        private const string AND = "AND";
        private const string OR = "OR";

        private readonly Dictionary<string, List<Point>> _missingReferences;
        private readonly Dictionary<string, List<Point>> _dependencies;
        private readonly Dictionary<string, Point> _points;

        public Solution14()
        {
            InitializeComponent();
            this.Title = base.GetTitle();

            _missingReferences = new Dictionary<string, List<Point>>();
            _dependencies = new Dictionary<string, List<Point>>();
            _points = new Dictionary<string, Point>();
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            ToggleUi(false);

            _missingReferences.Clear();
            _dependencies.Clear();
            _points.Clear();
            
            Regex value = new Regex(@"^(\w+) \-> ([a-z]+)$", RegexOptions.Compiled);
            Regex not = new Regex(@"^NOT (\w+) \-> ([a-z]+)$", RegexOptions.Compiled);
            Regex shift = new Regex(string.Format(@"^(\w+) ({0}|{1}) (\d+) \-> ([a-z]+)$", LEFT_SHIFT, RIGHT_SHIFT), RegexOptions.Compiled);
            Regex andOr = new Regex(string.Format(@"^(\w+) ({0}|{1}) (\w+) \-> ([a-z]+)$", AND, OR), RegexOptions.Compiled);

            foreach (string line in this.InputTextBox.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()))
            {
                Point point = new Point();
                Match match;
                if (value.IsMatch(line))
                {
                    match = value.Match(line);
                    point.Operation = Operation.Set;
                    point.Name = match.Groups[2].Value;
                    point.Reference1 = match.Groups[1].Value;
                }
                else if (not.IsMatch(line))
                {
                    match = not.Match(line);
                    point.Operation = Operation.Not;
                    point.Name = match.Groups[2].Value;
                    point.Reference1 = match.Groups[1].Value;
                }
                else if (shift.IsMatch(line))
                {
                    match = shift.Match(line);
                    switch (match.Groups[2].Value)
                    {
                        case RIGHT_SHIFT:
                            point.Operation = Operation.RightShift;
                            break;
                        case LEFT_SHIFT:
                            point.Operation = Operation.LeftShift;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    point.Name = match.Groups[4].Value;
                    point.Reference1 = match.Groups[1].Value;
                    point.Shift = Convert.ToByte(match.Groups[3].Value);
                }
                else if (andOr.IsMatch(line))
                {
                    match = andOr.Match(line);
                    switch (match.Groups[2].Value)
                    {
                        case AND:
                            point.Operation = Operation.And;
                            break;
                        case OR:
                            point.Operation = Operation.Or;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    point.Name = match.Groups[4].Value;
                    point.Reference1 = match.Groups[1].Value;
                    point.Reference2 = match.Groups[3].Value;
                }

                ushort test;
                if (!ushort.TryParse(point.Reference1, out test))
                {
                    AddDependency(point.Reference1, point);
                }
                if (!string.IsNullOrWhiteSpace(point.Reference2) && !ushort.TryParse(point.Reference2, out test))
                {
                    AddDependency(point.Reference2, point);
                }

                _points.Add(point.Name, point);
                point.Update(_missingReferences, _points);
            }

            _points["b"].Operation = Operation.Set;
            _points["b"].Value = _points["a"].Value;
            if (_dependencies.ContainsKey(_points["b"].Reference1))
            {
                _dependencies[_points["b"].Reference1].Remove(_points["b"]);
            }
            if (!string.IsNullOrWhiteSpace(_points["b"].Reference2) && _dependencies.ContainsKey(_points["b"].Reference2))
            {
                _dependencies[_points["b"].Reference2].Remove(_points["b"]);
            }
            _points["b"].Reference1 = _points["a"].Value.ToString();
            _points["b"].Reference2 = string.Empty;
            _points["b"].Shift = 0;
            _points["a"].UpdateIfInTree(_points["b"], new List<Point>());

            this.ResultTextBox.Text = _points["a"].Value.ToString();

            ToggleUi(true);
        }

        private void ToggleUi(bool toggle)
        {
            this.ProcessButton.IsEnabled = toggle;
            this.ResultTextBox.IsEnabled = toggle;
            this.InputTextBox.IsEnabled = toggle;
        }

        private void AddDependency(string dependantOn, Point point)
        {
            if (!_dependencies.ContainsKey(dependantOn))
            {
                _dependencies.Add(dependantOn, new List<Point>());
            }

            if (!_dependencies[dependantOn].Contains(point))
            {
                _dependencies[dependantOn].Add(point);
            }
        }
        
        private enum Operation
        {
            Not,
            Or,
            And,
            LeftShift,
            RightShift,
            Set
        }

        private class Point
        {
            public string Name { get; set; }
            public Operation Operation { get; set; }
            public string Reference1 { get; set; }
            public Point Dependency1 { get; set; }
            public string Reference2 { get; set; }
            public Point Dependency2 { get; set; }
            public ushort? Value { get; set; }
            public byte Shift { get; set; }
            public bool HasValue { get { return this.Value.HasValue; } }

            public void Update(Dictionary<string, List<Point>> missingReferences, Dictionary<string, Point> points, bool deleteResolved = true)
            {
                if (this.CanSetValue(points))
                {
                    ushort value1 = 0,
                           value2 = 0;
                    if (!ushort.TryParse(this.Reference1, out value1))
                    {
                        this.Dependency1 = points[this.Reference1];
                    }
                    if (!string.IsNullOrWhiteSpace(this.Reference2) && !ushort.TryParse(this.Reference2, out value2))
                    {
                        this.Dependency2 = points[this.Reference2];
                    }
                    UpdateValue();
                }

                if (this.HasValue)
                {
                    if (missingReferences.ContainsKey(this.Name))
                    {
                        List<Point> dependencies = missingReferences[this.Name];

                        if (deleteResolved)
                        {
                            missingReferences.Remove(this.Name);
                        }

                        foreach (Point dependency in dependencies)
                        {
                            dependency.Update(missingReferences, points, deleteResolved);
                        }
                    }
                }
                else
                {
                    AddToMissing(missingReferences, this.Reference1);
                    switch (this.Operation)
                    {
                        case Solution14.Operation.And:
                        case Solution14.Operation.Or:
                            AddToMissing(missingReferences, this.Reference2);
                            break;
                    }
                }
            }

            public bool UpdateIfInTree(Point point, List<Point> stack)
            {
                if (stack.Contains(this))
                {
                    return false;
                }

                List<Point> newStack = stack.Select(l => l).ToList();
                newStack.Add(this);

                bool inTreeFirst = this.Dependency1 != null && this.Dependency1.UpdateIfInTree(point, newStack),
                     inTreeSecond = this.Dependency2 != null && this.Dependency2.UpdateIfInTree(point, newStack);

                if (this.Dependency1 == point || 
                    this.Dependency2 == point ||
                    inTreeFirst ||
                    inTreeSecond)
                {
                    UpdateValue();
                    return true;
                }

                return false;
            }

            private void UpdateValue()
            {
                ushort value1 = 0,
                       value2 = 0;

                if (!ushort.TryParse(this.Reference1, out value1))
                {
                    value1 = this.Dependency1.Value.Value;
                }
                if (!string.IsNullOrWhiteSpace(this.Reference2) && !ushort.TryParse(this.Reference2, out value2))
                {
                    value2 = this.Dependency2.Value.Value;
                }

                switch (this.Operation)
                {
                    case Solution14.Operation.Set:
                        this.Value = value1;
                        break;
                    case Solution14.Operation.Not:
                        this.Value = (ushort)~value1;
                        break;
                    case Solution14.Operation.LeftShift:
                        this.Value = (ushort)(value1 << this.Shift);
                        break;
                    case Solution14.Operation.RightShift:
                        this.Value = (ushort)(value1 >> this.Shift);
                        break;
                    case Solution14.Operation.And:
                        this.Value = (ushort)(value1 & (ushort)value2);
                        break;
                    case Solution14.Operation.Or:
                        this.Value = (ushort)(value1 | (ushort)value2);
                        break;
                }
            }

            private void AddToMissing(Dictionary<string, List<Point>> missingReferences, string key)
            {
                if (!missingReferences.ContainsKey(key))
                {
                    missingReferences.Add(key, new List<Point>());
                }

                if (!missingReferences[key].Contains(this))
                {
                    missingReferences[key].Add(this);
                }
            }

            private bool CanSetValue(Dictionary<string, Point> points)
            {
                ushort temp;
                switch (this.Operation)
                {
                    case Solution14.Operation.Not:
                    case Solution14.Operation.LeftShift:
                    case Solution14.Operation.RightShift:
                    case Solution14.Operation.Set:
                        return points.ContainsKey(this.Reference1) && points[this.Reference1].HasValue || ushort.TryParse(this.Reference1, out temp);
                    case Solution14.Operation.And:
                    case Solution14.Operation.Or:
                        return (points.ContainsKey(this.Reference1) && points[this.Reference1].HasValue || ushort.TryParse(this.Reference1, out temp))
                            && (points.ContainsKey(this.Reference2) && points[this.Reference2].HasValue || ushort.TryParse(this.Reference2, out temp));
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
