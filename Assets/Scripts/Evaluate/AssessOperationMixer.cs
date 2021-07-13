using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class AssessOperationMixer : MonoBehaviour
{
    private UnityWebRequest webRequest;
    private string user = "Curtis_Feitty";
    public static string _url= "";
    private bool checkNull = true;

    private AssessStructure assessStructure;

    private void Start()
    {
        _url = "https://universal-unity-66825-default-rtdb.firebaseio.com/" + user;
        assessStructure = new AssessStructure();
        StartCoroutine(GetAssignmentRecords());
    }

    public void PatchPanelListener(string key)
    {
        Dictionary<string, object> panelArranque = new Dictionary<string, object>(){{ key, true }};
        string json = JsonConvert.SerializeObject(panelArranque, Formatting.Indented);

        StartCoroutine(PatchAssignment( _url +"/assignmentRecordOperation/panelControl.json", json));
    }

    private IEnumerator GetAssignmentRecords()
    {
        using (webRequest = UnityWebRequest.Get(_url +".json"))
        {
            webRequest.method = UnityWebRequest.kHttpVerbGET;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " +  webRequest.downloadHandler.text);
            }
            else
            {
                if (webRequest.downloadHandler.text == "null")
                {
                    StartCoroutine(PostStartAssignmentRecords());
                }
                else
                {
                    print("Ok.");
                }
            }
        }
    }
    
    private IEnumerator PostStartAssignmentRecords()
    {
        assessStructure.assignmentRecordName = "CAPACITACION AR";
        assessStructure.fecha = CurrentDate();
        assessStructure.hora = CurrentTime();

        assessStructure.assignmentRecordOperation.panelControl.arranque = false;
        assessStructure.assignmentRecordOperation.panelControl.paro = false;
        assessStructure.assignmentRecordOperation.panelControl.velocidadAlta = false;
        assessStructure.assignmentRecordOperation.panelControl.velocidadBaja = false;

        assessStructure.assignmentRecordOperation.tensarBanda.pasos = "0/9";

        assessStructure.assignmentRecordOperation.tiempo = 1;

        string serializeJson = JsonUtility.ToJson(assessStructure);
      
        using (webRequest = UnityWebRequest.Put(_url +".json", serializeJson))
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

    private IEnumerator PatchAssignment(string url_api, string json)
    {
        using (webRequest = UnityWebRequest.Put(url_api, json))
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
        return DateTime.Now.ToString("HH:mm:ss");
    }
}
