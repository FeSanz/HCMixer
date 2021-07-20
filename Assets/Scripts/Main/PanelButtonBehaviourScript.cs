using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelButtonBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject arrow;

    private bool estaParpadeando = false;
    private Image buttonImage;
    private IEnumerator _co;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = this.GetComponent<Image>();
        _co = Parpadeo();
    }

    public void IniciarParpadeo()
    {
        arrow.SetActive(true);
        StartCoroutine(_co);
    }

    public void DetenerParpadeo()
    {
        arrow.SetActive(false);
        StopCoroutine(_co);
        buttonImage.color = Color.clear;
    }

    public IEnumerator Parpadeo()
    {
        while (true)
        {
            buttonImage.color = Color.yellow;
            yield return new WaitForSeconds(1);
            buttonImage.color = Color.clear;
            yield return new WaitForSeconds(1);
        }
    }
}
