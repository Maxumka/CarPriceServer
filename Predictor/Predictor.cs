using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpPredictorML.Model
{
    public static class Predictor
    {
        public static int PredictOnePrice(string js)
        {
            var model = System.Text.Json.JsonSerializer.Deserialize<ModelInput>(js);
            var predictionResult = ConsumeModel.Predict(model);
            return (int)predictionResult.Score;
        }
        public static string GetBestDeals(string js)
        {
            var cars = JsonConvert.DeserializeObject<List<ModelInput>>(js);
            Dictionary<ModelInput, float> CarAndPrice = new Dictionary<ModelInput, float>();
            foreach (var car in cars)
            {
                var predictedPrice = PredictOnePrice(System.Text.Json.JsonSerializer.Serialize(car));
                CarAndPrice.Add(car, car.Price - predictedPrice);
            }

            var sortedcars = CarAndPrice.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value); ;

            return System.Text.Json.JsonSerializer.Serialize(CarAndPrice.Keys).ToString();
        }
    }
}
