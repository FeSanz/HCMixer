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
   [SerializeField] private TMP_InputField nameInput;
   
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
    */
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
        string path = "cuestionario/01";
      webRequest = UnityWebRequest.Get(
         "https://universal-unity-66825-default-rtdb.firebaseio.com/.json");
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

    #region CarlosApi Rest FireBase
    public void GetCuestionario()
    {
        StartCoroutine(GetRequestCuestionario());
    }

    private IEnumerator GetRequestCuestionario()
    {
        string path = "cuestionario/01";
        webRequest = UnityWebRequest.Get(rootPath + path);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError && webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
        }
        else
        {
            print(webRequest.downloadHandler.text);
            GetCuestionarioData deserializeJson = JsonUtility.FromJson<GetCuestionarioData>(webRequest.downloadHandler.text);
            print(deserializeJson);
        }
    }

    public void GetStatusCuestionario()
    {
        StartCoroutine(GetRequestCuestionarioStatus("01"));
    }

    private IEnumerator GetRequestCuestionarioStatus(string user)
    {
        string path = "cuestionario_status/01";
        webRequest = UnityWebRequest.Get(rootPath + path);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        GetSetCuestionarioStatus data = new GetSetCuestionarioStatus();
        data.user = user;

        string jsonData = JsonUtility.ToJson(data);

        if (jsonData != null)
        {
            byte[] datos = System.Text.Encoding.UTF8.GetBytes(jsonData);
            UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
            upHandler.contentType = "application/json";
            webRequest.uploadHandler = upHandler;
        }

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError && webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
        }
        else
        {
            print(webRequest.downloadHandler.text);
            GetSetCuestionarioStatus deserializeJson = JsonUtility.FromJson<GetSetCuestionarioStatus>(webRequest.downloadHandler.text);
            print(deserializeJson);
        }
    }
     
    public void PostStatusCuestionario()
    {
        StartCoroutine(PostCuestionarioStatus());
    }

    private IEnumerator PostCuestionarioStatus()
    {
      
        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = "01";
        myObject.tiempo = Random.Range(0, 1000);
        myObject.completo = true;
        myObject.calificacion = 90;
        myObject.intentos = 1;

        string serializeJson = JsonUtility.ToJson(myObject);



        using (webRequest = UnityWebRequest.Post(rootPath + path, serializeJson))
        {

            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");


            if (serializeJson != null)
            {
                byte[] datos = System.Text.Encoding.UTF8.GetBytes(serializeJson);
                UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
                upHandler.contentType = "application/json";
                webRequest.uploadHandler = upHandler;
            }

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " + webRequest.downloadHandler.text);
            }
            else
            {
                print("Registro exitoso");
                print(webRequest.downloadHandler.text);
            }
        }
    }

    public void PutStatusCuestionario()
    {
        StartCoroutine(PutCuestionarioStatus());
    }

    private IEnumerator PutCuestionarioStatus()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = "01";
        myObject.tiempo = Random.Range(0, 1000);
        myObject.completo = true;
        myObject.calificacion = 10;
        myObject.intentos = 1;

        string serializeJson = JsonUtility.ToJson(myObject);



        using (webRequest = UnityWebRequest.Put(rootPath + path, serializeJson))
        {

            webRequest.method = UnityWebRequest.kHttpVerbPUT;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");


            if (serializeJson != null)
            {
                byte[] datos = System.Text.Encoding.UTF8.GetBytes(serializeJson);
                UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
                upHandler.contentType = "application/json";
                webRequest.uploadHandler = upHandler;
            }

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " + webRequest.downloadHandler.text);
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

public class GetCuestionarioData
{
    public string name;
    public int no_preguntas;
    public string id;
    public Dictionary<string, Dictionary<string,string[]>> preguntas;

}

public class GetSetCuestionarioStatus 
{
    public string user;
    public int  tiempo;
    public bool completo;
    public float calificacion;
    public int intentos;

}
