using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ImageHistogram.Evaluation
{
    public class SimilarityBag
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<SimilarityHit>> _bag = new ConcurrentDictionary<string, ConcurrentBag<SimilarityHit>>();

        public void AddRange(IEnumerable<SimilarityResponse> responses, DatabaseItem item)
        {
            foreach (var response in responses)
            {
                Add(response, item);
            }
        }
        public void Add(SimilarityResponse response, DatabaseItem item)
        {
            var list = _bag.GetOrAdd(response.Name, (_) => new ConcurrentBag<SimilarityHit>());
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
