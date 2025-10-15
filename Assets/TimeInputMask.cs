using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections; // Required for Coroutines (IEnumerator)

[RequireComponent(typeof(TMP_InputField))]
public class TimeInputMask : MonoBehaviour
{
    private TMP_InputField timeInput;
    // We use a backing field to track the raw numbers without the colon
    private string previousNumbersOnly = "";

    void Awake()
    {
        timeInput = GetComponent<TMP_InputField>();
    }

    void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        timeInput.onValueChanged.AddListener(FormatTime);
    }

    void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        timeInput.onValueChanged.RemoveListener(FormatTime);
    }

    private void FormatTime(string input)
    {
        // 1. Get only the digits from the current input text
        string numbersOnly = new string(input.Where(char.IsDigit).ToArray());

        // If nothing has changed in the numbers, do nothing.
        // This helps prevent issues when we re-set the text.
        if (numbersOnly == previousNumbersOnly)
        {
            return;
        }

        previousNumbersOnly = numbersOnly;

        // Truncate if the user pastes too many numbers
        if (numbersOnly.Length > 4)
        {
            numbersOnly = numbersOnly.Substring(0, 4);
        }

        // 2. Build the formatted string
        string formattedText = "";
        
        // Handle Hours
        if (numbersOnly.Length > 0)
        {
            string hourString = numbersOnly.Substring(0, Mathf.Min(2, numbersOnly.Length));
            if (int.TryParse(hourString, out int hours) && hours > 23)
            {
                hourString = "23";
            }
            formattedText += hourString;
        }
        
        // Handle Colon and Minutes
        if (numbersOnly.Length > 2)
        {
            formattedText += ":";
            string minuteString = numbersOnly.Substring(2, Mathf.Min(2, numbersOnly.Length - 2));
            if (int.TryParse(minuteString, out int minutes) && minutes > 59)
            {
                minuteString = "59";
            }
            formattedText += minuteString;
        }
        
        // 3. Set the text and start the coroutine to fix the cursor position
        timeInput.text = formattedText;
        StartCoroutine(MoveCaretToEnd());
    }

    private IEnumerator MoveCaretToEnd()
    {
        // Wait until the end of the frame.
        // This is the crucial part that fixes the cursor jumping to the start.
        yield return new WaitForEndOfFrame();
        
        // Now, safely move the cursor to the end of the text.
        timeInput.caretPosition = timeInput.text.Length;
    }
}