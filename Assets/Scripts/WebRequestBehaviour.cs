using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;


public class WebRequestBehaviour : MonoBehaviour
{




    [Serializable]
    public struct FirebaseJSON
    {
        public struct register
        {
            public string name;
            public int score;
        }

        public register[] data;
    }

    public FirebaseJSON DataFirebase;

    private UnityWebRequest webRequest;
    private string rootPath = "http://localhost:5000/restservice-89269/us-central1/app/api/";



    /*
     * GET  -> Obtener datos o recursos unicamente
     * POST -> Crear nuevos recursos / envio de datos
     * PUT  -> Actualizar recursos existente
     * PUSH -> Actualizar un dato parcialmente (solo un dato)
     * DELETE -> Eliminar recursos
     * 
     */

    #region Template
    public void GetInfo()
   {
      StartCoroutine(GetRequest());
   }


   
   public void SetInfo()
   {
      StartCoroutine(PostUnityWebRequest());
   }

    private IEnumerator GetRequest()
   {
        webRequest = UnityWebRequest.Get("https://universal-unity-66825-default-rtdb.firebaseio.com/.json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError && webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
        }
        else
        {
            print(webRequest.downloadHandler.text);
            GetSetData deserializeJson = JsonUtility.FromJson<GetSetData>(webRequest.downloadHandler.text);
            print(deserializeJson.body);
        }
   }

   private IEnumerator PostUnityWebRequest()
   {
      Dictionary<string, object> dictionary = new Dictionary<string, object>()
      {
         { "title", "titulo de prueba" },
         { "body", 23 }
      };

      WWWForm form = new WWWForm();
      form.AddField("body", Random.Range(0, 101));
      form.AddField("title", "titulo de prueba");
      
      GetSetData myObject = new GetSetData();
      myObject.title = "titulo de unity";
      myObject.body = Random.Range(0, 101);
      
      string serializeJson = JsonUtility.ToJson(myObject);
      using (webRequest = UnityWebRequest.Put("https://universal-unity-66825-default-rtdb.firebaseio.com/.json", serializeJson))
      {
         webRequest.method = UnityWebRequest.kHttpVerbPOST;
         webRequest.SetRequestHeader("Content-Type", "application/json");
         webRequest.SetRequestHeader("Accept", "application/json");
         yield return webRequest.SendWebRequest();
         if (webRequest.result != UnityWebRequest.Result.Success)
         {
            print("Error." + webRequest.error + ", " +  webRequest.downloadHandler.text);
         }
         else
         {
            print("Registro exitoso");
            print(webRequest.downloadHandler.text);
         }
      }
   }

    #endregion



}

public class GetSetData
{
    public string title;
    public int body;
}
