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

            var distances = new double[2, b.Count + 1];
            for (var i = 0;  i <= b.Count;  distances[0, i] = i++);
            distances[1, 0] = 1;
            
            for (var i = 1;  i <= a.Count;  i++)
            {
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

                for (var k = 0; k < distances.GetLength(1); k++)
                    distances[0,k] = distances[1,k];
                distances[1, 0]++;
            }
            
            return distances[0, b.Count];
        }
    }
}
