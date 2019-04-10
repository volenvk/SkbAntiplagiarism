using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];
         
            for (var i = 1; i < first.Count + 1; i++)
                for (var j = 1; j < second.Count + 1; j++)
                {
                    if (first.Count == 0 || second.Count == 0)
                        opt[i, j] = 0;
                    if (first[i - 1] == second[j - 1])
                        opt[i, j] = opt[i - 1, j - 1] + 1;
                    else
                        opt[i, j] = Math.Max(opt[i - 1, j], opt[i, j - 1]);
                }

            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var result = new List<string>();
            var i = first.Count;
            var j = second.Count;
            while (i > 0 && j > 0)
            {
                if (first[i - 1] == second[j - 1])
                {
                    result.Add(first[i - 1]);
                    i -= 1;
                    j -= 1;
                }
                else if (opt[i - 1,j] == opt[i,j])
                    i -= 1;
                else
                    j -= 1;
            }

            result.Reverse();
            return result;
        }
    }
}