using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssessOperationMixer : MonoBehaviour
{
    private UnityWebRequest webRequest;
    private string TempUserName = "Curtis_Feitty";
    public void PostToFireBae()
    {
        StartCoroutine(PostUnityWebRequest());
    }
    
    public void PatchToFireBae()
    {
        StartCoroutine(PatchUnityWebRequest());
    }
    
    private IEnumerator PostUnityWebRequest()
    {
        AssessStructure assessStructure = new AssessStructure();
        
        assessStructure.assignmentRecordName = "CAPACITACION AR";
        assessStructure.fecha = CurrentDate();
        assessStructure.hora = CurrentTime();

        assessStructure.assignmentRecordOperation.panelControl.arranque = false;
        assessStructure.assignmentRecordOperation.panelControl.paro = false;
        assessStructure.assignmentRecordOperation.panelControl.velocidadAlta = false;
        assessStructure.assignmentRecordOperation.panelControl.velocidadBaja = false;
        assessStructure.assignmentRecordOperation.panelControl.calificacion = 0;

        assessStructure.assignmentRecordOperation.tensarBanda.pasos = "0/6";
        assessStructure.assignmentRecordOperation.tensarBanda.calificacion = 0;

        assessStructure.assignmentRecordOperation.tiempo = 1;
        assessStructure.assignmentRecordOperation.completo = false;

        assessStructure.assignmentRecordTest.tiempo = 1;
        assessStructure.assignmentRecordTest.respuestas = "0/6";
        assessStructure.assignmentRecordTest.calificacion = 0;
        assessStructure.assignmentRecordTest.intentos = "0/3";
        
        string serializeJson = JsonUtility.ToJson(assessStructure);
      
        using (webRequest = UnityWebRequest.Put("https://universal-unity-66825-default-rtdb.firebaseio.com/"+TempUserName+".json", serializeJson))
        {
            webRequest.method = UnityWebRequest.kHttpVerbPUT;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " +  webRequest.downloadHandler.text);
            }
            else
            {
                print("Registro exitoso!!!");
                print(webRequest.downloadHandler.text);
            }
        }
    }

    private IEnumerator PatchUnityWebRequest()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>()
        {
            { "assignmentRecordName", "titulo de prueba" },
            { "fecha", "20/20/20" }
        };
        
        string serializeJson = JsonUtility.ToJson(dictionary);
        print(dictionary.ToString());
        
        using (webRequest = UnityWebRequest.Put("https://universal-unity-66825-default-rtdb.firebaseio.com/"+TempUserName+".json", dictionary.ToString()))
        {
            webRequest.method = "PATCH";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " +  webRequest.downloadHandler.text);
            }
            else
            {
                print("Cambio exitoso!!!");
                print(webRequest.downloadHandler.text);
            }
        }
    }

    string CurrentDate()
    {
        return DateTime.Now.ToString("dd/MM/yyyy");
    }

    string CurrentTime()
    {
        //return DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        return DateTime.Now.ToString("HH:mm:ss");
    }
}
