using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    Coroutine lookingInsanityIncreaseCoroutine;

    private void Start()
    {
        darkFigureRenderer = GetComponent<Renderer>();
        mainCamera = FindObjectOfType<Camera>();
        wallMask = LayerMask.GetMask("Surfaces");
        zoom = FindObjectOfType<ZoomCamera>();
        switchFlag = false;
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

    private IEnumerator OnBecameVisible()
    {
        yield return new WaitForSeconds(0.2f);

        while(darkFigureRenderer.isVisible)
        {
            yield return new WaitForSeconds(0.1f);

            if (hasWallsInPath() || !isCameraFocused())
            {
                playerSeesMonster = false;
                continue;
            }
            playerSeesMonster = true;
        }  
    }

    private void OnBecameInvisible()
    {
        playerSeesMonster = false;
    }

    bool isCameraFocused()
    {
        float angleBetweenCameraAndMonster = Vector3.Angle(mainCamera.transform.rotation * Vector3.forward, transform.position - mainCamera.transform.position);
        return angleBetweenCameraAndMonster < maximumLOSFocusAngle;
    }

    public bool isOnScreen() 
    {
        Vector2 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y  > 0f && screenPos.y  < Screen.height;
    }

    bool hasWallsInPath()
    {
        Vector3 cameraPosition = mainCamera.transform.position;

        return Physics.Raycast(cameraPosition, transform.position - cameraPosition, Vector3.Distance(cameraPosition, transform.position), wallMask);
    }

    IEnumerator LookingInsanityIncrease()
    {
        while (true)
        {
            InsanityMeter.Instance.IncreaseInsanity(10f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
