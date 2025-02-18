
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace FitnessTracker.Controllers
{
    [Authorize] // Restrict access to authenticated users only
    public class CoachController : Controller
    {
        // GET: FitnessCoaches
        // This action will be available only to authenticated users
        public IActionResult FitnessCoaches()
        {
            // Create a list of fitness coaches
            var coaches = new List<FitnessCoachModel>
            {
                new FitnessCoachModel
                {
                    Id = 1,
                    Name = "Mike Hesson",
                    Specialty = "Weight Loss, Strength Training",
                    ImageUrl = "https://img.freepik.com/premium-photo/fitness-instructor-gym-after-workout_132453-16296.jpg",
                    Email = "mikehesson@fitness.com"
                },
                new FitnessCoachModel
                {
                    Id = 2,
                    Name = "Andrew Jack",
                    Specialty = "Yoga, Flexibility",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBvAbCKVZse5sWw3em61Kyqh3w3-VdzGAWM_0HLGyN8upZgtmIF5G2BNU--ODwWh2iqHw&usqp=CAU",
                    Email = "andrew@fitness.com"
                },
                new FitnessCoachModel
                {
                    Id = 3,
                    Name = "Spancer Jhonsen",
                    Specialty = "Cardio, Endurance",
                    ImageUrl = "https://img.freepik.com/premium-photo/unrecognizable-fit-hispanic-man-gym-working-out-with-kettlebell_629685-11089.jpg",
                    Email = "spancer@fitness.com"
                },
                new FitnessCoachModel
                {
                    Id = 4,
                    Name = "Lauren Bell",
                    Specialty = "Pilates, Postpartum Fitness",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjn0CYOf3hfE8OTrLSaIxjRklz-FK6c0Y7CMKkFJ-fjIi-_cA2YndE_dneHkAAWq_ieZw&usqp=CAU",
                    Email = "lauren@fitness.com"
                }
            };

            // Return the list of coaches to the view
            return View(coaches);
        }
    }
}
