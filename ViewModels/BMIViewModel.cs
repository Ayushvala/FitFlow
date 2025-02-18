using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Controllers;

namespace FitnessTracker.ViewModels
{
    public class BMISummaryViewModel
    {
        public IEnumerable<BMICalculationModel> AllRecords { get; set; }
        public IEnumerable<BMICalculationModel> CurrentWeekRecords { get; private set; }
        public IEnumerable<BMICalculationModel> CurrentMonthRecords { get; private set; }
        public BMICalculationModel MostRecentRecord { get; private set; }

        public BMICalculationTarget Target { get; private set; }

        public float CurrentWeekProgress { get; private set; } = 0;
        public float CurrentWeekAverage { get; private set; } = 0;
        public float CurrentMonthProgress { get; private set; } = 0;
        public float CurrentMonthAverage { get; private set; } = 0;
        public float AllTimeProgress { get; private set; } = 0;
        public float AllTimeAverage { get; private set; } = 0;
        public float DistanceToTarget { get; private set; } = 0;
        public float DailyProgressNeeded { get; private set; } = 0;
        public float WeeklyProgressNeeded { get; private set; } = 0;

        public BMISummaryViewModel(IEnumerable<BMICalculationModel> AllRecords, BMICalculationTarget Target)
        {
            if (AllRecords == null || AllRecords.Count() == 0)
                return;

            this.AllRecords = AllRecords;
            this.Target = Target;
            this.MostRecentRecord = AllRecords.First();

            // Calculate the records for the current month and week
            CurrentMonthRecords = AllRecords.Where(record => record.Date >= DateTime.Today.AddDays(-28));
            CurrentWeekRecords = CurrentMonthRecords.Where(record => record.Date >= DateTime.Today.AddDays(-7));

            // Calculate progress and averages for the current week
            if (CurrentWeekRecords.Count() != 0)
            {
                CurrentWeekProgress = CalculateBMI(CurrentWeekRecords.First()) - CalculateBMI(CurrentWeekRecords.Last());
                CurrentWeekAverage = CurrentWeekProgress / 7;
            }

            // Calculate progress and averages for the current month
            if (CurrentMonthRecords.Count() != 0)
            {
                CurrentMonthProgress = CalculateBMI(CurrentMonthRecords.First()) - CalculateBMI(CurrentMonthRecords.Last());
                CurrentMonthAverage = CurrentMonthProgress / 28;
            }

            // Calculate progress for all time
            if (AllRecords.Count() != 0)
            {
                AllTimeProgress = CalculateBMI(AllRecords.First()) - CalculateBMI(AllRecords.Last());
                AllTimeAverage = AllTimeProgress / ((float)(AllRecords.First().Date - AllRecords.Last().Date).TotalDays) * 7;
            }

            // Calculate the distance to target if a target is set
            if (Target == null)
                return;

            DistanceToTarget = (float)(Target.TargetBMIScore - CalculateBMI(MostRecentRecord));
            DailyProgressNeeded = (float)(DistanceToTarget / (Target.TargetDate - DateTime.Today).TotalDays);
            WeeklyProgressNeeded = DailyProgressNeeded * 7;
        }

        // Method to calculate BMI based on the record's weight and height
        private float CalculateBMI(BMICalculationModel record)
        {
            if (record.height > 0)
            {
                // BMI formula: BMI = weight(kg) / height(m)^2
                return (float)(record.weight / (record.height * record.height));
            }
            return 0;
        }
    }
}
