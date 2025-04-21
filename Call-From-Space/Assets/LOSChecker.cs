using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Device;
using Screen = UnityEngine.Screen;

public class LOSChecker : MonoBehaviour
{
    public bool playerSeesMonster;
    bool switchFlag;

    [SerializeField]
    float maximumLOSFocusAngle;

    Camera mainCamera;
    LayerMask wallMask;
    ZoomCamera zoom;
    Renderer darkFigureRenderer;

    [SerializeField]
    Transform figureCenter;

    Coroutine lookingInsanityIncreaseCoroutine;

    private void Start()
    {
        darkFigureRenderer = GetComponent<Renderer>();
        mainCamera = FindObjectOfType<Camera>();
        wallMask = LayerMask.GetMask("Surfaces");
        zoom = FindObjectOfType<ZoomCamera>();
        switchFlag = false;
        StartCoroutine(CheckIfLooking());
    }

    private void Update()
    {
        if (playerSeesMonster && switchFlag)
        {
            zoom.StartZoomIn();
            lookingInsanityIncreaseCoroutine = StartCoroutine(LookingInsanityIncrease());
            switchFlag = !switchFlag;
        }
        else if(!playerSeesMonster && !switchFlag)
        {
            zoom.StartZoomOut();
            if (lookingInsanityIncreaseCoroutine != null) StopCoroutine(lookingInsanityIncreaseCoroutine);
            switchFlag = !switchFlag;
        }
    }

    private IEnumerator CheckIfLooking()
    {
        //yield return new WaitForSeconds(0.2f);

        while(true)
        {
            yield return new WaitForSeconds(0.1f);


            if (hasWallsInPath(figureCenter.position) || !isCameraFocused())
            {
                if (!playerSeesMonster)
                {
                    yield return new WaitForSeconds(0.2f); //buffer for first focus on monster
                }
                playerSeesMonster = false;
                continue;
            }
            playerSeesMonster = true;
        }  
    }


    bool isCameraFocused()
    {
        float angleBetweenCameraAndMonster = Vector3.Angle(mainCamera.transform.rotation * Vector3.forward, figureCenter.position - mainCamera.transform.position);

        return angleBetweenCameraAndMonster < maximumLOSFocusAngle;
    }

    public bool isOnScreen(Vector3 position)
    {
        Vector2 screenPos = mainCamera.WorldToScreenPoint(position);
        float angleBetweenCameraAndPosition = Vector3.Angle(mainCamera.transform.rotation * Vector3.forward, position - mainCamera.transform.position);
        
        return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y  > 0f && screenPos.y  < Screen.height 
            && !hasWallsInPath(position) 
            && angleBetweenCameraAndPosition < 90;
    }

    public bool hasWallsInPath(Vector3 position)
    {
        Vector3 cameraPosition = mainCamera.transform.position;

        return Physics.Raycast(cameraPosition, position - cameraPosition, Vector3.Distance(cameraPosition, position), wallMask);
    }

    IEnumerator LookingInsanityIncrease()
    {
        while (true)
        {
            InsanityMeter.Instance.IncreaseInsanity(10f);
            yield return new WaitForSeconds(1f);
        }
    }
}
