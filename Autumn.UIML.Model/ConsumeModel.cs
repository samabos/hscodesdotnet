using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ML;
using Autumn_UIML.Model;
using Microsoft.ML.Data;

namespace Autumn_UIML.Model
{
    public class ConsumeModel
    {
        // Cached prediction engine — loaded once, reused for all predictions
        private static readonly Lazy<(PredictionEngine<ModelInput, ModelOutput> Engine, DataViewSchema Schema)> _cached = new(() =>
        {
            var mlContext = new MLContext();
            string modelPath = AppDomain.CurrentDomain.BaseDirectory + "MLModel.zip";
            ITransformer mlModel = mlContext.Model.Load(modelPath, out _);
            var engine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            return (engine, engine.OutputSchema);
        });

        public static ModelOutput Predict(ModelInput input)
        {
            return _cached.Value.Engine.Predict(input);
        }

        public static Dictionary<string, float> Predict(ModelInput input, double threshold)
        {
            var (engine, schema) = _cached.Value;
            ModelOutput result = engine.Predict(input);
            Dictionary<string, float> confidence = GetScoresWithLabelsSorted(schema, "Score", result.Score, threshold);
            return confidence;
        }

        private static Dictionary<string, float> GetScoresWithLabelsSorted(DataViewSchema schema, string name, float[] scores, double threshold)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            var column = schema.GetColumnOrNull(name);

            var slotNames = new VBuffer<ReadOnlyMemory<char>>();
            column.Value.GetSlotNames(ref slotNames);
            var num = 0;
            foreach (var denseValue in slotNames.DenseValues())
            {
                float scoreInternal = scores[num++];
                result.Add(denseValue.ToString(), scoreInternal);
            }

            return result.OrderByDescending(c => c.Value).Take(10).ToDictionary(i => i.Key, i => i.Value);
        }
    }
}
