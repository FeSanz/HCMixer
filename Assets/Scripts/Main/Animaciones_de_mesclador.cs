using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones_de_mesclador : MonoBehaviour
{
    [SerializeField] Animator Tolva;
    [SerializeField] Animator MRojo;
    [SerializeField] Animator EngraneG;
    [SerializeField] Animator EngraneP;

    AlertsBehaviour _alertsBehaviour = new AlertsBehaviour();

    public void Inicio() {
        _alertsBehaviour.AndroidToastMessage("Mezclador encendido");
        print("Mezclador encendido");
        
        Tolva.Play("MixingblowINI");
        MRojo.Play("AgitadorDDM_INI");
        EngraneG.Play("EngraneG_INI");
        EngraneP.Play("EngraneP_INI");
    }

    public void Apagado()
    {
        _alertsBehaviour.AndroidToastMessage("Mezclador apagado");
        print("Mezclador apagado");
        
        Tolva.Play("MixingblowAP");
        MRojo.Play("AgitadorDDM_AP");
        EngraneG.Play("EngraneG_AP");
        EngraneP.Play("EngraneP_AP");
    }
    public void Lento()
    {
        Tolva.Play("MixingblowLEN");
        MRojo.Play("AgitadorDDM_LEN");
        EngraneG.Play("EngraneG_LEN");
        EngraneP.Play("EngraneP_");
    }

    public void Rapido()
    {
        Tolva.Play("MixingBlow");
        MRojo.Play("AgitadorDDM");
        EngraneG.Play("EngraneG_INI");
        EngraneP.Play("EngraneP_INI");
    }
}
