using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AssessOperationMixer : MonoBehaviour
{
    [SerializeField] private Sprite complete;
    [SerializeField] private Sprite incomplete;
    
    [SerializeField] private Image arranqueIcon;
    [SerializeField] private Image paroIcon;
    [SerializeField] private Image velocidadAltaIcon;
    [SerializeField] private Image velocidadBajaIcon;
    [SerializeField] private Image tensarBandaIcon;

    [SerializeField] private TextMeshProUGUI userText;
    
    
    private UnityWebRequest webRequest;
    public static string user = "Anonimus";
    public static string _url= "";
    private bool checkNull = true;

    private AssessStructure assessStructure;

    private void Start()
    {
        userText.SetText(user);
        user = "felipe_antonio";
        _url = "https://mixerar-d96f8-default-rtdb.firebaseio.com/" + user;
        assessStructure = new AssessStructure();

        StartCoroutine(GetAssignmentRecords());
    }

    public void PatchPanelListener(string key)
    {
        Dictionary<string, object> panelArranque = new Dictionary<string, object>(){{ key, true }};
        string json = JsonConvert.SerializeObject(panelArranque, Formatting.Indented);

        StartCoroutine(PatchAssignment( _url +"/assignmentRecordOperation/panelControl.json", json));
    }

    public void GetAssignmentRecordsCheckListener()
    {
        StartCoroutine(GetAssignmentRecordsCheck());
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
    
    private IEnumerator GetAssignmentRecordsCheck()
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
                assessStructure = JsonUtility.FromJson<AssessStructure>(webRequest.downloadHandler.text);
                ChangeStatus(
                    assessStructure.assignmentRecordOperation.panelControl.arranque,
                    assessStructure.assignmentRecordOperation.panelControl.paro,
                    assessStructure.assignmentRecordOperation.panelControl.velocidadAlta,
                    assessStructure.assignmentRecordOperation.panelControl.velocidadBaja,
                    assessStructure.assignmentRecordOperation.tensarBanda.pasos
                    );
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

    private void ChangeStatus(bool arranque, bool paro, bool alta, bool baja, string pasos)
    {
        arranqueIcon.sprite = arranque ? complete : incomplete;
        arranqueIcon.color = arranque ? Color.green : Color.red;
        
        paroIcon.sprite = paro ? complete : incomplete;
        paroIcon.color = paro ? Color.green : Color.red;
        
        velocidadAltaIcon.sprite = alta ? complete : incomplete;
        velocidadAltaIcon.color = alta ? Color.green : Color.red;
        
        velocidadBajaIcon.sprite = baja ? complete : incomplete;
        velocidadBajaIcon.color = baja ? Color.green : Color.red;
        
        tensarBandaIcon.sprite = pasos.Equals("9/9") ? complete : incomplete;
        tensarBandaIcon.color = pasos.Equals("9/9") ? Color.green : Color.red;
    }

    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}
