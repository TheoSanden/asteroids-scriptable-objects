Shader "Unlit/ReplaceColor"
{
    Properties
    {
        //Camera Blit Map
        _MainTex("Texture", 2D) = "white" {}
        _StandardLut ("Standard Lut", 2D) = "white" {}
        _LutWidth("Lut Width",Int) = 0
       // _ReplacementPosition("Replacement Position", Range(0,5)) = 0
        _ReplacementLut ("Replacement Lut",2D) = "white" {}
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
            sampler2D _StandardLut;
            sampler2D _ReplacementLut;
            float4 _MainTex_ST;
            float _LutWidth;
            //float _ReplacementPosition;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 replacementUV;
                fixed4 col = tex2D(_MainTex, i.uv);
                for(int i = 0; i < _LutWidth; i++)
                {
                    replacementUV = float2((float)i / _LutWidth, 0)- 0.001;
                    fixed4 sCol = tex2D(_StandardLut, replacementUV);
                    //Float compare
                    half3 delta = abs(col.rgb - sCol.rgb);
                    float deltaLength = length(delta);
                    if(deltaLength < 0.1)
                    {
                        col = tex2D(_ReplacementLut, replacementUV);
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
