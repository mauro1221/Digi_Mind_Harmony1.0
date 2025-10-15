using System;
using UnityEngine;
using UnityEngine.UI; // Required for the Slider class
using TMPro;

public class ReminderScheduler : MonoBehaviour
{
    // Changed from InputFields to Sliders
    public Slider yearSlider;
    public Slider monthSlider;
    public Slider daySlider;

    public TMP_InputField timeInput; // Still using an InputField for time

    public TMP_InputField reminderInput;
    public AndroidNotificationController androidNotificationController;

    public void ScheduleReminder()
    {
        // --- GETTING VALUES FROM SLIDERS ---

        // Get the value from each slider and cast it to an integer
        int year = (int)yearSlider.value;
        int month = (int)monthSlider.value;
        int day = (int)daySlider.value;

        // --- PARSING THE TIME (remains the same) ---

        string[] timeParts = timeInput.text.Split(':');
        if (timeParts.Length != 2 ||
            !int.TryParse(timeParts[0], out int hour) ||
            !int.TryParse(timeParts[1], out int minute))
        {
            Debug.LogError("Invalid time format! Please use HH:mm.");
            return;
        }

        // --- CREATING THE DATETIME & SCHEDULING (remains the same) ---

        DateTime reminderTime;
        try
        {
            reminderTime = new DateTime(year, month, day, hour, minute, 0);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError($"Invalid date! The date {year}-{month}-{day} does not exist.");
            return;
        }

        DateTime now = DateTime.Now;
        if (reminderTime <= now)
        {
            Debug.LogWarning("Selected time is in the past. Choose a future time.");
            return;
        }

        TimeSpan delay = reminderTime - now;
        int seconds = (int)delay.TotalSeconds;

        androidNotificationController.SendNotification(
            $"{reminderInput.text}",
            $"Reminder for {day}.{month}.{year}, {hour}:{minute:D2}",
            seconds
        );

        Debug.Log($"Reminder set for {reminderTime} in {seconds} seconds.");

        AndroidToast.Show("Reminder Set");
        reminderInput.text = "";
    }
}