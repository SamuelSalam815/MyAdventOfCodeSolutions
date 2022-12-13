using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    internal class CPUSimulation
    {
        private int register;

        // one item per cycle
        // the fifth item is the value of the register after 5 cycles
        readonly List<int> historyOfRegisterValues;
        public CPUSimulation()
        {
            register = 1;
            historyOfRegisterValues = new List<int>() { register };
        }

        public void ExecuteInstruction(CPUInstructionType instruction, params int[] arguments)
        {
            if(instruction == CPUInstructionType.noop)
            {
                historyOfRegisterValues.Add(register);
                return;
            }

            if(instruction != CPUInstructionType.addx)
            {
                throw new Exception("Unknown instruction");
            }

            if(arguments.Length != 1)
            {
                throw new Exception($"Unexpected number of arguments for instruction {CPUInstructionType.addx}");
            }

            historyOfRegisterValues.Add(register);
            register += arguments[0];
            historyOfRegisterValues.Add(register);

        }

        // the fifth item is the value of the register AFTER 5 cycles
        public List<int> GetRegisterHistory() => new(historyOfRegisterValues);
    }
}
