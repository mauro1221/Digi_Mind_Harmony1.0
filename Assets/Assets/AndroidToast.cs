using UnityEngine;

public static class AndroidToast
{
    /// <summary>
    /// Shows a native Android Toast message.
    /// </summary>
    /// <param name="message">The text to display in the toast.</param>
    public static void Show(string message)
    {
        // This code block will only run on an actual Android device.
        #if UNITY_ANDROID && !UNITY_EDITOR
        
        // We need to run this on the main UI thread in Android
        new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            int duration = toastClass.GetStatic<int>("LENGTH_SHORT");

            // --- THE FIX IS HERE ---
            // Step 1: Create the Toast object by calling the static "makeText" method.
            // We use CallStatic<AndroidJavaObject> to ensure it returns the object.
            AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", context, message, duration);

            // Step 2: Now, call the "show" method on the Toast object we just created.
            toastObject.Call("show");
            // ---------------------
        }));
        
        #else
        Debug.Log("Toast: " + message);
        #endif
    }
}