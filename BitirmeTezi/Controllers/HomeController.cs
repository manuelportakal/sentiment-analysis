using BitirmeTezi.Models;
using BitirmeTeziML.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using System;
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
            var list = context.Emojis.ToArray();
            return View();
        }


        [HttpPost]
        public IActionResult Index(ModelInput input)
        {

            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(@"..\BitirmeTeziML.Model\MLModel.zip", out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            ModelOutput result = predEngine.Predict(input);
            String[] emotion = new String[6] { "sadness", "anger", "love", "surprise", "fear", "joy" };

            var emoarray = emotion.Zip(result.Score, (first, second) => first + ": " + Math.Round(second, 4));
            ViewBag.Score = emoarray;
            ViewBag.Prediction = result.Prediction;
            return View();
        }
    }

}

