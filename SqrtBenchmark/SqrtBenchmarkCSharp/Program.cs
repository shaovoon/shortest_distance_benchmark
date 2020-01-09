using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SqrtBenchmarkCSharp
{
    class Point
    {
        public double x;
        public double y;
    }

    class Program
    {
        static Random random = new Random();

        static public double GetRand()
        {
            return random.NextDouble() * 1000.0;
        }
        static void DisplayElapseTime(string title, TimeSpan ts)
        {
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0}{1:000}ms",
                ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine(title + " timing: " + elapsedTime);
        }

        static void Main(string[] args)
        {
            const int MAX_LOOP = 1000000;
            Point dest = new Point();
            dest.x = GetRand();
            dest.y = GetRand();

            List<Point> list = new List<Point>();
            for (int i = 0; i < 1000; ++i)
            {
                Point pt = new Point();
                pt.x = GetRand();
                pt.y = GetRand();
                list.Add(pt);
            }

            double shortest = 10000000.0;
            int shortest_index = 0;
            double shortest2 = 10000000.0;
            int shortest_index2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < MAX_LOOP; ++i)
            {
                shortest = 10000000.0;
                shortest_index = 0;

                for (int j = 0; j < list.Count; ++j)
                {
                    Point pt = list[j];
                    double x = (pt.x - dest.x);
                    double y = (pt.y - dest.y);
                    x *= x;
                    y *= y;
                    double distance = Math.Sqrt(x + y);
                    if (distance < shortest)
                    {
                        shortest = distance;
                        shortest_index = j;
                    }
                }
            }
            stopWatch.Stop();
            DisplayElapseTime("With sqrt", stopWatch.Elapsed);

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < MAX_LOOP; ++i)
            {
                shortest2 = 10000000.0;
                shortest_index2 = 0;

                for (int j = 0; j < list.Count; ++j)
                {
                    Point pt = list[j];
                    double x = (pt.x - dest.x);
                    double y = (pt.y - dest.y);
                    x *= x;
                    y *= y;
                    double distance = x + y;
                    if (distance < shortest2)
                    {
                        shortest2 = distance;
                        shortest_index2 = j;
                    }
                }
                shortest2 = Math.Sqrt(shortest2);
            }
            stopWatch.Stop();
            DisplayElapseTime("Without sqrt", stopWatch.Elapsed);


            Console.WriteLine("shortest: {0}, {1}", shortest, shortest2);
            Console.WriteLine("shortest_index: {0}, {1}", shortest_index, shortest_index2);


        }
    }
}
