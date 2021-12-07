using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaHelper
{
    class Program
    {
        
        //Пример raw формулы
        //string rawFormula = "¬X1∧X2∧X3∧¬X4∧¬X5∨X1∧X2∧X3∧¬X4∧¬X5∨¬X1∧X2∧¬X3∧X4∧¬X5∨X1∧X2∧¬X3∧X4∧X5∨X1∧¬X2∧X3∧X4∧X5∨¬X1∧¬X2∧¬X3∧¬X4∧X5∨X1∧X2∧¬X3∧¬X4∧X5∨¬X1∧X2∧¬X3∧¬X4∧X5";
        static string FormatFormulaToWord(string rawFormula)
        {
            string FormatFormula = rawFormula.Replace("¬", "\\overbar");

            FormatFormula = FormatFormula.Replace("1", "!");
            FormatFormula = FormatFormula.Replace("2", "@");
            FormatFormula = FormatFormula.Replace("3", "#");
            FormatFormula = FormatFormula.Replace("4", "$");
            FormatFormula = FormatFormula.Replace("5", "%");

            FormatFormula = FormatFormula.Replace("!", "0");
            FormatFormula = FormatFormula.Replace("@", "1");
            FormatFormula = FormatFormula.Replace("#", "2");
            FormatFormula = FormatFormula.Replace("$", "3");
            FormatFormula = FormatFormula.Replace("%", "4");

            FormatFormula = FormatFormula.Replace("rX0", "r(X0)");
            FormatFormula = FormatFormula.Replace("rX1", "r(X1)");
            FormatFormula = FormatFormula.Replace("rX2", "r(X2)");
            FormatFormula = FormatFormula.Replace("rX3", "r(X3)");
            FormatFormula = FormatFormula.Replace("rX4", "r(X4)");

            //old Version
            /*int num1 = 1;
            var chars1 = num1.ToString().Select(c => (char)('₀' + c - '0'));
            int num2 = 2;
            var chars2 = num2.ToString().Select(c => (char)('₀' + c - '0'));
            int num3 = 3;
            var chars3 = num3.ToString().Select(c => (char)('₀' + c - '0'));
            int num4 = 4;
            var chars4 = num4.ToString().Select(c => (char)('₀' + c - '0'));*/

            /*FormatFormula = FormatFormula.Replace('1', chars1.First());
            FormatFormula = FormatFormula.Replace('2', chars2.First());
            FormatFormula = FormatFormula.Replace('3', chars3.First());
            FormatFormula = FormatFormula.Replace('4', chars4.First());*/

            FormatFormula = FormatFormula.Replace("0", "_0");
            FormatFormula = FormatFormula.Replace("1", "_1");
            FormatFormula = FormatFormula.Replace("2", "_2");
            FormatFormula = FormatFormula.Replace("3", "_3");
            FormatFormula = FormatFormula.Replace("4", "_4");
            return FormatFormula;
        }
        //MatchEvaluator С#9.0 switch expression
        public static string MA(Match match) => match.Value switch
        {
            
            "¬X1" => "\\overbar(X_0)",
            "¬X2" => "\\overbar(X_1)",
            "¬X3" => "\\overbar(X_2)",
            "¬X4" => "\\overbar(X_3)",
            "¬X5" => "\\overbar(X_4)",
            "X1" => "X_0",
            "X2" => "X_1",
            "X3" => "X_2",
            "X4" => "X_3",
            "X5" => "X_4",
            _ => match.Value

        };
        //MatchEvaluator native switch
        public static string MA1(Match match)
        {
            switch (match.Value)
            {
                //С#9.0 switch expression
                case string s when s.StartsWith('X'):
                    return @$"X_{(char)(s[1] - 1)}";
                case string s when s.StartsWith("¬X"):
                    return @$"\overbar(X_{(char)(s[2] - 1)})";
                default:
                    return match.Value;
            }
        }
        //Подход через MatchEvaluator
        public static string RegexReplace(string text, MatchEvaluator ma)
        {
            string output;

            output = text;

            output = Regex.Replace(text, @"[¬{1}]+[X{1}]+[\d{1}]|[X{1}]+[\d{1}]", ma);

            return output;
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string rawFormule = "¬X1∧X2∧X3∧¬X4∧¬X5∨X1∧X2∧X3∧¬X4∧¬X5∨¬X1∧X2∧¬X3∧X4∧¬X5∨X1∧X2∧¬X3∧X4∧X5∨X1∧¬X2∧X3∧X4∧X5∨¬X1∧¬X2∧¬X3∧¬X4∧X5∨X1∧X2∧¬X3∧¬X4∧X5∨¬X1∧X2∧¬X3∧¬X4∧X5";

            string FormattedFormula = FormatFormulaToWord(rawFormule);

            string FormattedFormula1 = RegexReplace(rawFormule, MA);

            string FormattedFormula2 = RegexReplace(rawFormule, MA1);

            Console.ReadLine();
        }
    }
}
