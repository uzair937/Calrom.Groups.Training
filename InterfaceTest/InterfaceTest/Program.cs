using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public interface ITCalculator
    {
        void displayAnswer();
    }

    class Calculator : ITCalculator
    {
        private int num1;
        private int num2;
        private string operand;

        public Calculator()
        {
            num1 = 0;
            num2 = 0;
            operand = null;
        }

        public Calculator(int num1, string operand, int num2)
        {
            this.num1 = num1;
            this.operand = operand;
            this.num2 = num2;
        }

        public double getAnswer()
        {
            if (operand == "*")
            {
                return num1 * num2;
            }
            else if (operand == "/")
            {
                return num1 / num2;
            }
            else if (operand == "+")
            {
                return num1 + num2;
            }
            else if (operand == "-")
            {
                return num1 - num2;
            }
            else
            {
                return 0;
            }
        }

        public void displayAnswer()
        {
            Console.WriteLine("Answer: {0}", getAnswer());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the first number, followed by the enter key, an operand (* / + -) and then enter followed by the final number.");
            Calculator calc = new Calculator(Convert.ToInt32(Console.ReadLine()), Console.ReadLine(), Convert.ToInt32(Console.ReadLine()));
            calc.displayAnswer();
            Console.ReadKey();
        }
    }
}