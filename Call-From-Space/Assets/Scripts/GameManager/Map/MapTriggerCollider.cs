using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTriggerCollider : MonoBehaviour
{
    public MapManager mapManager;
    public GameObject map;
    public bool clearAll = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(clearAll)
                mapManager.ClearAllMaps();
            else
                mapManager.SetMap(map);

        }
    }
}

