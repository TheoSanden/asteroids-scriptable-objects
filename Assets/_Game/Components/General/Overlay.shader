Shader "Unlit/Overlay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Overlay("Overlay", 2D) = "white" {}
        _XOffset("X Offset", Range(0.0,1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Overlay;
            float4 _MainTex_ST;
            float _XOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv +  (_XOffset, 0);
                if (col.a < 0.01f) { discard; }
                fixed4 overlay = tex2D(_Overlay, uv);
                if(overlay.a < 0.01f)
                {
                    return col;
                }
                return overlay;
            }
            ENDCG
        }
    }
}
