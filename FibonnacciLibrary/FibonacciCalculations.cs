﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonnacciLibrary
{
    public class FibonacciCalculations
    {

        /// <summary>
        /// Fibonacci sequence goes 0, 1, 1, 2, 3, 5, 8, etc.
        /// Return number of Fibonacci elements requested 
        /// </summary>
        /// <param name="numberOfElements"></param>
        /// <returns></returns>
        public static IEnumerable<uint> ListOfFibs(uint numberOfElements)
        {
            List<uint> lst = new List<uint>();
            for (int i = 0; i < numberOfElements; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                        lst.Add((uint)i);
                        break;
                    default:

                        lst.Add(lst[i - 1] + lst[i - 2]);
                        break;

                }
            }
            return lst;
        }

        /// <summary>
        /// Would NOT expect this as interview answer
        /// </summary>
        /// <param name="numberOfElements"></param>
        /// <returns></returns>
        public static IEnumerable<uint> GenerateFibonacciNumbers(uint numberOfElements)
        {
            for (uint i = 0, j = 0, k = 1; i < numberOfElements; i++)
            {
                yield return j;
                uint temp = j + k;
                j = k;
                k = temp;
            }
        }

        /// <summary>
        /// Notice that this could be an extremely dangerous method!
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<uint> GenerateInfiniteFibonacciNumbers()
        {
            uint i = 0, j = 0, k = 1;
            for (; ; )
            {
                yield return j;
                uint temp = j + k;
                j = k;
                k = temp;
            }
        }

        /// <summary>
        /// Uses recursion ... very naively. Notice that a lot of work is repeated.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ulong JustOneFib(ulong index)
        {
            switch (index)
            {
                case 0:
                case 1:
                    return index;
                default:
                    // Can you improve on this?  If these are huge numbers, you're repeating a lot of the work!
                    // You could use a HashTable to store values.
                    return JustOneFib(index - 1) + JustOneFib(index - 2);
            }
        }

        /// <summary>
        /// See for example: https://www.geeksforgeeks.org/tail-recursion-fibonacci/
        /// </summary>
        /// <param name="index"></param>
        /// <param name="accumulator1"></param>
        /// <param name="accumulator2"></param>
        /// <returns></returns>
        private static ulong GetFibUsingIntelligentRecursion(ulong index, ulong accumulator1 = 0, ulong accumulator2 = 1)
        {
            if (index == 0)
                return accumulator1;
            else if (index == 1)
                return accumulator2;
            else
                return GetFibUsingIntelligentRecursion(index - 1, accumulator2, accumulator1 + accumulator2);
        }

        public static ulong GetFibUsingIntelligentRecursionHelper(ulong index)
        {
            return GetFibUsingIntelligentRecursion(index);
        }

        public static ulong JustOneFibSimpleLoop(ulong index)
        {
            ulong answer = 0;
            ulong previousAnswer = 0;
            for (ulong i = 0; i <= index; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                        answer = i;
                        break;
                    default:
                        ulong temp = answer;
                        answer = answer + previousAnswer;
                        previousAnswer = temp;
                        break;

                }
            }
            return answer;
        }
        public static ulong JustOneFibFaster(ulong index)
        {
            IList<ulong> lst = GenerateFibonacciNumbersRecursion1(index + 1);
            return lst[(int)index];
        }

        // Add some recursive methods to find Fibonnacci.  Try "blind" or "bad" recursion, and then tail-recursion.
        // For tail-recursion ideas see:
        // https://stackoverflow.com/questions/33923/what-is-tail-recursion
        // https://stackoverflow.com/questions/22111252/tail-recursion-fibonacci



        /// <summary>

        /// </summary>
        /// Originally, this was intended to be a "naive" implementation of recursion, 
        /// but in fact it's pretty efficient!
        /// <param name="numberOfElements"></param>
        /// <returns></returns>
        public static IList<ulong> GenerateFibonacciNumbersRecursion1(ulong numberOfElements)
        {
            //List<uint> lst = new List<uint>();
            if (numberOfElements == 1)
            {
                return new List<ulong> { 0 };

            }
            else if (numberOfElements == 2)
            {
                IList<ulong> tmp = GenerateFibonacciNumbersRecursion1(1);
                tmp.Add(1);
                return tmp;
            }
            else
            {
                IList<ulong> previousLst = GenerateFibonacciNumbersRecursion1(numberOfElements - 1);
                previousLst.Add(previousLst[previousLst.Count - 1] + previousLst[previousLst.Count - 2]);
                return previousLst;
            }

        }

        public static void GenerateFibonacciNumbersRecursion2(uint numberOfElments, ref IList<uint> lst)
        {
            if (numberOfElments == 1)
            {
                lst.Add(0);               
            }
            else if (numberOfElments == 2)
            {
                GenerateFibonacciNumbersRecursion2(numberOfElments - 1, ref lst);
                lst.Add(1);                
            }
            else
            {
                GenerateFibonacciNumbersRecursion2(numberOfElments - 1, ref lst);
                lst.Add(lst[lst.Count - 1] + lst[lst.Count - 2]);               
            }
        }

    }
}
