using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaHelper
{
    class Program
    {
        public static string RegexReplace(string text)
        {
            string output = text;
            var symbols = new Dictionary<string, string>()
            {
                ["∧"] = @"\land",
                ["∨"] = @"\vee",
                ["∩"] = @"\cap",
                ["∪"] = @"\cup"
            };
            // отрицательные значения также предусмотрены
            output = Regex.Replace(output, @"[\s]*", ""); // убрать все пробелы из исходной строки
            output = Regex.Replace(output, @"X[^\d]", "X1"); // X → X1 (X0)
            output = Regex.Replace(output, @"(¬\(.*\))|(¬[\-\^\w]*)", m => @"\overline{" + Regex.Replace(m.Value, @"¬|\(|\)", "") + @"}"); // Замена отрицания
            output = Regex.Replace(output, @"(?<=X[\-_\^\d]*\}?\)?)[^\d](?=[\-\(]*((X)|(\\over)))", m => @$" {symbols.GetValueOrDefault(m.Value, m.Value)} "); // Замена операндов
            output = Regex.Replace(output, @"X[\d]*", m => @$"X_{{{Convert.ToInt32(m.Value.Substring(m.Value.IndexOf('X') + 1)) - 1}}}"); // Уменьшение индексов
            output = Regex.Replace(output, @"\^[\-\d]*", m => @$"^{{{m.Value.TrimStart('^')}}}"); // Степени
            output = Regex.Replace(output, @"\^\{[\d]\}*(?=[^\}\s])", m => @$"{m.Value} "); // todo: Костыль со степенями и пробелами
            return output;
        }

        static void Main(string[] args)
        {
            string rawFormule = "¬-X12^2∧-X2∨X3∧-¬X4∧¬(X5^-5∧-X10)∨X1^2¬X2^3∧X2∧X3";
            Console.WriteLine(rawFormule + "\n");
            Console.WriteLine(RegexReplace(rawFormule) + "\n");
            Console.ReadLine();
        }
    }
}
