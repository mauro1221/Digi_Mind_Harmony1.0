using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueToText : MonoBehaviour
{
    // Reference to the TextMeshPro text component
    public TMP_Text valueText;

    // This function will be called when the slider's value changes
    public void UpdateText(float value)
    {
        // Update the text to display the slider's value
        valueText.text = value.ToString();
    }
}