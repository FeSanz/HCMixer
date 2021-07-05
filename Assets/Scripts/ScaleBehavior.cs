using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    private AlertsBehaviour _alertsBehaviour = new AlertsBehaviour();

    private void Start()
    {
        ResizeImage = GetComponent<Image>();
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
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else 
                Application.Quit();
        #endif
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
}
