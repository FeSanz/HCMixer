using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiaColor : MonoBehaviour
{
    private bool esColor = false;
    private Renderer colorVolta;
    void Start()
    {
        colorVolta = GetComponent<Renderer>();
        colorVolta.material.SetColor("_Color", Color.white);

        esColor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cambiarcolor();
            Debug.Log("boton");
        }
    }
    private void cambiarcolor()
    {
        if (esColor == true)
        {
            colorVolta.material.SetColor("_Color", Color.yellow);
            esColor = false;
        }
        else
        {
            colorVolta.material.SetColor("_Color", Color.white);
            esColor = true;
        }
    }
}
