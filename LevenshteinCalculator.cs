using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            if (documents.Count < 2) return new List<ComparisonResult>();
            
            return new List<ComparisonResult> {
                new ComparisonResult(
                    documents[0], 
                    documents[1], 
                    CalcLevenshteinDistance(documents[0], documents[1]))};
        }
        
        private Double CalcLevenshteinDistance(List<string> a, List<string> b)
        {
            if (a.Count == 0 && b.Count == 0) return 0;
            if (a.Count == 0) return b.Count;
            if (b.Count == 0) return a.Count;

            var max = Math.Max(a.Count, b.Count);
            var  distances = new Double[2, max + 1];
           
            for (var j = 0;  j <= max;  distances[0, j] = j++);          
            
            for (var i = 1;  i <= a.Count;  i++)
                for (var j = 1;  j <= b.Count;  j++)
                {
                    var replace = TokenDistanceCalculator.GetTokenDistance(a[i - 1], b[j - 1]);
                    var replaceCost = b[j - 1] == a[i - 1] ? 0 : replace;
                    
                    distances[1,j] = Math.Min
                    (
                        Math.Min(distances[0, j] + 1, distances[1, j - 1] + 1),
                        distances[0, j - 1] + replaceCost
                    );
                }
            
            var temp = distances[0, j];
            distances[0, j] = distances[1, j];
            distances[1, j] = temp;
            
            return distances[1, b.Count];
        }
        
        private Double CalcLevenshteinDistanceBefore(List<string> a, List<string> b)
        {
            if (a.Count == 0 && b.Count == 0) return 0;
            if (a.Count == 0) return b.Count;
            if (b.Count == 0) return a.Count;
            
            var  distances = new Double[a.Count + 1, b.Count + 1];
           
            for (var i = 0;  i <= a.Count;  distances[i, 0] = i++);
            for (var j = 0;  j <= b.Count;  distances[0, j] = j++);          
            
            for (var i = 1;  i <= a.Count;  i++)
            for (var j = 1;  j <= b.Count;  j++)
            {
                var change = TokenDistanceCalculator.GetTokenDistance(a[i - 1], b[j - 1]);
                var cost = b[j - 1] == a[i - 1] ? 0 : change;
                    
                distances[i,j] = Math.Min
                (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                );
            }
            return distances[a.Count, b.Count];
        }
    }
}
