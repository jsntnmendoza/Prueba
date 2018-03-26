using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema03
{
    public class MoneyParts
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese valor: ");
            string str = Console.ReadLine();
            double target= double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);            
            double[] set = { 0.05, 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100, 200 };
            List<double> ent = new List<double>();
            foreach (var s in set)
            {
                for (int i = 0; i < Math.Truncate(target / s) && s <= target; i++)
                {
                    ent.Add(s);
                }
            }
            
            foreach (var item in build(ent, target))
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        public static List<string> build(List<double> list,double target)
        {
            List<string> salida = new List<string>();
            List<string> salida2= new List<string>();
            string sal = "";
            string nsal = "";
            decimal sum = 0;
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                sum = Convert.ToDecimal(target);
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                for (int j = 0; j < str.Length; j++)
                {                    
                    if (str[j] == '1')
                    {
                        sum = sum - (decimal)list[j];
                        sal = sal + list[j] + " ";
                    }
                }
                if(sum==0)
                {
                    nsal=sal.Replace(",",".");
                    salida.Add(nsal);
                }
                sal = "";
                nsal = "";
            }
            salida2 = salida.Distinct().OrderByDescending(x=>x.Length).ToList();
            return salida2;
        }

    }
}
