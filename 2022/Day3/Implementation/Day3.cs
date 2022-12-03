﻿using System.Collections;

public class Day3
{
    private static int ToPriority(char item) => item switch
    {
        (>= 'a' and <= 'z') => item - 'a' + 1,
        (>= 'A' and <= 'Z') => item - 'A' + 27,
        _ => throw new Exception($"Char value '{item}' has no defined priority")
    };

    private static char FromPriority(int priority) => priority switch
    {
        (>= 1 and <= 26) => (char)('a' + priority - 1),
        (>= 27 and <= 52) => (char)('A' + priority - 27),
        _ => throw new Exception($"Priority value '{priority}' has no defined character")
    };

    // Helper method for debugging
    private static string GetMaskedCharacters(BitArray mask)
    {
        string result = "";
        for(int i = 0; i < 52; i++)
        {
            if (mask.Get(i))
            {
                result += FromPriority(i+1);
            }
        }

        return result;
    }

    public static char FindRucksackOutlier(string rucksack)
    {
        // Use bits to indicate the presence of an item in a compartment
        // 1st bit is 'a', 2nd bit is 'b' ...
        BitArray firstCompartmentMask = new(52);
        BitArray secondCompartmentMask = new(52);

        // Place all items into their comparments
        for(int i = 0; i < rucksack.Length; i++)
        {
            if(i+1 < rucksack.Length / 2)
            {
                firstCompartmentMask.Set(ToPriority(rucksack[i]) - 1, true);
            }
            else
            {
                secondCompartmentMask.Set(ToPriority(rucksack[i]) - 1, true);
            }
        }
        
        // Take the intersection of the types of items present in each compartment
        // Exactly one will be in both components. Return this character
        BitArray overlappingItems = new(firstCompartmentMask);
        overlappingItems.And(secondCompartmentMask);
            
        for(int i = 0; i < overlappingItems.Length; i++)
        {
            if (overlappingItems.Get(i))
            {
                return FromPriority(i + 1);
            }
        }

        throw new Exception("Unexpected failure to find outlier");
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

