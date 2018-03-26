using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema01
{
    public class ChangeString
    {
        static void Main(string[] args)
        {
            string entrada;
            entrada= Console.ReadLine();            
            Console.WriteLine(build(entrada));
            Console.WriteLine("Presiona enter para cerrar ...");
            Console.ReadLine();
        }
        public static string build(string entrada)
        {
            string salida="";
            char car;
            foreach (char c in entrada)
            {
                car = c;
                bool esletra = char.IsLetter(c);
                if (esletra)
                {
                    if (c.Equals('ñ'))
                    {
                        salida = salida + "o";
                    }
                    else if(c.Equals('Ñ'))
                    {
                        salida = salida + "O";
                    }
                    else if (c.Equals('n'))
                    {
                        salida = salida + "ñ";
                    }
                    else if (c.Equals('N'))
                    {
                        salida = salida + "Ñ";
                    }
                    else if (c.Equals('z'))
                    {
                        salida = salida + " ";
                    }
                    else if (c.Equals('Z'))
                    {
                        salida = salida + " ";
                    }
                    else
                    {
                        car++;
                        salida = salida + car.ToString();
                    }
                }
                else
                {
                    salida = salida + c.ToString();
                }
            }
            return salida;
        }
    }
}
