using System;
using System.Linq;
using Utilities;

namespace Day5
{
    class Day5
    {
        static void Main(string[] args)
        {
            int maxSeatId = 0;
            int mySeatId = 0;
            Performance.TimeRun("foo", () =>
            {
                var lines = TextFile.ReadStringList("input.txt");
                var seatIds = lines.Select(l => ParseSeatId(l));
                var ordered = seatIds.OrderBy(s => s).ToArray();
                maxSeatId = ordered[ordered.Length - 1];
                int last = ordered[0];
                int id = 0;
                for (int i = 0; i < ordered.Length; i++)
                {
                    id = ordered[i];
                    if (id == last + 2) break;
                    last = id;
                }
                mySeatId = id;
            });
            Console.WriteLine(maxSeatId);
            Console.WriteLine(mySeatId);
        }

        static int ParseSeatId(string bsp)
        {
            byte row = 0;
            byte column = 0;
            for (int i = 0; i < 7; i++)
            {
                if (bsp[i] == 'B') row += (byte)(1 << (6 - i));
            }
            for (int i = 7; i < 10; i++)
            {
                if (bsp[i] == 'R') column += (byte)(1 << (9 - i));
            }
            return row * 8 + column;
        }
    }
}
