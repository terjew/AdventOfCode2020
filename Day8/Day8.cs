using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day8
{
    public abstract class Instruction
    {
        public int Argument;
        protected Instruction(int arg)
        {
            Argument = arg;
        }

        public abstract void Execute(ref int pc, ref int acc);
    }

    public class Nop : Instruction
    {
        public Nop(int arg) : base(arg) { }
        public override void Execute(ref int pc, ref int acc)
        {
            pc++;
        }
    }

    public class Acc : Instruction
    {
        public Acc(int arg) : base(arg) { }
        public override void Execute(ref int pc, ref int acc)
        {
            acc += Argument;
            pc++;
        }
    }

    public class Jmp : Instruction
    {
        public Jmp(int arg) : base(arg) { }
        public override void Execute(ref int pc, ref int acc)
        {
            pc += Argument;
        }
    }

    public static class InstructionFactory
    {
        private static Dictionary<string, Type> Types = new Dictionary<string, Type>
        {
            {"nop", typeof(Nop) },
            {"acc", typeof(Acc) },
            {"jmp", typeof(Jmp) },
        };

        public static Instruction Parse(string s)
        {
            var pair = s.Split(" ");
            int arg = int.Parse(pair[1]);
            var opc = pair[0];
            var type = Types[opc];
            var constructor = type.GetConstructor(new[] { typeof(int) });
            return (Instruction)constructor.Invoke(new object[] { arg });
        }

        public static Instruction Flip(Instruction i)
        {
            switch(i)
            {
                case Nop n: return new Jmp(n.Argument);
                case Jmp j: return new Nop(j.Argument);
                default: return i;
            }
        }
    }

    class Machine
    {
        public List<Instruction> Program { get; }
        public int AC;
        public int PC;

        public Machine(List<string> program)
        {
            Program = program.Select(l => InstructionFactory.Parse(l)).ToList();
        }

        public void Step()
        {
            Program[PC].Execute(ref PC, ref AC);
        }

        public HashSet<int> RunFrom(int start)
        {
            var visited = new HashSet<int>();
            PC = start;
            while (PC < Program.Count && !visited.Contains(PC))
            {
                visited.Add(PC);
                Step();
            }
            visited.Add(PC); //reached end or outside
            return visited;
        }
    }

    class Day8
    {
        static int NextIfFlipped(Machine m)
        {
            var instr = m.Program[m.PC];
            var flipped = InstructionFactory.Flip(instr);
            int pc = m.PC;
            int ac = 0;
            flipped.Execute(ref pc, ref ac);
            return pc;
        }

        static void Main(string[] args)
        {
            int acWithoutFlip = 0;
            int acAfterFlip = 0;

            Performance.TimeRun("Read and solve", () =>
            {
                var machine = new Machine(TextFile.ReadStringList("input.txt"));
                var visited = machine.RunFrom(0);
                acWithoutFlip = machine.AC;

                var target = machine.Program.Count;
                HashSet<int> knownGood = new HashSet<int>();
                HashSet<int> knownBad = new HashSet<int>(visited);
                for (int i = 1; i < machine.Program.Count; i++)
                {
                    if (knownBad.Contains(i) || knownGood.Contains(i))//this instruction has already been visited in a previous path
                    {
                        continue;
                    }
                    visited = machine.RunFrom(i);
                    if (visited.Contains(target)) knownGood.UnionWith(visited);
                    else knownBad.UnionWith(visited);
                }

                machine.AC = 0;
                machine.PC = 0;
                bool flipped = false;

                while (machine.PC < machine.Program.Count)
                {
                    if (!flipped)
                    {
                        var candidate = NextIfFlipped(machine);
                        if (knownGood.Contains(candidate)) //flipping this instructions brings us to a "winning" path
                        {
                            var curr = machine.Program[machine.PC];
                            var flippedInstruction = InstructionFactory.Flip(curr);
                            machine.Program[machine.PC] = flippedInstruction;
                            flipped = true;
                        }
                    }
                    machine.Step();
                }
                acAfterFlip = machine.AC;
            }, 100, 100);

            Console.WriteLine($"AC before flip: {acWithoutFlip}");
            Console.WriteLine($"AC after flip: {acAfterFlip}");
        }
    }
}
