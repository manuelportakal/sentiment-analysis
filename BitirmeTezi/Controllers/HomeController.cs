using BitirmeTezi.Models;
using BitirmeTeziML.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace BitirmeTezi.Controllers
{
    public class HomeController : Controller
    {

        thesisDatabaseContext context;
        public HomeController(thesisDatabaseContext _context) => context = _context;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(ModelInput input)
        {

            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(@"..\BitirmeTeziML.Model\MLModel.zip", out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            ModelOutput result = predEngine.Predict(input);
            ModelOutput tmp_result = result;

            for (int i = 0; i < tmp_result.Score.Length; i++)
            {
                result.Score[i] = (float)Math.Round(tmp_result.Score[i], 4);
            }


            IDictionary<string, float> mydic = new Dictionary<string, float>();
            String[] emotion = new String[6] { "sadness", "anger", "love", "surprise", "fear", "joy" };
            for (int i = 0; i < emotion.Length; i++)
            {
                mydic.Add(emotion[i], result.Score[i]);
            }

            ViewBag.Secilenler = findEmoji(mydic);
            ViewBag.Score = mydic;
            ViewBag.Prediction = result.Prediction;
            return View();
        }

        public List<string> findEmoji(IDictionary<string, float> mydic)
        {
            IDictionary<string, float> tmp_mydic = new Dictionary<string, float>();
            foreach (KeyValuePair<string, float> kvp in mydic)
            {
                if (kvp.Value >= 0.25)
                {
                    tmp_mydic.Add(kvp);
                }
                Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);

            }

            List<string> secilenemojiler = new List<string>();
            foreach (KeyValuePair<string, float> kvp in tmp_mydic)
            {
                var query = from st in context.Emojis
                            where st.Grup == kvp.Key
                            select st;

                var emoji = query.ToArray();
                double enkucukfark = 100;
                string enkucukfarkEmojisi = " ";
                foreach (var item in emoji)
                {
                    double fark = 0;
                    double angerpoint = Convert.ToDouble(item.AngerPoint.Replace(".", ","));
                    double fearpoint = Convert.ToDouble(item.FearPoint.Replace(".", ","));
                    double joypoint = Convert.ToDouble(item.JoyPoint.Replace(".", ","));
                    double lovepoint = Convert.ToDouble(item.LovePoint.Replace(".", ","));
                    double sadnesspoint = Convert.ToDouble(item.SadnessPoint.Replace(".", ","));
                    double surprisepoint = Convert.ToDouble(item.SurprisePoint.Replace(".", ","));

                    switch (kvp.Key)
                    {
                        case "sadness":
                            fark = Math.Abs(kvp.Value - sadnesspoint);
                            break;
                        case "anger":
                            fark = Math.Abs(kvp.Value - angerpoint);
                            break;
                        case "love":
                            fark = Math.Abs(kvp.Value - lovepoint);
                            break;
                        case "surprise":
                            fark = Math.Abs(kvp.Value - surprisepoint);
                            break;
                        case "fear":
                            fark = Math.Abs(kvp.Value - fearpoint);
                            break;
                        case "joy":
                            fark = Math.Abs(kvp.Value - joypoint);
                            break;
                    }

                    if(fark <= enkucukfark)
                    {
                        enkucukfark = fark;
                        enkucukfarkEmojisi = item.Emo;
                    }
                }
                secilenemojiler.Add(enkucukfarkEmojisi);
            }
            return secilenemojiler;
        }
    }

}

