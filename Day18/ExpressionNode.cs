using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{

    class ExpressionNode
    {
        private ExpressionNode left;
        private ExpressionNode right;
        private char data;

        public long Evaluate()
        {
            if (char.IsDigit(data)) return data - '0';
            else if (data == '+') return left.Evaluate() + right.Evaluate();
            else if (data == '*') return left.Evaluate() * right.Evaluate();
            throw new InvalidOperationException();
        }

        public ExpressionNode(char value)
        {
            data = value;
        }

        public static ExpressionNode Build(string s)
        {
            Stack<ExpressionNode> nodeStack = new Stack<ExpressionNode>();
            Stack<char> charStack = new Stack<char>();

            // Prioritising the operators
            int[] p = new int[255];
            p['*'] = 1;
            p['+'] = 2;
            p[')'] = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    // Push '(' in char stack
                    charStack.Push(s[i]);
                }
                // Push the operands in node stack
                else if (char.IsDigit(s[i]))
                {
                    nodeStack.Push(new ExpressionNode(s[i]));
                }
                else if (p[s[i]] > 0)
                {
                    // If an operator with lower or
                    // same associativity appears
                    while (charStack.Count != 0 && charStack.Peek() != '(' && (p[charStack.Peek()] >= p[s[i]]))
                    {
                        var n = new ExpressionNode(charStack.Pop());
                        n.right = nodeStack.Pop();
                        n.left = nodeStack.Pop();
                        nodeStack.Push(n);
                    }
                    charStack.Push(s[i]);
                }
                else if (s[i] == ')')
                {
                    while (charStack.Count != 0 && charStack.Peek() != '(')
                    {
                        var n = new ExpressionNode(charStack.Pop());
                        n.right = nodeStack.Pop();
                        n.left = nodeStack.Pop();
                        nodeStack.Push(n);
                    }
                    charStack.Pop();
                }
            }
            return nodeStack.Peek();
        }

    }
}
