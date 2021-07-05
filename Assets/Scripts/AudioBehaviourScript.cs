using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehaviourScript : MonoBehaviour
{
    [SerializeField] AudioSource NormalSound;
    [SerializeField] AudioSource BoostedSound;

    //Iniciar la mezcladora
    public void StartMixer()
    {
        //Si el sonido de la mezcladora está apagado, tanto normal como boosted
        if (!NormalSound.isPlaying && !BoostedSound.isPlaying)
        {
            //Iniciamos la mezcladora
            NormalSound.Play();
        }
    }

    //Detener la mezcladora
    public void StopMixer()
    {
        //Detenemos todos los sonidos
        NormalSound.Stop();
        BoostedSound.Stop();
    }

    public void BoostMixer()
    {
        //Si el sonido normal está sonando
        if (NormalSound.isPlaying)
        {
            //Detenemos el sonido normal
            NormalSound.Stop();

            //Iniciamos el sonido de incremento
            BoostedSound.Play();
        }
    }

    public void DecreaseSpeed()
    {
        //Si la velocidad está incrementada
        if (BoostedSound.isPlaying)
        {
            //Detenemos la velocidad incrementada
            BoostedSound.Stop();

            //Activamos el sonido de decremento
            NormalSound.Play();
        }
    }
}
