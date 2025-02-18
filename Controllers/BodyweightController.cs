// This controller handles bodyweight records and targets for the authenticated user.
using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

[Authorize] // Ensures that only authenticated users can access these methods.
public class BodyweightController : Controller
{
    private IBodyweightStorageService storageService;  // Service to handle bodyweight data storage and retrieval.
    private UserManager<FitnessUser> userManager; // Manages user authentication and identity.

    // Constructor to initialize the storage service and user manager.
    public BodyweightController(IBodyweightStorageService StorageService, UserManager<FitnessUser> UserManager)
    {
        this.storageService = StorageService;
        this.userManager = UserManager;
    }

    // Retrieves the currently authenticated user.
    private async Task<FitnessUser> GetUser()
    {
        return await userManager.GetUserAsync(HttpContext.User); // Gets the authenticated user.
    }

    // Displays the bodyweight summary for the user.
    public async Task<IActionResult> Summary()
    {
        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Retrieves the user's bodyweight records and target.
        IEnumerable<BodyweightRecord> records = await storageService.GetBodyweightRecords(currentUser);
        BodyweightTarget target = await storageService.GetBodyweightTarget(currentUser);

        // Creates a view model with the records and target for the summary page.
        BodyweightSummaryViewModel viewModel = new BodyweightSummaryViewModel(records, target);

        // Returns the view with the bodyweight summary data.
        return View(viewModel);
    }

    // Displays the form to edit the bodyweight target.
    [HttpGet]
    public async Task<IActionResult> EditTarget()
    {
        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Retrieves the current target bodyweight for the user.
        BodyweightTarget target = await storageService.GetBodyweightTarget(currentUser);

        // Returns the view with the current target data for editing.
        return View(target);
    }

    // Handles the form submission for editing the bodyweight target.
    [HttpPost]
    public async Task<IActionResult> EditTarget(float TargetWeight, DateTime TargetDate)
    {
        // Validates the input data for the target weight and target date.
        if (TargetWeight <= 0 || TargetWeight >= 200 || TargetDate <= DateTime.Today)
            return BadRequest(); // Returns an error if the data is invalid.

        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Retrieves the current target for the user.
        BodyweightTarget newTarget = await storageService.GetBodyweightTarget(currentUser);
        if (newTarget == null)
        {
            // Creates a new target if one does not exist.
            newTarget = new BodyweightTarget()
            {
                User = currentUser
            };
        }

        // Updates the target weight and date.
        newTarget.TargetWeight = TargetWeight;
        newTarget.TargetDate = TargetDate;

        // Saves the updated target data.
        await storageService.StoreBodyweightTarget(newTarget);

        // Redirects the user to the summary page after saving the target.
        return RedirectToAction("Summary");
    }

    // Displays the form to edit the bodyweight records.
    [HttpGet]
    public async Task<IActionResult> EditRecords()
    {
        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Retrieves all bodyweight records for the user.
        BodyweightRecord[] records = await storageService.GetBodyweightRecords(currentUser);

        // Returns the view with the current records for editing.
        return View(records);
    }

    // Handles the form submission for editing multiple bodyweight records.
    [HttpPost]
    public async Task<IActionResult> EditRecords(DateTime[] Dates, float[] Weights)
    {
        // Validates the input data for the dates and weights arrays.
        if (Dates == null || Weights == null)
            return BadRequest(); // Returns an error if the data is invalid.
        if (Dates.Length != Weights.Length)
            return BadRequest(); // Returns an error if the dates and weights arrays don't match.

        // Validates that each weight is within a reasonable range.
        for (int i = 0; i < Dates.Length; i++)
        {
            if (Weights[i] <= 0 || Weights[i] >= 200)
                return BadRequest(); // Returns an error if any weight is invalid.
        }

        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Deletes any existing bodyweight records for the user before updating.
        await storageService.DeleteExistingRecords(currentUser);

        // Creates new records based on the provided dates and weights.
        BodyweightRecord[] records = new BodyweightRecord[Dates.Length];
        for (int i = 0; i < Dates.Length; i++)
        {
            BodyweightRecord newRecord = new BodyweightRecord()
            {
                User = currentUser,
                Date = Dates[i],
                Weight = Weights[i]
            };
            records[i] = newRecord;
        }

        // Saves the new bodyweight records.
        await storageService.StoreBodyweightRecords(records);

        // Redirects the user to the summary page after saving the records.
        return RedirectToAction("Summary");
    }

    // Adds today's bodyweight record for the authenticated user.
    [HttpPost]
    public async Task<IActionResult> AddTodayWeight(float Weight)
    {
        // Validates the input data for today's weight.
        if (Weight <= 0 || Weight >= 200)
            return BadRequest(); // Returns an error if the weight is invalid.

        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Creates a new bodyweight record for today.
        BodyweightRecord newRecord = new BodyweightRecord()
        {
            User = currentUser,
            Date = DateTime.Today,
            Weight = Weight
        };

        // Saves the new bodyweight record.
        await storageService.StoreBodyweightRecord(newRecord);

        // Redirects the user to the summary page after saving the record.
        return RedirectToAction("Summary");
    }

    // Retrieves the user's bodyweight data for a specific number of previous days.
    [HttpGet]
    public async Task<IActionResult> GetBodyweightData(int PreviousDays)
    {
        FitnessUser currentUser = await GetUser(); // Gets the authenticated user.

        // Retrieves bodyweight records for the last 'PreviousDays' days.
        BodyweightRecord[] records = await storageService.GetBodyweightRecords(currentUser, true);

        // Prepares the data in a simple format (date and weight) for JSON response.
        var result = records.Select(record => new { Date = record.Date.ToString("d"), Weight = record.Weight }).ToArray();

        // Returns the data as a JSON response.
        return Json(result);
    }
}
