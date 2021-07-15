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
    public void DebugText(string message, TypeAlert alert)
    {
        TextMeshProUGUI deb = GameObject.FindWithTag("Debug").GetComponent<TextMeshProUGUI>();        
       
        deb.color = TypeColor(alert);
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
    
    private Color32 TypeColor(TypeAlert typeAlert)
    {
        Color color;
        if (typeAlert == TypeAlert.Error)
        {
            color =new Color32(255, 4, 9, 94);
            return color;
        }
        else if (typeAlert == TypeAlert.Success)
        {
            color = new Color32(0, 234, 139, 94);
            return color;
        }
        else if (typeAlert == TypeAlert.Default)
        {
            color = new Color32(255, 255, 255, 94);
            return color;
        }
        else
        {
            color = new Color32(255, 255, 255, 94);
            return color;
        }
    }
}


public enum TypeAlert
{
    Error,
    Success,
    Default
}

