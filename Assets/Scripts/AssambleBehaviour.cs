using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssambleBehaviour : MonoBehaviour
{
    [SerializeField] Animator Tornillo1, Tornillo2, Tornillo3, Banda, EngraneBig, EngraneSmall, Puerta1, Puerta2;
    [SerializeField] GameObject PuertaGame1, PuertaGame2;
    private bool isDisassamble = false;

    public void DisassambleListener()
    {
        if(isDisassamble)
        {
            StartCoroutine(DelayCerrar());
            isDisassamble = false;
        }
        else
        {
            StartCoroutine(DelayAbrir());
            isDisassamble = true;
        }
    }

    IEnumerator DelayAbrir()
    {
        Puerta1.Play("Puerta3_Abrir");
        Puerta2.Play("Puerta2_Abrir");

        yield return new WaitForSeconds(1.5f);

        PuertaGame1.SetActive(false);
        PuertaGame2.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        Tornillo1.Play("tor1");
        Tornillo2.Play("tor2");
        Tornillo3.Play("tor3");
        EngraneBig.Play("MoveBig");
        Banda.Play("banda");
        EngraneSmall.Play("engrane_small");
    }

    IEnumerator DelayCerrar()
    {
        Tornillo1.Play("tor1(reset)");
        Tornillo2.Play("tor2(reset)");
        Tornillo3.Play("tor3(reset)");
        EngraneBig.Play("MoveBig(reset)");
        Banda.Play("banda(reset)");
        EngraneSmall.Play("engrane_small(reset)");

        yield return new WaitForSeconds(1.5f);

        PuertaGame1.SetActive(true);
        PuertaGame2.SetActive(true);

        Puerta1.Play("Puerta3_Cerrar");
        Puerta2.Play("Puerta2_Cerrar");
    }



    /* Método para mostrar alertas en Android (Toast.makeText) */
    /// <param name="message">Mensaje de tipo String que se desea mostrar en la alerta TOAST</param>
    private void ToastMessage(string message)
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
}
