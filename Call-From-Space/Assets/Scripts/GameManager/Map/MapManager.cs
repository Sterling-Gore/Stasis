using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] maps;
    public GameObject[] UImaps;
    public GameObject PlayerPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearAllMaps()
    {
        PlayerPoint.SetActive(false);
        for (int count = 0; count < maps.Length; count++)
        {
            maps[count].SetActive(false);
            UImaps[count].SetActive(false);
            
        }
    }

    public void SetMap(GameObject map)
    {
        ClearAllMaps();
        for (int count = 0; count < maps.Length; count++)
        {
            if (maps[count] == map)
            {
                PlayerPoint.SetActive(true);
                maps[count].SetActive(true);
                UImaps[count].SetActive(true);
                break;
            }
        }
    }
}
