using System;

namespace FitnessTracker.Models
{
    public class BMICalculationTarget
    {
        public long ID { get; set; }
        public FitnessUser User { get; set; }
        public DateTime TargetDate { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double TargetBMIScore { get; set; }
    }
}
