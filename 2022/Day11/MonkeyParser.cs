using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    internal class MonkeyParser
    {
        public static Monkey Parse(StreamReader input)
        {
            input.ReadLine(); // Skip first line

            // Parse starting items
            string line = input.ReadLine();

            string startingItemsAsString = line["  Starting items:".Length..];
            
            List<long> startingItems = startingItemsAsString.Split(',').Select(x => long.Parse(x)).ToList();


            // Parse inspection operation
            line = input.ReadLine();
            string operationBody = line.Split('=',2)[1];
            operationBody = new string(operationBody.Where(c => !char.IsWhiteSpace(c)).ToArray());

            string[] unparsedInspectionArguments;

            Monkey.InspectionType inspectionType;

            if (operationBody.Contains('+'))
            {
                inspectionType = Monkey.InspectionType.Add;
                unparsedInspectionArguments = operationBody.Split('+');
            } else
            {
                inspectionType = Monkey.InspectionType.Multiply;
                unparsedInspectionArguments = operationBody.Split('*');
            }

            long? inspectionArgument1;
            long? inspectionArgument2;

            if (long.TryParse(unparsedInspectionArguments[0], out long argument1))
            {
                inspectionArgument1 = argument1;
            }
            else
            {
                inspectionArgument1 = null;
            }
            
            if (long.TryParse(unparsedInspectionArguments[1], out long argument2))
            {
                inspectionArgument2 = argument2;
            }
            else
            {
                inspectionArgument2 = null;
            }

            // Parse throw targeting
            //  Get the divisor
            line = input.ReadLine(); 
            long divisisor = long.Parse(line["  Test: divisible by ".Length..]);

            line = input.ReadLine();
            int firstTarget = int.Parse(line["    If true: throw to monkey ".Length..]); ;

            line = input.ReadLine();
            int secondTarget = int.Parse(line["    If false: throw to monkey ".Length..]); ;

            return new Monkey(
                startingItems,
                inspectionArgument1,
                inspectionArgument2,
                inspectionType,
                divisisor,
                firstTarget,
                secondTarget
                );
        }
    }
}
