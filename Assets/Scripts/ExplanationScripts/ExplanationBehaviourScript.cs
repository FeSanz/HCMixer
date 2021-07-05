using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class ExplanationBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform NextButton;
    [SerializeField] Transform BackButton;
    [SerializeField] TextMeshProUGUI InstructionsTMPUGUI;
    [SerializeField] Transform InstructionsPanel, ButtonsPanel;
    [SerializeField] Transform Mixer;

    private TensarBandaBehaviourScript tbbs;

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
        else
        {
            j = 0;
            InstructionsPanel.gameObject.SetActive(false);
            ButtonsPanel.gameObject.SetActive(false);
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
