using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class SelectedObjectBehaviour : LeanSelectableBehaviour
{
    [SerializeField] private GameObject SelectedIndicators;
    private AlertsBehaviour _alertsBehaviour = new AlertsBehaviour();
    protected override void OnSelect(LeanFinger finger)
    {
        if (ScaleBehavior.isEditionMode)
        {
            SelectedIndicators.SetActive(true);
            _alertsBehaviour.AndroidToastMessage("Objeto seleccionado, ahora puede mover y rotar");
        }
        
    }

    protected override void OnDeselect()
    {
        if (ScaleBehavior.isEditionMode)
        {
            SelectedIndicators.SetActive(false);
            _alertsBehaviour.AndroidToastMessage("Objeto deseleccionado");
        }
    }
}
