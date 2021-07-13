using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class ExplanationBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform NextButton;
    [SerializeField] Transform BackButton;
    [SerializeField] TextMeshProUGUI InstructionsTMPUGUI;
    [SerializeField] Transform InstructionsPanel, ButtonsPanel;
    [SerializeField] Transform Mixer;

    private TensarBandaBehaviourScript tbbs;
    private UnityWebRequest webRequest;

    private int j, tam;
    private string[] lines;

    private void Start()
    {
        tbbs = this.GetComponent<TensarBandaBehaviourScript>();
        j = 0;
    }

    public void CancelClicked()
    {
        tbbs.Step0();
        j = 0;
        tam = 0;
        lines = null;
        InstructionsPanel.gameObject.SetActive(false);
        ButtonsPanel.gameObject.SetActive(false);

    }

    public void buttonBackClicked()
    {
        if (j.Equals(0))
        {
            tbbs.Step0();
            InstructionsPanel.gameObject.SetActive(false);
            ButtonsPanel.gameObject.SetActive(false);
        }
        else
        {
            j--;
            InstructionsTMPUGUI.text = lines[j];
            switch (j)
            {
                case 0:
                    tbbs.Step1();
                    break;
                case 1:
                    tbbs.Step2();
                    break;
                case 2:
                    tbbs.Step3();
                    break;
                case 3:
                    tbbs.Step4();
                    break;
                case 4:
                    tbbs.Step5();
                    break;
                case 5:
                    tbbs.Step6();
                    break;
                case 6:
                    tbbs.Step7();
                    break;
                case 7:
                    tbbs.Step8();
                    break;
                case 8:
                    tbbs.Step9();
                    break;
            }
        }
    }

    public void buttonNextClicked()
    {
        if (j < tam)
        {
            j++;
            InstructionsTMPUGUI.text = lines[j];
            switch(j)
            {
                case 0:
                    tbbs.Step1();
                    PatchSupportListener("1/9");
                    break;
                case 1:
                    tbbs.Step2();
                    PatchSupportListener("2/9");
                    break;
                case 2:
                    tbbs.Step3();
                    PatchSupportListener("3/9");
                    break;
                case 3:
                    tbbs.Step4();
                    PatchSupportListener("4/9");
                    break;
                case 4:
                    tbbs.Step5();
                    PatchSupportListener("5/9");
                    break;
                case 5:
                    tbbs.Step6();
                    PatchSupportListener("6/9");
                    break;
                case 6:
                    tbbs.Step7();
                    PatchSupportListener("7/9");
                    break;
                case 7:
                    tbbs.Step8();
                    PatchSupportListener("8/9");
                    break;
                case 8:
                    tbbs.Step9();
                    PatchSupportListener("9/9");
                    break;
            }
        }
        else
        {
            j = 0;
            InstructionsPanel.gameObject.SetActive(false);
            ButtonsPanel.gameObject.SetActive(false);
        }
    }
    
    public void PatchSupportListener(string value)
    {
        Dictionary<string, object> panelArranque = new Dictionary<string, object>(){{ "pasos", value }};
        string json = JsonConvert.SerializeObject(panelArranque, Formatting.Indented);

        StartCoroutine(PatchAssignment( AssessOperationMixer._url +"/assignmentRecordOperation/tensarBanda.json", json));
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
    
    public void ReadString(string name)
    {
        if (Mixer.gameObject.activeSelf)
        {
            try
            {

                TextAsset v_text = Resources.Load("Instructions/" + name) as TextAsset;

                string[] linesFromFile = v_text.text.Split("\n"[0]);
                int i = linesFromFile.Length;
                lines = new string[i];
                int ii = 0;
                foreach (string l in linesFromFile)
                {
                    lines[ii] = l;
                    ii++;
                }
                tam = i - 1;
                j = -1;

            }
            catch (Exception e)
            {
                InstructionsTMPUGUI.text = e.Message;
            }
            InstructionsPanel.gameObject.SetActive(true);
            ButtonsPanel.gameObject.SetActive(true);

            this.buttonNextClicked();
        }
        

    }
}
