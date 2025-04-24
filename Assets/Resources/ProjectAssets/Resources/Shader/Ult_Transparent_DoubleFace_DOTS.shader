Shader "WJDR/Ult_Transparent_DoubleFace_DOTS"
{
 Properties {
	_TintColor ("Color", Color) = (0.5,0.5,0.5,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    _DisCutoff ("Distance Cutoff Modify", Range(0,0.005)) = 0
}
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
    LOD 100
	
	Cull Off
    Lighting Off

    Pass {
        HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
				
				float dis : TEXCOORD2; 
            };

            CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_ST;
			
            float4 _TintColor;
            fixed _Cutoff;
            fixed _DisCutoff;
            CBUFFER_END

            #if defined(UNITY_DOTS_INSTANCING_ENABLED)
                UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                    UNITY_DOTS_INSTANCED_PROP(float4, _TintColor)
                    UNITY_DOTS_INSTANCED_PROP(fixed, _Cutoff)
                    UNITY_DOTS_INSTANCED_PROP(fixed, _DisCutoff)
                UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

                // _TintColor = UNITY_ACCESS_DOTS_INSTANCED_PROP(float4, Color);
                // _Cutoff = UNITY_ACCESS_DOTS_INSTANCED_PROP(fixed, Range(0, 1));
                // _DisCutoff = UNITY_ACCESS_DOTS_INSTANCED_PROP(fixed, Range(0, 0.005));
            #endif

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				o.dis = length(worldPos - _WorldSpaceCameraPos);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
				col.rgb += _TintColor * 2 - 1;
                clip(col.a - _Cutoff + i.dis*_DisCutoff);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDHLSL
    }
}

}