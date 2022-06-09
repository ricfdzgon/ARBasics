using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ARInteractionManager : MonoBehaviour
{
    public Text debugText;
    public GameObject objectPrefab;
    public GameObject pointer;
    public Camera myCamera;

    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject currentObject;
    private bool isInitialPosition;
    private bool isOverCurrentObject;
    private bool isOverUI;

    private Vector2 initialTouchPose;

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
                isOverCurrentObject = IsOverCurrentObject(touchOne.position);
                isOverUI = IsOverUI(touchOne.position);
            }

            if (touchOne.phase == TouchPhase.Moved && isOverCurrentObject && !isOverUI)
            {
                raycastManager.Raycast(touchOne.position, hits, TrackableType.FeaturePoint);
                if (hits.Count > 0)
                {
                    transform.position = hits[0].pose.position;
                }
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
            {
                initialTouchPose = touchTwo.position - touchOne.position;
            }

            if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPose = touchTwo.position - touchOne.position;
                float angle = Vector2.SignedAngle(initialTouchPose, currentTouchPose);
                if (currentObject)
                {
                    currentObject.transform.Rotate(Vector3.up, angle);
                }
                initialTouchPose = currentTouchPose;
            }
        }
    }

    public bool IsOverCurrentObject(Vector2 position)
    {
        Ray ray = myCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return true;
        }

        return false;
    }

    public bool IsOverUI(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, result);

        return result.Count > 0;
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

    public void BlueButtonOnClick()
    {
        currentObject.transform.parent = null;
        currentObject = null;
        pointer.SetActive(false);
    }

    public void RedButtonOnClick()
    {
        if (currentObject)
        {
            Destroy(currentObject);
        }
    }
}
