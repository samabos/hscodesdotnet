

using Autumn.Service.Interface;
using Autumn_UIML.Model;

namespace Autumn.Service
{
    public class PredictService : IPredictService
    {
        public ModelOutput GetHSCode(string product)
        {
            ModelInput data = new ModelInput
            {
                Keyword = product
            };
            // Make a single prediction on the sample data and print results
            ModelOutput predictionResult = ConsumeModel.Predict(data);

            return predictionResult;
        }
    }
}
