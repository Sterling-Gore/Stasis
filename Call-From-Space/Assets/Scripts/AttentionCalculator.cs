using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttentionCalculator 
{
    static int checkForWallDampening(int attentionIncrease, Vector3 noisePosition, Vector3 alienPosition)
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

    public static int calculateAttention(int noiseCoefficent, Vector3 noisePosition, Vector3 alienPosition)
    {
        int attention = (int)(noiseCoefficent * (noiseCoefficent / Vector3.Distance(alienPosition, noisePosition)));
        attention = checkForWallDampening(attention, noisePosition, alienPosition);
        attention = Mathf.Clamp(attention, 0, 100);

        return attention;
    }
}
