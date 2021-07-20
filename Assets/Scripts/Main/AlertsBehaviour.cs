using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
//using DeadMosquito.IosGoodies;

public class AlertsBehaviour
{
    /// <summary>
    /// Metodo para ver alerta para Toast para android.
    /// </summary>
    /// <param name="message">Mensage de tipo string para ver en el Toast.</param>
    public void AndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    /// <summary>
    /// Metodo para ver alerta de Debug en el Canvas
    /// </summary>
    /// <param name="message">Mensaje string para ver en la alerta</param>
    public void DebugText(string message, Colour color)
    {
        TextMeshProUGUI deb = GameObject.FindWithTag("Debug").GetComponent<TextMeshProUGUI>();        
        Color customColor = new Color();

        if (color == Colour.Error)
        {
            customColor = Color.red;
        }
        if (color == Colour.Success)
        {
            customColor = Color.green;
        }
        if (color == Colour.Normal)
        {
            customColor = Color.white;
        }

        deb.color = customColor;
        deb.text = message;
    }

    /// <summary>
    /// Metodo para ver alertas en IOS
    /// </summary>
    /// <param name="header">Encabezado de la alerta</param>
    /// <param name="message">Mensaje de la alerta</param>
    [UsedImplicitly]
    public void iOSConfirmationDialog(string header, string message)
    {
        #if UNITY_IOS
        IGDialogs.ShowOneBtnDialog(header, message, "Confirmar", () => Debug.Log("iOS. Alerta cerrada!"));
        #endif
    }
}

public enum Colour
{
    Error,
    Success,
    Normal
}

