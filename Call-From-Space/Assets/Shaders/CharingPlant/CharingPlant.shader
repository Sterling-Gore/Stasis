Shader "Custom/VineBurnProper"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BurnTex ("Burn Mask (RGB)", 2D) = "white" {}
        _BurnThreshold ("Burn Threshold", Range(0,1)) = 0
        _BurnColor ("Burn Color", Color) = (0,0,0,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.01
        _FadeNoiseTex ("Fade Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alphatest:_Cutoff addshadow

        sampler2D _MainTex;
        sampler2D _BurnTex;
        sampler2D _FadeNoiseTex;
        float _BurnThreshold;
        fixed4 _BurnColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BurnTex;
            float2 uv_FadeNoiseTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
            float burnMask = tex2D(_BurnTex, IN.uv_BurnTex).r;
            fixed4 finalColor;
            float alpha = 1.0;

            if (_BurnThreshold <= 0.5)
            {
                // 0.0 - 0.5: Gradually blend into BurnColor
                float burnProgress = saturate((_BurnThreshold / 0.5)); // 0 to 1
                float maskValue = burnMask;
                float burnLerp = saturate((burnProgress - maskValue) * 2.0); // Sharper transition
                burnLerp = pow(burnLerp, 3.0); // optional: smooth nonlinear

                finalColor = lerp(mainColor, _BurnColor, burnLerp);
                alpha = 1.0;
            }
            else
            {
                // After 0.5: fully BurnColor, start fading out
                finalColor = _BurnColor;

                float fadeProgress = saturate((_BurnThreshold - 0.5) * 2.0); // remap 0.5 -> 1.0 to 0 -> 1
                float noise = tex2D(_FadeNoiseTex, IN.uv_FadeNoiseTex).r;

                float noiseFade = saturate((fadeProgress - noise) * 5.0); // control how fast it fades
                alpha = 1.0 - noiseFade;
            }

            o.Albedo = finalColor.rgb;
            o.Alpha = alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
