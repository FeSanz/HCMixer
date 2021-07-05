using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensarBandaBehaviourScript : MonoBehaviour
{
    [SerializeField] Animator puerta3Animator;
    [SerializeField] Animator bandaAnimator;
    [SerializeField] Animator puerta2Animator;
    [SerializeField] Animator tornilloMR1Animator;
    [SerializeField] Animator tornilloMR2Animator;
    [SerializeField] Animator tornilloMR3Animator;
    [SerializeField] Animator tornilloMR4Animator;
    [SerializeField] Animator rctAnimator;
    [SerializeField] Transform flechaRCT;
    [SerializeField] ArrowBehaviourScript arrow;
    [SerializeField] PanelButtonBehaviourScript OnButton;
    [SerializeField] PanelButtonBehaviourScript OffButton;
    

    
    //Regresar todo a la normalidad
    public void Step0()
    {
        flechaRCT.gameObject.SetActive(false);
        puerta3Animator.Play("Puerta3_InitialState");
        puerta2Animator.Play("Puerta2_InitialState");
        bandaAnimator.Play("Banda_Initial");
        tornilloMR1Animator.Play("TornilloMR1_Initial");
        tornilloMR2Animator.Play("TornilloMR2_Initial");
        tornilloMR3Animator.Play("TornilloMR3_Initial");
        tornilloMR4Animator.Play("TornilloMR4_Initial");
        rctAnimator.Play("RCT_Initial");
        OffButton.DetenerParpadeo();
        OnButton.DetenerParpadeo();
    }

    //Apague el mezclador
    public void Step1()
    {
        rctAnimator.Play("RCT_InitialMovido");
        OffButton.IniciarParpadeo();
        arrow.PointToOffButton();
        puerta3Animator.Play("Puerta3_InitialState");
        puerta2Animator.Play("Puerta2_InitialState");
    }

    //Dirígete a las puertas señaladas
    public void Step2()
    {
        OffButton.DetenerParpadeo();
        arrow.gameObject.SetActive(true);
        arrow.PointToDoors();
        puerta3Animator.Play("Puerta3_Signal");
        puerta2Animator.Play("Puerta2_Signal");
        
    }

    //Abre las puertas
    public void Step3()
    {
        arrow.gameObject.SetActive(false);
        puerta3Animator.Play("Puerta3_Abrir");
        puerta2Animator.Play("Puerta2_Abrir");
        bandaAnimator.Play("Banda_Initial");
    }

    //Asegúrate que la banda esté detenida por completo
    public void Step4()
    {
        bandaAnimator.Play("Banda_Signal");
        tornilloMR1Animator.Play("TornilloMR1_Initial");
        tornilloMR2Animator.Play("TornilloMR2_Initial");
        tornilloMR3Animator.Play("TornilloMR3_Initial");
        tornilloMR4Animator.Play("TornilloMR4_Initial");
    }

    //Afloja los tornillos ubicados en la base del motorreductor
    public void Step5()
    {
        flechaRCT.gameObject.SetActive(false);
        bandaAnimator.Play("Banda_Initial");
        tornilloMR1Animator.Play("TornilloMR1_Aflojar");
        tornilloMR2Animator.Play("TornilloMR2_Aflojar");
        tornilloMR3Animator.Play("TornilloMR3_Aflojar");
        tornilloMR4Animator.Play("TornilloMR4_Aflojar");
        rctAnimator.Play("RCT_Initial");

    }

    //Mueve el motorreductor hacia la izquierda para tensar la banda
    public void Step6()
    {
        flechaRCT.gameObject.SetActive(true);
        tornilloMR1Animator.Play("TornilloMR1_Aflojado");
        tornilloMR2Animator.Play("TornilloMR2_Aflojado");
        tornilloMR3Animator.Play("TornilloMR3_Aflojado");
        tornilloMR4Animator.Play("TornilloMR4_Aflojado");
        rctAnimator.Play("RCT_Empujar");
    }

    //Asegura los tornillos nuevamente, sin dejar de tensar la banda
    public void Step7()
    {
        flechaRCT.gameObject.SetActive(false);
        rctAnimator.Play("RCT_Empujado");
        tornilloMR1Animator.Play("TornilloMR1_Asegurar");
        tornilloMR2Animator.Play("TornilloMR2_Asegurar");
        tornilloMR3Animator.Play("TornilloMR3_Asegurar");
        tornilloMR4Animator.Play("TornilloMR4_Asegurar");
        OnButton.DetenerParpadeo();
    }

    //Para iniciar operaciones, presione el botón para encender el mezclador
    public void Step8()
    {
        tornilloMR1Animator.Play("TornilloMR1_Initial");
        tornilloMR2Animator.Play("TornilloMR2_Initial");
        tornilloMR3Animator.Play("TornilloMR3_Initial");
        tornilloMR4Animator.Play("TornilloMR4_Initial");
        puerta3Animator.Play("Puerta3_Abrir");
        puerta2Animator.Play("Puerta2_Abrir");
        OnButton.IniciarParpadeo();
        arrow.PointToOnButton();
    }

    //Cierre las puertas.
    public void Step9()
    {
        OnButton.DetenerParpadeo();
        puerta3Animator.Play("Puerta3_Cerrar");
        puerta2Animator.Play("Puerta2_Cerrar");
    }
}
