using System;
using System.Collections.Generic;
using System.IO;
using Autumn.Domain.Models;
using Microsoft.ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Autumn.Domain.Infra
{
    public class Tokenizer : ITokenizer
    {
        public Tokenizer()
        { }
        public IList<string> GetTokens(string text)
        {

            IList<string> response = new List<string>();
            try
            {
                var context = new MLContext();
                var emptyData = new List<TextData>();

                var data = context.Data.LoadFromEnumerable(emptyData);
                List<string> verbs = new List<string>();
                List<string> adverbs = new List<string>();
                List<string> adjectives = new List<string>();
                using (StreamReader r = new StreamReader(@"verbs.json"))
                {
                    string json = r.ReadToEnd();
                    verbs = JsonConvert.DeserializeObject<List<string>>(json);
                }
                using (StreamReader r = new StreamReader(@"adverbs.json"))
                {
                    string json = r.ReadToEnd();
                    adverbs = JsonConvert.DeserializeObject<List<string>>(json);
                }
                using (StreamReader r = new StreamReader(@"adjectives.json"))
                {
                    string json = r.ReadToEnd();
                    adjectives = JsonConvert.DeserializeObject<List<string>>(json);
                }

                var tokenization = context.Transforms.Text.TokenizeIntoWords("Tokens", "Text", separators: new[] { ' ', '.', ',', '-','(',')','[',']','{','}','_','+','&','*','$','#','@',';','!','|','`','~' })
                    .Append(context.Transforms.Text.RemoveDefaultStopWords("Tokens", "Tokens", Microsoft.ML.Transforms.Text.StopWordsRemovingEstimator.Language.English))
                    .Append(context.Transforms.Text.RemoveStopWords("Tokens", "Tokens", verbs.ToArray()))
                    .Append(context.Transforms.Text.RemoveStopWords("Tokens", "Tokens", adverbs.ToArray()))
                    .Append(context.Transforms.Text.RemoveStopWords("Tokens", "Tokens", adjectives.ToArray()));

                var model = tokenization.Fit(data);

                var engine = context.Model.CreatePredictionEngine<TextData, TextTokens>(model);

                var result = engine.Predict(new TextData { Text = text });

                return result.Tokens;

            }
            catch(Exception ex){
                //response.Add(ex.Message);
                return response ;
            }
        }

    }


}
