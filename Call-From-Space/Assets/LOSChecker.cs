using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Device;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;
using Screen = UnityEngine.Screen;

public class LOSChecker : MonoBehaviour
{
    public bool playerSeesMonster;
    bool switchFlag;

    [SerializeField]
    float normalLOSFocusAngle;
    [SerializeField]
    float harmlessLOSFocusAngle;
    float currentFocusAngle;

    Camera mainCamera;
    LayerMask wallMask;
    ZoomCamera zoom;

    [SerializeField]
    Transform figureCenter;

    [Header("Static Effect")]
    [SerializeField]
    GameObject StaticUI;
    [SerializeField]
    AudioSource staticAudio;
    [SerializeField]
    AudioLowPassFilter staticAudioFilter;
    

    float staticTimer;
    Coroutine lookingInsanityIncreaseCoroutine;

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
        wallMask = LayerMask.GetMask("Surfaces");
        zoom = FindObjectOfType<ZoomCamera>();
        switchFlag = false;
        
    }
    private void Start()
    {
        StartCoroutine(CheckIfLooking());
        StartCoroutine(StaticBasedOnSight());
    }

    private void Update()
    {
        bool isHarmless = transform.parent.GetComponent<DarkFigureController>().isHarmless;
        if (isHarmless)
        {
            currentFocusAngle = harmlessLOSFocusAngle;
            StaticUI.SetActive(false);
        }
        else
        {
            currentFocusAngle = normalLOSFocusAngle;
            StaticUI.SetActive(true);
        }
        
        if (playerSeesMonster && switchFlag)
        {
            if (isHarmless)
            {
                transform.parent.GetComponent<DarkFigureController>().SendToTimeout(15f);
                return;
            }
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
                playerSeesMonster = false;
                staticTimer = 0f;
                continue;
            }
            //if (!playerSeesMonster)
            //{
            //    yield return new WaitForSeconds(0.2f); //buffer for first focus on monster
            //}
            playerSeesMonster = true;
        }  
    }


    bool isCameraFocused()
    {
        float angleBetweenCameraAndMonster = Vector3.Angle(mainCamera.transform.rotation * Vector3.forward, figureCenter.position - mainCamera.transform.position);

        return angleBetweenCameraAndMonster < currentFocusAngle;
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

    IEnumerator StaticBasedOnSight()
    {
        

        RawImage staticMaterial = StaticUI.GetComponent<RawImage>();
        Color materialColor = staticMaterial.color;
        while (isActiveAndEnabled)
        {
            if (transform.parent.GetComponent<DarkFigureController>().hunting 
                || !InsanityMeter.Instance.acceptingInsanityIncrease 
                || !transform.parent.GetComponent<DarkFigureController>().enabled
                || transform.parent.GetComponent<DarkFigureController>().inTimeout)
            {
                yield return new WaitForFixedUpdate();
                staticAudio.volume = 0;
                staticMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0f);
                continue;
            }

            if (playerSeesMonster)
                staticTimer += Time.deltaTime;
            if (staticTimer > 3f)
                transform.parent.GetComponent<DarkFigureController>().SendToTimeout(7f);

            //Debug.Log(staticTimer);
            float angleBetweenCameraAndCenter = Vector3.Angle(mainCamera.transform.rotation * Vector3.forward, figureCenter.position - mainCamera.transform.position);

            float alpha = Mathf.Lerp(0f, 0.02f, (50 - angleBetweenCameraAndCenter) / 50) + staticTimer / 30f;
            staticAudio.volume = Mathf.Lerp(0.2f, 1f, (50 - angleBetweenCameraAndCenter) / 50);
            staticAudioFilter.lowpassResonanceQ = Mathf.Lerp(0f, 7f, (50 - angleBetweenCameraAndCenter) / 50) + staticTimer;

            staticMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);

            

            yield return new WaitForFixedUpdate();
        }
        
    }
}
