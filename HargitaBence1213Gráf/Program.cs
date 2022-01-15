using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

namespace HargitaBence1213Gráf
{
    class Program
    {
        public static Stopwatch timer = new Stopwatch();

        #region take1
        public class Node
        {
            public int HanyszorVoltunkRajta { get; set; } = 0;
            public List<int> Connections { get; set; } = new List<int>();

            public Node()
            {

            }
            public Node(int a, List<int> b)
            {
                this.HanyszorVoltunkRajta = a;
                this.Connections = b;
            }
        }

        public static int rekurzivLeghosszabbUt(List<Node> grafOriginal, int akthely, int cel, int megtettUt)
        {
            List<Node> graf = grafCopy(grafOriginal);
            if (graf[akthely].HanyszorVoltunkRajta < graf[akthely].Connections.Count + 1)
            {
                if (graf[akthely].HanyszorVoltunkRajta == 0)
                    ++megtettUt;
                ++graf[akthely].HanyszorVoltunkRajta;

                List<int> megoldasok = new List<int>();

                if (akthely == cel)
                    megoldasok.Add(megtettUt);

                foreach (var item in graf[akthely].Connections)
                {
                    megoldasok.Add(rekurzivLeghosszabbUt(graf, item, cel, megtettUt));
                }

                megoldasok.Sort();
                return megoldasok.Count == 0 ? 0 : megoldasok[megoldasok.Count - 1]; //max
            }
            else
                return 0;
        }

        public static List<Node> grafCopy(List<Node> graf)
        {
            List<Node> temp = new List<Node>();
            foreach (var item in graf)
                temp.Add(new Node(item.HanyszorVoltunkRajta, new List<int>(item.Connections)));
            return temp;
        }

        #endregion

        #region take2

        public static int secondSolution(List<List<int>> graf, int cel, List<int> voltmar, Queue<int> toDoList)
        {
            List<int> masikUtvonalakMegoldasai = new List<int>();

            while(toDoList.Count != 0)
            {
                int task = toDoList.Peek();
                toDoList.Dequeue();

                if (elerhetoCel(graf, task, cel))
                {
                    if (voltmar.Count == 0 || elerhetoCel(graf, voltmar[voltmar.Count - 1], task))//ha elérhető az előzőleg hozzáadott pontból akkor ugyanazon útvonal tagja
                        voltmar.Add(task);
                    else
                    {
                        List<int> voltmarCopy = new List<int>(voltmar);
                        voltmarCopy.RemoveAt(voltmarCopy.Count - 1); // az utolsót kiszedjük

                        Queue<int> toDoListCopy = new Queue<int>();
                        toDoListCopy.Enqueue(task);//hogy újra megnézzük, hátha csak egy új útvonal
                        foreach (var item in toDoList)
                            toDoListCopy.Enqueue(item);//sorrendben viszsarakjuk

                        masikUtvonalakMegoldasai.Add(secondSolution(graf, cel, voltmarCopy, toDoListCopy)); // nem tudom hogy kell e másolni vagy magától másolja
                    }
                }


                foreach (var item in graf[task])
                    if (!toDoList.Contains(item) && !voltmar.Contains(item))
                    {
                        toDoList.Enqueue(item);
                    }
            }

            /*
            Console.WriteLine("Útvonal:");
            foreach (var item in voltmar)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();*/
            masikUtvonalakMegoldasai.Add(voltmar.Count);
            masikUtvonalakMegoldasai.Sort();

            return masikUtvonalakMegoldasai[masikUtvonalakMegoldasai.Count-1];
        }

        public static bool elerhetoCel(List<List<int>> graf, int kezdo, int cel)
        {
            bool solution = false;
            List<int> voltmar = new List<int>();

            Queue<int> toDoList = new Queue<int>();
            toDoList.Enqueue(kezdo);

            while (toDoList.Count != 0)
            {
                int task = toDoList.Peek();
                toDoList.Dequeue();

                if (task == cel)
                {
                    solution = true;
                    break;
                }
                else
                    foreach (var item in graf[task])
                        if (!voltmar.Contains(item))
                        {
                            toDoList.Enqueue(item);
                            voltmar.Add(item);
                        }
            }

            return solution;
        }
        #endregion

        static void Main(string[] args)
        {
            bool runSecondSolution = true;


            int N, M, kezdes, vegzes;
            int[] sInt = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
            N = sInt[0];
            M = sInt[1];
            kezdes = sInt[2];
            vegzes = sInt[3];

            #region take1
            if (!runSecondSolution)
            {
                timer.Start();
                List<Node> graf = new List<Node>();
                for (int i = 0; i < N + 1; i++) //nem lesz 0. !!!!!!!!!!!!!!!!!!!
                    graf.Add(new Node());

                for (int i = 0; i < M; i++)
                {
                    sInt = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                    if (!graf[sInt[0]].Connections.Contains(sInt[1]))
                        graf[sInt[0]].Connections.Add(sInt[1]);
                }


                //hogy szép legyen:

                Console.WriteLine();
                Console.WriteLine("A gráf:");
                for (int i = 0; i < graf.Count; i++)
                {
                    Console.Write(i + " ");
                    foreach (var item in graf[i].Connections)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Megoldás:");

                Console.WriteLine(rekurzivLeghosszabbUt(graf, kezdes, vegzes, 0));

                Console.WriteLine("ennyi időbe telt:");
                Console.WriteLine(timer.Elapsed);
                //ki lehet zárni a nem cél' illetve a 0 connections.count os node okat
            }
            #endregion

            #region take2
            if (runSecondSolution)
            {
                timer.Start();
                List<List<int>> graf = new List<List<int>>();
                for (int i = 0; i < N + 1; i++) //nem lesz 0. !!!!!!!!!!!!!!!!!!!
                    graf.Add(new List<int>());

                for (int i = 0; i < M; i++)
                {
                    sInt = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                    if (!graf[sInt[0]].Contains(sInt[1]))
                        graf[sInt[0]].Add(sInt[1]);
                }

                Queue<int> toDoList = new Queue<int>();
                toDoList.Enqueue(kezdes);
                Console.WriteLine(secondSolution(graf,vegzes, new List<int>(),toDoList));

                Console.WriteLine("ennyi időbe telt:");
                Console.WriteLine(timer.Elapsed);
            }
            #endregion
        }
    }
}
