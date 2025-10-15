using UnityEngine;
using TMPro;

public class QuoteGenerator : MonoBehaviour
{
    [TextArea]
    public string[] quotes;          // list of quotes
    public TMP_Text quoteText;       // the text element where it will appear

    public void ShowRandomQuote()
    {
        if (quotes.Length == 0 || quoteText == null) return;

        int randomIndex = Random.Range(0, quotes.Length);
        quoteText.text = quotes[randomIndex];
    }

     private void OnEnable()
    {
        ShowRandomQuote();
    }
}
