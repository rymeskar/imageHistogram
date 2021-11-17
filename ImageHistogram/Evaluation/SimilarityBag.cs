using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageHistogram.Evaluation
{
    public class SimilarityBag
    {
        private readonly Dictionary<string, List<SimilarityHit>> _bag = new Dictionary<string, List<SimilarityHit>>();

        public void AddRange(IEnumerable<SimilarityResponse> responses, DatabaseItem item)
        {
            foreach (var response in responses)
            {
                Add(response, item);
            }
        }
        public void Add(SimilarityResponse response, DatabaseItem item)
        {
            if (!_bag.TryGetValue(response.Name, out var list))
            {
                list = new List<SimilarityHit>();
                _bag[response.Name] = list;
            }
            list.Add(new SimilarityHit(response, item));
        }

        public EvaluationResponse EvaluateBest()
        {
            return new EvaluationResponse(_bag
                .Values
                .Select(
                    l => new SimilarityMeasureEvaluation(
                        l.OrderBy(hit => hit.response.Measure).First(), 
                        l.Aggregate(TimeSpan.Zero, (temp, source) => temp + source.response.Duration))).ToList());
        }
    }
}
