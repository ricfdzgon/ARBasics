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
            currentObject.transform.position = transform.position;
            currentObject.transform.parent = transform;
            isInitialPosition = true;
        }
    }
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            raycastManager.Raycast(middleScreen, hits, TrackableType.FeaturePoint);

            if (hits.Count > 0)
            {
                //Colocamos o obxecto
                transform.position = hits[0].pose.position;
                debugText.text = debugText.text + " | " + "Punto 1";
                transform.rotation = hits[0].pose.rotation;
                debugText.text = debugText.text + " | " + "Punto 2";
                pointer.SetActive(true);
                debugText.text = debugText.text + " | " + "Punto 3";

                isInitialPosition = false;

                debugText.text = debugText.text + " | " + "Colocamos o obxecto";
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == TouchPhase.Began)
            {
                debugText.text = debugText.text + " | " + "Toque en " + touchOne.position;
            }
        }
    }

    public void GreenButtonOnClick()
    {
        debugText.text = debugText.text + " | Green Button Clicked";
        CurrentObject = Instantiate(objectPrefab);
    }

    public void BorrarDebugButtonOnClick()
    {
        debugText.text = "";
    }
}
