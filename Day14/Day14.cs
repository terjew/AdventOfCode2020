using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Day14
{
    class BitMaskComputer
    {
        public Dictionary<long, long> Memory = new Dictionary<long, long>();
        public long AndMask = 0;
        public long OrMask = 0;
        List<long> FloatingBitCombinations = new List<long>() { 0 };
        public int Version;

        public long RunProgram(List<string> program, int version = 1)
        {
            Version = version;
            Memory = new Dictionary<long, long>();
            foreach(var line in program)
            {
                var instruction = line.Split(new[] { " = ", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                switch (instruction[0])
                {
                    case "mask":
                        SetMask(instruction[1]);
                        break;
                    case "mem":
                        var address = long.Parse(instruction[1]);
                        var value = long.Parse(instruction[2]);
                        SetMem(address, value);
                        break;
                }
            }
            return Memory.Sum(m => m.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long ApplyMask(long num)
        {
            num |= OrMask;
            return num &= AndMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetMem(long address, long value)
        {
            if (Version == 1)
            {
                Memory[address] = ApplyMask(value);
            }
            else
            {
                address = ApplyMask(address);
                foreach (var offset in FloatingBitCombinations)
                {
                    var floatingAddress = address + offset;
                    Memory[floatingAddress] = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetMask(string mask)
        {
            OrMask = 0;
            AndMask = 0;
            List<long> floatingBits = new List<long>();
            FloatingBitCombinations = new List<long>() { 0 };
            long value = 0b100000000000000000000000000000000000;
            foreach(char c in mask)
            {
                if (Version == 1)
                {
                    AndMask = (AndMask << 1) + ((c == 'X' || c == '1') ? 1 : 0);
                    OrMask = (OrMask << 1) + ((c == 'X' || c == '0') ? 0 : 1);
                }
                else
                {
                    OrMask = (OrMask << 1) + ((c == '1') ? 1 : 0);
                    AndMask = (AndMask << 1) + ((c == 'X') ? 0 : 1);
                    if (c == 'X')
                    {
                        floatingBits.Add(value);
                        int num = FloatingBitCombinations.Count;
                        for (int i = 0; i < num; i++) FloatingBitCombinations.Add(FloatingBitCombinations[i] + value);
                    }
                    value >>= 1;
                }
            }
        }
    }

    class Day14
    {
        static void Main(string[] args)
        {
            long part1 = 0;
            long part2 = 0;
            Performance.TimeRun("part 1 and 2", () =>
            {
                var sample = TextFile.ReadStringList("input.txt");
                var computer = new BitMaskComputer();
                part1 = computer.RunProgram(sample, 1);
                part2 = computer.RunProgram(sample, 2);
            });
            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
