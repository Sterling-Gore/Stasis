using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTriggerDarkFigure : MonoBehaviour
{
    public GameObject player;
    public ManagerDarkFigure managerDarkFigure;
    public Collider FigureCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(player.transform.position + new Vector3(0,1,0), player.transform.forward);
        Debug.DrawRay(player.transform.position + new Vector3(0,1,0), player.transform.forward * 10f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            //Debug.Log("YERRRRRR");
            Collider otherCollider = hit.collider.GetComponent<Collider>();
            if(otherCollider != null && FigureCollider == otherCollider)
            {
                FigureCollider.enabled = false;
                StartCoroutine(despawnFigure());
            }
        }
    }

    IEnumerator despawnFigure()
    {
        managerDarkFigure.PlayScaryAudio();
        yield return new WaitForSeconds(.75f);
        managerDarkFigure.despawnFigure();
    }
}
