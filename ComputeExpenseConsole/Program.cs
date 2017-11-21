using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ComputeExpenseConsole
{
    class Program
    {
        private static List<string> Lines { get; set; }
        private static Dictionary<string, Dictionary<string, List<double>>> TotalCamps { get; set; }
        static void Main(string[] args)
        {
            askAgain:
            var fileName = Console.ReadLine();
            if (!string.IsNullOrEmpty(fileName))
            {
                Lines = new List<string>();
                var isSuccess = ReadInputFile(fileName);
                if (isSuccess)
                {
                    //Console.ReadLine();
                    WriteOutputFile(fileName);
                    Console.ReadLine();
                }
                else
                {
                    goto askAgain;
                }
            }
        }

        private static void WriteOutputFile(string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName + "txt.out");
                foreach (var camp in TotalCamps)
                {
                    var campPeople = camp.Value;
                    double campTotalExpense = 0;
                    foreach (var campTotal in campPeople.Values)
                    {
                        campTotalExpense += campTotal.Sum();
                    }
                    double campAvgExpense = campTotalExpense / campPeople.Count;
                    foreach (var people in campPeople)
                    {
                        double total = Math.Round(campAvgExpense,2) - Math.Round(people.Value.Sum(),2);
                        sw.WriteLine(total > 0 ? "$" + Math.Round(total,2) : "($" + Math.Round(Math.Abs(total), 2) + ")");
                    }
                    sw.WriteLine("");
                }
                sw.Close();
                Console.WriteLine("Output file generated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private static bool ReadInputFile(string fileName)
        {
            try
            {
                String line;
                StreamReader sr = new StreamReader(fileName + ".txt");
                line = sr.ReadLine();
                while (line != "0")
                {
                    //Console.WriteLine(line);
                    Lines.Add(line);
                    line = sr.ReadLine();

                }
                sr.Close();
                
                TotalCamps = new Dictionary<string, Dictionary<string, List<double>>>();
                var firstCamp = Lines.FirstOrDefault();
                if (!string.IsNullOrEmpty(firstCamp))
                {
                    int campPeople;
                    bool isCamp = int.TryParse(firstCamp, out campPeople);
                    var campIndex = 1;
                    var peopleIndex = 1;
                    if (isCamp)
                    {
                        int peopleTotalExpense;
                        for (int i = 1; i < Lines.Count; i += (peopleTotalExpense + 1))
                        {

                            if (campPeople == 0)
                            {
                                bool isInt = int.TryParse(Lines.ElementAt(i), out campPeople);
                                if (isInt)
                                {
                                    campIndex++;
                                    peopleTotalExpense = 0;
                                    continue;
                                }
                            }

                            var peopleList = new List<double>();
                            //var peopleExpense = Lines.ElementAt(i);

                            bool isIntegar = int.TryParse(Lines.ElementAt(i), out peopleTotalExpense);
                            if (isIntegar)
                            { 
                                //find expenses
                                for (int j = 1; j <= peopleTotalExpense; j++)
                                {
                                    double val;
                                    double.TryParse(Lines[i + j], out val);
                                    peopleList.Add(val);
                                }
                            }
                            if (TotalCamps.ContainsKey("Camp " + campIndex))
                            {
                                var peopleDic = TotalCamps["Camp " + campIndex];
                                if (!peopleDic.Keys.Contains("People " + peopleIndex))
                                {
                                    peopleDic.Add("People " + peopleIndex, peopleList);
                                }
                                TotalCamps["Camp " + campIndex] = peopleDic;
                            }
                            else
                            {
                                var peopleDic = new Dictionary<string, List<double>>();
                                peopleDic.Add("People " + peopleIndex, peopleList);
                                TotalCamps.Add("Camp " + campIndex, peopleDic);
                            }
                            peopleIndex++;
                            campPeople--;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }
    }
}
