using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace Multiplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            var expression = textBox1.Text;
            var regex = new Regex(@"\([x]-\d+(\,\d+)?\)");

            // Validate the expression
            if (!regex.IsMatch(expression))
            {
                label3.Visible = true;
            }

            // Split expression on parts and distinguish the roots into array
            var rootRegex = new Regex(@"\d+(\,\d+)?");
            var parts = regex.Matches(expression);
            var rootsArray = new List<float>();
            foreach (Match part in parts)
                rootsArray.Add(float.Parse(rootRegex.Match(part.Value).Value));


            // Calculate 
            var result = new List<CoefficientOfX>();
            foreach (var root in rootsArray)
            {
                result = Multiplicate(root, result);
            }

            // Summarize
            var results = result.GroupBy(p => p.Power,
                               p => p.Coefficient,
                               (power, g) => new {
                                   Power = power,
                                   Coefficient = Math.Round(g.Sum(), 2)
                               }
                              );

            // Format result
            var resString = "";
            bool isEven = false;
            foreach (var item in results.OrderByDescending(t => t.Power))
            {
                resString += (item.Coefficient == 1 ? "" : item.Coefficient.ToString()) + "x" + (item.Power <= 1 ? "" : "^" + item.Power);
                resString += isEven ? "+" : "-";
                isEven = !isEven;
            }
            resString = resString.TrimEnd('-', '+');

            // Print result
            textBox2.Text = resString;
        }

        private List<CoefficientOfX> Multiplicate(float n1, List<CoefficientOfX> currentResult)
        {
            var result = new List<CoefficientOfX>();
            if (currentResult.Count == 0)
            {
                result.Add(new CoefficientOfX { Power = 1, Coefficient = 1 }); // x
                result.Add(new CoefficientOfX { Power = 0, Coefficient = n1 }); // 2
            }
            else
            {
                foreach(var item in currentResult)
                {
                    result.Add(new CoefficientOfX { Power = item.Power + 1, Coefficient = item.Coefficient });
                    result.Add(new CoefficientOfX { Power = item.Power, Coefficient = item.Coefficient * n1 });
                }
            }

            return result;
        }

        class CoefficientOfX
        {
            public int Power { get; set; }
            public float Coefficient { get; set; }
        }
    }
}
