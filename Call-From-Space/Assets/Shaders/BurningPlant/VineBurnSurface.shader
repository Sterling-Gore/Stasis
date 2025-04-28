Shader "Custom/VineBurnSurfaceShaderFixed"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BurnTex ("Burn Mask (RGB)", 2D) = "white" {}
        _BurnThreshold ("Burn Threshold", Range(0,1)) = 0
        _BurnColor ("Burn Color", Color) = (0,0,0,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.01
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alphatest:_Cutoff addshadow

        sampler2D _MainTex;
        sampler2D _BurnTex;
        float _BurnThreshold;
        fixed4 _BurnColor;
        // _Cutoff automatically available.

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BurnTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float burnValue = tex2D(_BurnTex, IN.uv_BurnTex).r;

            float burnAmount = saturate((_BurnThreshold - burnValue) * 2.0);
            float burnLerp = pow(burnAmount, 3); // nonlinear

            fixed4 burnedColor = lerp(c, _BurnColor, burnLerp);

            o.Albedo = burnedColor.rgb;

            float alpha = 1.0 - burnLerp;
            o.Alpha = alpha; // <<< set alpha normally. Surface shader + alphatest handles it!
        }
        ENDCG
    }
    FallBack "Diffuse"
}
