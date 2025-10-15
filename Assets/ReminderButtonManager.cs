using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ReminderButtonManager : MonoBehaviour
{
    // --- UI REFERENCES ---
    [Header("Date Components")]
    public Slider yearSlider;
    public Slider monthSlider;
    public Slider daySlider;

    [Header("Time and Reminder Components")]
    public TMP_InputField timeInput;
    public TMP_InputField reminderInput;

    [Header("Target Button")]
    public Button setReminderButton;

    // --- SCRIPT LOGIC ---

    void Start()
    {
        // --- NEUER CODE ZUM SETZEN DES DATUMS ---
        // Hole das aktuelle Systemdatum
        DateTime today = DateTime.Now;

        // Setze die Werte der Slider auf das heutige Datum
        yearSlider.value = today.Year;
        monthSlider.value = today.Month;
        daySlider.value = today.Day;
        // -----------------------------------------

        // Füge die Listener für die Validierung hinzu
        yearSlider.onValueChanged.AddListener(delegate { ValidateInputs(); });
        monthSlider.onValueChanged.AddListener(delegate { ValidateInputs(); });
        daySlider.onValueChanged.AddListener(delegate { ValidateInputs(); });
        timeInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
        reminderInput.onValueChanged.AddListener(delegate { ValidateInputs(); });

        // Setze den initialen Status des Buttons
        ValidateInputs();
    }

    public void ValidateInputs()
    {
        // 1. Prüfe, ob das Textfeld für die Erinnerung leer ist
        bool isReminderTextValid = !string.IsNullOrEmpty(reminderInput.text);

        // 2. Prüfe, ob die Zeit vollständig und gültig ist
        bool isDateTimeValid = false;
        
        string dateString = $"{(int)yearSlider.value}-{(int)monthSlider.value:D2}-{(int)daySlider.value:D2}";
        string fullDateTimeString = $"{dateString} {timeInput.text}";
        
        if (DateTime.TryParseExact(fullDateTimeString, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDateTime))
        {
            // Prüfe, ob die gewählte Zeit in der Zukunft liegt
            if (selectedDateTime > DateTime.Now)
            {
                isDateTimeValid = true;
            }
        }

        // 3. Der Button ist nur klickbar, wenn beide Bedingungen erfüllt sind
        setReminderButton.interactable = isReminderTextValid && isDateTimeValid;
    }
}