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

            double shortest = double.MaxValue;
            int shortest_index = 0;
            double shortest2 = double.MaxValue;
            int shortest_index2 = 0;
            double shortest3 = double.MaxValue;
            double shortest_abs = double.MaxValue;
            int shortest_index3 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < MAX_LOOP; ++i)
            {
                shortest = double.MaxValue;
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
                shortest2 = double.MaxValue;
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

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < MAX_LOOP; ++i)
            {
                shortest3 = double.MaxValue;
                shortest_index3 = 0;

                for (int j = 0; j < list.Count; ++j)
                {
                    Point pt = list[j];
                    double x = Math.Abs(pt.x - dest.x); // N.B.: abs!
                    if (x > shortest_abs) continue; // bail out

                    double y = Math.Abs(pt.y - dest.y);
                    if (y > shortest_abs) continue;

                    double xq = x * x;
                    double yq = y * y;
                    double distance = xq + yq;
                    if (distance < shortest3)
                    {
                        shortest3 = distance;
                        shortest_abs = x + y;
                        shortest_index3 = j;
                    }
                }
                shortest3 = Math.Sqrt(shortest3);
            }
            stopWatch.Stop();
            DisplayElapseTime("Bait out before square", stopWatch.Elapsed);

            Console.WriteLine("shortest: {0}, {1}, {2}", shortest, shortest2, shortest3);
            Console.WriteLine("shortest_index: {0}, {1}, {2}", shortest_index, shortest_index2, shortest_index3);


        }
    }
}
