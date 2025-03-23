using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlienAttentionHandler 
{
    //This may come bite me in the ass later on but it's so convienient that I'm gonna use it
    static AlienController controller = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();

    static int CheckForWallDampening(int attentionIncrease, Vector3 noisePosition, Vector3 alienPosition)
    {
        RaycastHit[] hits;
        LayerMask wallMask = LayerMask.GetMask("Surfaces");

        hits = Physics.RaycastAll(noisePosition,
            (alienPosition - noisePosition).normalized,
            Vector3.Distance(noisePosition, alienPosition),
            wallMask);

        int dampenedAttention = hits.Length > 0 ? (int)(attentionIncrease * (0.5 / hits.Length)) : attentionIncrease;
        Debug.Log("# walls: " + hits.Length + " | original attention: " + attentionIncrease + " | modified attention: " + dampenedAttention);

        return dampenedAttention;
    }

    public static void NoiseToAttentionIncrease(int noiseCoefficent, Vector3 noisePosition)
    {
        int attention = (int)(noiseCoefficent * (noiseCoefficent / Vector3.Distance(controller.transform.position, noisePosition)));
        attention = CheckForWallDampening(attention, noisePosition, controller.transform.position);
        attention = Mathf.Clamp(attention, 0, 100);

        controller.IncreaseAttention(attention, noisePosition);
    }
}
