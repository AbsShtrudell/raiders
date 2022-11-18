// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
 
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            sampler2D _CameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenSpace : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.screenSpace = ComputeScreenPos(o.vertex);
                return o;
            }

            float CorrectDepth(float rawDepth)
            {
                float persp = Linear01Depth(rawDepth);
                float ortho = (_ProjectionParams.z - _ProjectionParams.y) * (1 - rawDepth) + _ProjectionParams.y;
                return lerp(persp, ortho, unity_OrthoParams.w);
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float2 screenSpaceUV = i.screenSpace.xy; //perspective
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenSpaceUV);
                return fixed4(depth, 0, 0, 1);
            }

            ENDCG
        }
    }
        FallBack "VertexLit"
}