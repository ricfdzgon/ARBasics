using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{
    public Text debugText;
    public GameObject objectPrefab;
    public GameObject pointer;

    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject currentObject;
    private bool isInitialPosition;

    public GameObject CurrentObject
    {
        set
        {
            currentObject = value;
            currentObject.transform.position = pointer.transform.position;
            currentObject.transform.parent = pointer.transform;
            isInitialPosition = true;
        }
    }
    void Start()
    {

    }

    void Update()
    {
        Vector2 middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        raycastManager.Raycast(middleScreen, hits, TrackableType.FeaturePoint);
        debugText.text = hits.Count.ToString();

        //Ray ray = Camera.main.ScreenPointToRay(middleScreen);
    }

    public void GreenButtonOnClick()
    {
        debugText.text = debugText.text + " | Green Button Clicked";
    }
}
