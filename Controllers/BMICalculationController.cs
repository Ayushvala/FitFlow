using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace YourNamespace.Controllers
{
    // The [Authorize] attribute ensures that only authenticated users can access this controller
    [Authorize] // Restrict access to authenticated users only
    public class BMICalculationController : Controller
    {
        // GET: BMIView
        // This action will be available only to authenticated users
        public ActionResult BMIView()
        {
            // The model is initialized here
            return View(new BMICalculationModel());
        }

        // POST: BMIView
        [HttpPost]
        // This action is for submitting the form with BMI calculation
        public ActionResult BMIView(BMICalculationModel model)
        {
            if (ModelState.IsValid)
            {
                // Calculate BMI using the formula: weight / (height in meters)^2
                model.BMIScore = model.weight / ((model.height * 0.01) * (model.height * 0.01));

                // Determine the BMI category based on the calculated BMI
                if (model.BMIScore < 18.5)
                {
                    model.Category = "Underweight";
                }
                else if (model.BMIScore >= 18.5 && model.BMIScore <= 24.9)
                {
                    model.Category = "Healthy weight";
                }
                else if (model.BMIScore >= 25 && model.BMIScore <= 29.9)
                {
                    model.Category = "Overweight";
                }
                else
                {
                    model.Category = "Obesity";
                }
            }
            else
            {
                // Handle invalid model (optional)
                // You could display an error message if the model is invalid
                Console.WriteLine("Kuch to gadbad jarur he");
            }

            // Return the model back to the view, with calculated BMI and category
            return View(model);
        }
    }
}
