using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [Header("Shadow Figure Zoom")]
    public float maxFov;
    public float minFov;
    public int zoomIntervals;

    Coroutine zoomInCoroutine;
    Coroutine zoomOutCoroutine;

    Camera mainCamera;
    MoveCamera camMove
        ;
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        camMove = GetComponent<MoveCamera>();
        zoomInCoroutine = null;
        zoomOutCoroutine = null;
    }

    public void StartZoomOut()
    {
        if (zoomOutCoroutine == null)
        {
            if (zoomInCoroutine != null)
            {
                StopCoroutine(zoomInCoroutine);
                zoomInCoroutine = null;
            }
            zoomOutCoroutine = StartCoroutine(ZoomOut());
        }
    }

    public void StartZoomIn()
    {
        if (zoomInCoroutine == null)
        {
            if (zoomOutCoroutine != null)
            {
                StopCoroutine(zoomOutCoroutine);
                zoomOutCoroutine = null;
            }
            zoomInCoroutine = StartCoroutine(ZoomIn());
        }
    }

    IEnumerator ZoomIn()
    {
        float fovDecrease = (maxFov - minFov) / zoomIntervals;

        while (mainCamera.fieldOfView > minFov)
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - fovDecrease, minFov, maxFov);
            yield return new WaitForFixedUpdate();
        }

        zoomInCoroutine = null;
    }

    IEnumerator ZoomOut()
    {
        float fovIncrease = (maxFov - minFov) / zoomIntervals;

        while (mainCamera.fieldOfView < maxFov)
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView + fovIncrease, minFov, maxFov);
            yield return new WaitForFixedUpdate();
        }

        zoomOutCoroutine = null;
    }
}
