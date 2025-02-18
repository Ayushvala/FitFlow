using System;
using FitnessTracker.Models;

namespace YourNamespace.Controllers
{
    public class BMICalculationModel
    {
        public long ID { get; set; }
        public FitnessUser User { get; set; }
        public DateTime Date { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double BMIScore { get; set; }
        public string Category { get; set; }  // Add a category property
    }
}
