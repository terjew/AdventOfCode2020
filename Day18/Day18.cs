using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace Day18
{



    class Day18
    {
        static void Test()
        {
            Dictionary<string, int> testdata1 = new Dictionary<string, int>()
            {
                {"1 + 2 * 3 + 4 * 5 + 6", 71 },
                {"1 + (2 * 3) + (4 * (5 + 6))", 51 },
                {"2 * 3 + (4 * 5)", 26 },
                {"5 + (8 * 3 + 9 + 3 * 4 * 3)", 437 },
                {"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240 },
                {"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632 },
                { "(5 + (2 + 9 * 7 + 7) + 3 * 2) * 5 + (2 * 7 + 6)", 940 }
            };
            foreach (var test in testdata1)
            {
                int pos = 0;
                if (SolveExpressionLeftToRight(test.Key, ref pos) != test.Value) Console.WriteLine($"Error, {test.Key} != {test.Value}");
            }

            Dictionary<string, int> testdata2 = new Dictionary<string, int>()
            {
                {"1 + 2 * 3 + 4 * 5 + 6", 231 },
                {"1 + (2 * 3) + (4 * (5 + 6))", 51 },
                {"2 * 3 + (4 * 5)", 46 },
                {"5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445 },
                {"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060 },
                {"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340 },
                //{ "(5 + (2 + 9 * 7 + 7) + 3 * 2) * 5 + (2 * 7 + 6)", 940 }
            };
            foreach (var test in testdata2)
            {
                if (SolveExpressionPrecedence(test.Key) != test.Value) Console.WriteLine($"Error, {test.Key} != {test.Value}");
            }
        }

        public static long SolveExpressionPrecedence(string expression)
        {
            var exprNode = ExpressionNode.Build("(" + expression.Replace(" ", "") + ")");
            return exprNode.Evaluate();
        }

        static void Main(string[] args)
        {
            Test();
            long part1 = 0;
            long part2 = 0;
            Performance.TimeRun("Read and solve", () => {
                using (StreamReader sr = File.OpenText("input.txt"))
                {
                    string s = String.Empty;
                    int pos = 0;
                    while ((s = sr.ReadLine()) != null)
                    {
                        pos = 0;
                        part1 += SolveExpressionLeftToRight(s, ref pos);
                        part2 += SolveExpressionPrecedence(s);
                    }
                }
            },10,1000);

            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }

        static long SolveExpressionLeftToRight(string expression, ref int pos)
        {
            long value = 0;
            char operation = '+';
            long operand = 0;
            while(pos < expression.Length)
            {
                char c = expression[pos++];
                if (c >= '0' && c <= '9')
                {
                    operand = c - '0';
                }
                else if (c == '+' || c == '*')
                {
                    operation = c;
                    pos++;
                }
                else if (c == ' ')
                {
                    value = PerformOperation(value, operand, operation);
                    operand = 0;
                }
                else if (c == '(')
                {
                    operand = SolveExpressionLeftToRight(expression, ref pos);
                }
                else if (c == ')')
                {
                    value = PerformOperation(value, operand, operation);
                    operand = 0;
                    return value;
                }
            }
            value = PerformOperation(value, operand, operation);
            operand = 0;
            return value;
        }

        static long PerformOperation(long opa, long opb, char operation)
        {
            switch (operation)
            {
                case '*': return opa * opb;
                case '+': return opa + opb;
                default: throw new ArgumentException();
            }
        }
    }
}
