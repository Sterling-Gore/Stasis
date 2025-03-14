//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SoundTriggerController : MonoBehaviour
//{
//    public SoundType currentSoundType;
//    public SoundType startingSoundType;

//    int soundTypeValue;
//    GameObject pathfinder;
//    AlienController pathfindingController;

//    [Header("Testing stuff")]
//    public bool crouchOnAlert;
//    public bool crouchOnHunt;



//    IEnumerator InSoundBubbleHolder;
//    private void Start()
//    {
//        pathfindingController = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();
//        changeSoundType(startingSoundType);
//    }

//    public void changeSoundType(SoundType target)
//    {
//        currentSoundType = target;
//        soundTypeValue = (int)target;
//    }

    

//    void OnTriggerEnter(Collider other)
//    {

//        if (other.tag != "Alien") return;

//        InSoundBubbleHolder = InSoundBubble();

//        StartCoroutine(InSoundBubbleHolder);

//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.tag != "Alien") return;

//        StopCoroutine(InSoundBubbleHolder);
//    }

//    //Reduces attention gain by 50% for each wall between the sound and the alien
//    int checkForWallDampening(int attentionIncrease, Vector3 alienPosition)
//    {
        

//        RaycastHit[] hits;
//        LayerMask wallMask = LayerMask.GetMask("Surfaces");

//        hits = Physics.RaycastAll(this.transform.position, 
//            (alienPosition - this.transform.position).normalized,
//            Vector3.Distance(this.transform.position,alienPosition), 
//            wallMask);
        
//        int dampenedAttention = hits.Length > 0 ? (int) (attentionIncrease * (0.5 / hits.Length)) : attentionIncrease;
//        Debug.Log("# walls: " + hits.Length + " | original attention: " + attentionIncrease + " | modified attention: " + dampenedAttention);

//        return dampenedAttention;
//    }
//    IEnumerator InSoundBubble()
//    {
//        var wait = new WaitForSeconds(1);
        
//        while (true)
//        {
//            Vector3 alienPosition = pathfindingController.gameObject.transform.position;
//            //just some random equation i made so attention it realisticly hears u more when you're are closer
//            //int attentionIncrease = soundTypeValue - ((int)Math.Pow(Vector3.Distance(alienPosition, transform.position), 2)) / (5*soundTypeValue);
//            int attentionIncrease = (int) (soundTypeValue * (soundTypeValue / Vector3.Distance(alienPosition, transform.position)));
//            attentionIncrease = checkForWallDampening(attentionIncrease, alienPosition);

//            if ((attentionIncrease >= 30 || pathfindingController.CurrentAttention > 50) && currentSoundType != SoundType.Still && crouchOnAlert)
//            {
//                changeSoundType(SoundType.Still);
//            }

//            if ((pathfindingController.CurrentAttention == 100) && currentSoundType != SoundType.Still && crouchOnHunt)
//            {
//                changeSoundType(SoundType.Still);
//            }

//            attentionIncrease = Mathf.Clamp(attentionIncrease, 0, 100);
//            //Debug.Log(attentionIncrease);
//            pathfindingController.IncreaseAttention(attentionIncrease, this.transform.position);
//            yield return wait;
//        }
//    }
//}
