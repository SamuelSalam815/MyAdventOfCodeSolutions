using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13;

internal class PuzzlePacketData : IComparable
{
        // Exactly one of list and value should be null
        // Call instance a value packet when storing a value
        // Call instance a list packet when storiny a list
        List<PuzzlePacketData>? list;
        int? value;
        public PuzzlePacketData(int value)
        {
                list = null;
                this.value = value;
        }

        public PuzzlePacketData()
        {
                list = new List<PuzzlePacketData>();
                value = null;
        }

        public static PuzzlePacketData Parse(string stringRepresentation = null)
        {
                Stack<PuzzlePacketData> referenceStack = new();
                PuzzlePacketData currentList = null;

                for(int charIndex = 0; charIndex < stringRepresentation.Length - 1; charIndex++)
                {
                        switch (stringRepresentation[charIndex])
                        {
                                case ',':
                                        continue;
                                case '[':
                                        PuzzlePacketData newList = new();
                                        if (currentList is not null)
                                        {
                                                currentList.Add(newList);
                                                referenceStack.Push(currentList);
                                        }
                                        currentList = newList;
                                        break;
                                case ']':
                                        currentList = referenceStack.Pop();
                                        break;
                                default:
                                        int startingIndex = charIndex;
                                        while ( '0' <= stringRepresentation[charIndex+1] && stringRepresentation[charIndex+1] <= '9')
                                        {
                                                charIndex++;
                                        }
                                        string substring = stringRepresentation.Substring(startingIndex, charIndex - startingIndex + 1);
                                        PuzzlePacketData intPacket = new(int.Parse(substring));
                                        currentList.Add(intPacket);
                                        break;
                        }
                }

                return currentList;
        }

        public void Add(PuzzlePacketData item)
        {
                if (list is null)
                {
                        throw new InvalidOperationException("Cannot add item to packet data representing a value");
                }

                list?.Add(item);
        }

        public override string ToString()
        {
                if(value is not null)
                {
                        return value.ToString();
                }
                else
                {
                        return "[" + string.Join(",", list) + "]";
                }
        }

        public int CompareTo(object? obj)
        {
                PuzzlePacketData nextPacket = obj as PuzzlePacketData;
                if(nextPacket is null)
                {
                        throw new ArgumentException($"Cannot compare with non-{nameof(PuzzlePacketData)} objects");
                }

                // Both packets are values
                if(value is not null && nextPacket.value is not null)
                {
                        return (int)(value?.CompareTo(nextPacket.value));
                }

                // Both packets are lists
                if(list is not null && nextPacket.list is not null)
                {
                        int maxIndex = Math.Min(list.Count, nextPacket.list.Count);
                        
                        // Compare element-wise on lists
                        for(int packetIndex = 0; packetIndex < maxIndex; packetIndex++)
                        {
                                int comparisonResult = list[packetIndex].CompareTo(nextPacket.list[packetIndex]);
                                if(comparisonResult != 0)
                                {
                                        return comparisonResult;
                                }
                        }

                        // If inconclusive, break on list size
                        return list.Count.CompareTo(nextPacket.list.Count);
                }

                // One packet is a value packet while the other is a list packet
                if(value is not null)
                {
                        // The current instance is a value packet
                        PuzzlePacketData dummy = new();
                        dummy.Add(this);
                        return dummy.CompareTo(nextPacket);
                }
                else
                {
                        // The argument is a value packet
                        PuzzlePacketData dummy = new();
                        dummy.Add(nextPacket);
                        return CompareTo(dummy);
                }

        }
}
