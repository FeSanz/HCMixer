using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScanBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject MixerModel; 
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private Texture texturePlane;
    [SerializeField] private GameObject focus;
    [SerializeField] private RawImage VideoPlayerAnim;
    [SerializeField] private GameObject PlaceMixerButton;
    [SerializeField] private float ModelPositionY; //Indica el ajuste que se le debe dar al modelo en el eje "Y" respecto del plano dependiedo del tamaño del modelo
    
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Vector3 screenCenter;
    private Pose currentPose;

    public static bool placeObject = false;
    public static float PosYHit;
    
    private Transform camera;

    private float damping = 5.0f;

    void Start()
    {
        screenCenter.Set(Screen.width / 2, Screen.height / 2, 0);
        camera = Camera.main.transform;
    }

    void Update()
    {
        if (!placeObject)
        {
            if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon | TrackableType.Planes))
            {
                if (hits.Count > 0)
                {
                    focus.SetActive(true);
                    VideoPlayerAnim.enabled = false;
                    Ray ray = Camera.current.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        currentPose.position = hit.point;
                        focus.transform.position = currentPose.position;
                        PlaceMixerButton.SetActive(true);
                        PlaceMixerButton.GetComponent<Button>().enabled = true;
                    }
                    else
                    {
                        PlaceMixerButton.GetComponent<Button>().enabled = false;
                    }
                   
                }
            }
        }
    }

    private void InstantiateMixer(Pose hit)
    {
        /*MixerModel.SetActive(true);//Hacer visible el modelo en la escena después del toque
        MixerModel.transform.position = new Vector3(hit.position.x, (hit.position.y) + ModelPositionY, hit.position.z);// Posicionar al modelo en el lugar de toque del plano
        PosYHit = (hit.Pose.position.y) + ModelPositionY;
        MixerModel.transform.Rotate(0, ModelRotation, 0, Space.Self); // Compensar la rotación hitPose que se aleja del raycast       
        MixerModel.transform.Rotate(0, -90f, 0, Space.Self);
        var ancla = hit.Trackable.CreateAnchor(hit.Pose); // Cree un ancla para que ARCore rastree el objeto colocado.
        MixerModel.transform.parent = ancla.transform;// Hacer que el modelo sea hijo del ancla.
        IsPlaced = true;*/
    }

    public void PlaceObject()
    {
        Renderer renderer;
        placeObject = true; //Variables estatica que tiene origen en ScanBehavior.cs
        
        foreach (ARPlane plane in planeManager.trackables)
        {
            renderer = plane.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_NORMALMAP");
            renderer.material.EnableKeyword("_METALLICGLOSSMAP");
            renderer.material.SetTexture("_MainTex", texturePlane);
        }
        planeManager.enabled = false;
        
        focus.SetActive(false);
        PlaceMixerButton.SetActive(false);
        
        MixerModel.SetActive(true); //Hacer visible el modelo en la escena después del toque
        MixerModel.transform.position = new Vector3(currentPose.position.x, (currentPose.position.y) + ModelPositionY, currentPose.position.z); // Posicionar al modelo en el lugar de toque del plano
        PosYHit = (currentPose.position.y) + ModelPositionY;
        
        var lookPos = camera.position - MixerModel.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(0, 90, 0);
        
        MixerModel.transform.rotation = Quaternion.Slerp(MixerModel.transform.rotation, rotation, Time.deltaTime * damping);
        
        //MixerModel.transform.Rotate(0, ModelRotation, 0, Space.Self); // Compensar la rotación hitPose que se aleja del raycast       
        //MixerModel.transform.Rotate(0, -90f, 0, Space.Self);
    }

    public void DestroyStaticsValues()
    {
        placeObject = false;
        ScaleBehavior.isEditionMode = false;
    }
}
