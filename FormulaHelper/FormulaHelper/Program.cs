using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaHelper
{
    class Program
    {
        

        public static string RM_AND(string raw_formula) => Regex.Replace(raw_formula, @"[\s]*∧[\s]*", @" \; "); // todo: добавить рассмотрение скобок

        public static string Basis(string raw_formula)
        {
            string[] components = raw_formula.Split("∨");
            for (int i = 0; i < components.Length; i++) components[i] = "¬(" + components[i] + ")";
            return "¬(" + String.Join("∧", components) + ")";
        }

        public static string Over_MA(Match m)
        {
            Console.WriteLine($"#{m.Value}#");
            string output = m.Value;
            if (output != "")
            {
                output = output.TrimStart('¬');
                if (output.StartsWith("(") && output.EndsWith(")")) output = output.Substring(1, output.Length - 2);
                return @" \overline{" + output + @"} ";
            }
            else
            {
                Console.WriteLine($"${output}$");
                return "";
            }
        }

        public static string OVER_MA(Match m) => (m.Value.StartsWith("¬(") && m.Value.EndsWith(")")) ? m.Value.Substring(2, m.Value.Length - 3) : m.Value.TrimStart('¬');

        public static string LatexConverter(string raw_formula)
        {
            string output = raw_formula;
            var symbols = new Dictionary<string, string>()
            {
                ["∧"] = @"\land",
                ["∨"] = @"\vee",
                ["∩"] = @"\cap",
                ["∪"] = @"\cup"
            };
            output = Regex.Replace(output, @"X[^\d]", "X1"); // X → X1 (X0)
            while (output.Contains("¬"))
            {
                output = Regex.Replace(output, @"(¬\([^\(\)]*\))|(¬\-?X[\d]*)", Over_MA); // только для одиночных иксов или скобок
            }
            output = Regex.Replace(output, @"(?<=X[\-_\^\d]*[\s\}\\\)]*)[^\w\s\d\\{};](?=[\-\\\(\s]*((X)|(\\over)))", m => @$" {symbols.GetValueOrDefault(m.Value, m.Value)} "); // Замена операндов
            output = Regex.Replace(output, @"X[\d]*", m => @$"X_{{{Convert.ToInt32(m.Value.Substring(m.Value.IndexOf('X') + 1)) - 1}}} "); // Уменьшение индексов
            output = Regex.Replace(output, @"\^[\-\d]*", m => @$"^{{{m.Value.TrimStart('^')}}}"); // Степени
            output = Regex.Replace(output, @"\^\{[\d]\}*(?=[^\}\s])", m => @$"{m.Value} "); // todo: Костыль со степенями и пробелами
            output = Regex.Replace(output, @"[\s]+", " ");
            return output;
        }

        static void Main(string[] args)
        {
            string rawFormule = "¬X1∧¬X2∧¬X3∧¬X4∧¬X5∨X1∧X2∧X3∧¬X4∧¬X5∨X1∧¬X2∧X3∧¬X4∧¬X5∨¬X1∧¬X2∧X3∧¬X4∧¬X5∨¬X1∧X2∧¬X3∧X4∧¬X5∨¬X1∧X2∧X3∧X4∧¬X5∨¬X1∧¬X2∧¬X3∧¬X4∧X5∨¬X1∧X2∧¬X3∧¬X4∧X5";
            Console.WriteLine("raw\t" + rawFormule + "\n");

            string output = RM_AND(rawFormule);
            Console.WriteLine("step1\t" + output + "\n");
            output = Basis(output);
            Console.WriteLine("step2\t" + output + "\n");
            output = LatexConverter(output);

            Console.WriteLine("final\t" + output + "\n");
            Console.ReadLine();
        }
    }
}
