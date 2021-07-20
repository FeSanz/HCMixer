using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform OnButton;
    [SerializeField] Transform OffButton;
    [SerializeField] Transform BoostButton;
    [SerializeField] Transform DecreaseButton;
    [SerializeField] Transform Doors;


    private Transform PositionToPoint = null;

    // Update is called once per frame
    void Update()
    {
        if(PositionToPoint != null)
        {
            transform.LookAt(PositionToPoint.position);
            //transform.Rotate(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.z);
        }
    }

    public void PointToOnButton()
    {
        PositionToPoint = OnButton; 
    }

    public void PointToOffButton()
    {
        PositionToPoint = OffButton;
    }

    public void PointToBoostButton()
    {
        PositionToPoint = BoostButton;
    }

    public void PointToDecreaseButton()
    {
        PositionToPoint = DecreaseButton;
    }

    public void PointToDoors()
    {
        PositionToPoint = Doors;
    }


}
