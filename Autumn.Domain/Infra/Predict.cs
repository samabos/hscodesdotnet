using Autumn_UIML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Infra
{
    public class Predict:IPredict
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

    public interface IPredict
    {
        ModelOutput GetHSCode(string product);
    }
}
