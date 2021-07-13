using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScaleBehavior : MonoBehaviour
{
    [SerializeField] private GameObject Mixer, ScalePop, EditPop, EditMode, ExitPop;
    [SerializeField] private Button SmallButton, MidiumButton, BigButton;
    [SerializeField] private Image ResizeImage;
    [SerializeField] private Animator EditAnimation;

    public float Small, Midium, Big;
    public float PtoM, PtoG, MtoG;
    private bool isSmall = true, isMidium, isBig;

    public static bool isEditionMode = false;
    public static float PositionY;
    
    private float timeOnOperation = 0.0f;


    private AlertsBehaviour _alertsBehaviour;
    private UnityWebRequest webRequest;

    private void Start()
    {
        _alertsBehaviour = new AlertsBehaviour();
        
        ResizeImage = GetComponent<Image>();
    }

    private void Update()
    {
        timeOnOperation += Time.deltaTime;
        //print(timeOnOperation.ToString("f0"));
    }

    public void ShowExitListener()
    {
        ExitPop.SetActive(true);
    }
    public void CancelExitListener()
    {
        ExitPop.SetActive(false);
    }

    public void ExitApplicationListener()
    {
        PatchTimeOpearion(timeOnOperation);
    }
    
    public void PatchTimeOpearion(float value)
    {
        Dictionary<string, object> panelArranque = new Dictionary<string, object>(){{ "tiempo", Mathf.Round(value / 60)}};
        string json = JsonConvert.SerializeObject(panelArranque, Formatting.Indented);

        StartCoroutine(PatchAssignment( AssessOperationMixer._url +"/assignmentRecordOperation.json", json));
        //print(AssessOperationMixer._url);
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
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    public void ShowScaleListener()
    {
        ScalePop.SetActive(true);
    }
    public void HideScaleListener()
    {
        ScalePop.SetActive(false);
    }

    public void ScaleSmallListener()
    {
        isSmall = true;
        SmallButton.interactable = false;
        MidiumButton.interactable = true;
        BigButton.interactable = true;
        ScalePop.SetActive(false);
        Mixer.transform.localScale = new Vector3(Small, Small, Small);

        if(isMidium)
        {
            PositionY = (Mixer.transform.position.y) - PtoM;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }  
         if(isBig)
        {
            PositionY = (Mixer.transform.position.y) - PtoG;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }
             
         _alertsBehaviour.AndroidToastMessage("Tamaño pequeño del modelo");
        isMidium = false;
        isBig = false;
    }

    public void ScaleMidiumListener()
    {
        isMidium = true;
        SmallButton.interactable = true;
        MidiumButton.interactable = false;
        BigButton.interactable = true;
        ScalePop.SetActive(false);
        Mixer.transform.localScale = new Vector3(Midium, Midium, Midium);

        if(isSmall)
        {
            PositionY = (Mixer.transform.position.y) + PtoM;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }
        if (isBig)
        {
            PositionY = (Mixer.transform.position.y) - MtoG;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }
        _alertsBehaviour.AndroidToastMessage("Tamaño mediano del modelo");
        isSmall = false;
        isBig = false;
    }

    public void ScaleBigListener()
    {
        isBig = true;
        SmallButton.interactable = true;
        MidiumButton.interactable = true;
        BigButton.interactable = false;
        ScalePop.SetActive(false);
        Mixer.transform.localScale = new Vector3(Big, Big, Big);

        if (isSmall)
        {
            PositionY = (Mixer.transform.position.y) + PtoG;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }
            
        if(isMidium)
        {
            PositionY = (Mixer.transform.position.y) + MtoG;
            Mixer.transform.position = new Vector3(Mixer.transform.position.x, PositionY, Mixer.transform.position.z);
        }
        _alertsBehaviour.AndroidToastMessage("Tamaño grande del modelo");
        isSmall = false;
        isMidium = false;
    }

    public void EditionModeOnListener()
    {
        EditPop.SetActive(true);
        isEditionMode = true;
        _alertsBehaviour.AndroidToastMessage("Toque el objeto para poder mover o rotar");
        EditMode.SetActive(true);
        ResizeImage.color = new Color(1, 0, 0, 1);
    }
    public void EditionModeOffListener()
    {
        isEditionMode = false;
        _alertsBehaviour.AndroidToastMessage("Modo edición apagado");
        EditMode.SetActive(false);
        ResizeImage.color = new Color(1, 1, 1, 1);
    }
    public void HideEditionPopListener()
    {
        EditAnimation.Play("EditButton");
        EditPop.SetActive(false);
    }
    
    void OnDisable ()
    {
        PatchTimeOpearion(timeOnOperation);
        print("Disable");
    }

    private void OnDestroy()
    {
        PatchTimeOpearion(timeOnOperation);
        print("Destroy");
    }
}
