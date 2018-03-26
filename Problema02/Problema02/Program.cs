using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema02
{
    public class OrderRange
    {
        static void Main(string[] args)
        {
            string valor;
            int num;
            List<int> pares = new List<int>();
            List<int> impares = new List<int>();
            do
            {
                valor = Console.ReadLine();
                bool esNumero = int.TryParse(valor,out num);
                if (esNumero)
                {                   
                    if(num>=0)
                    {
                        if ((num%2)==0)
                        {
                            pares.Add(num);
                        }
                        else
                        {
                            impares.Add(num);
                        }
                    }
                    else
                    {
                        break;    
                    }
                }
                else
                {
                    break;
                }
            } while (valor!="");
            build(pares,impares);
            Console.WriteLine("Presiona enter para cerrar ...");
            Console.ReadLine();
        }
        public static void build(List<int> pares, List<int> impares)
        {
            pares.Sort();
            impares.Sort();
            Console.WriteLine(Environment.NewLine + "Pares");
            foreach (var n in pares)
                Console.WriteLine(n);
            Console.WriteLine(Environment.NewLine + "Impares");
            foreach (var n in impares)
                Console.WriteLine(n);
        }
    }
}
