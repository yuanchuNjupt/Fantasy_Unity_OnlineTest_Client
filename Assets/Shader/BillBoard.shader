Shader "Unlit/BillBoard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" "DisableBatching" = "True" }

        
        ZWrite Off
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"



            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert (appdata_full v)
            {
                v2f o;

                float3 center = float3(0,0,0);
                float3 newZ = normalize(ObjSpaceViewDir(float4(center,1)));
                float3 newX = normalize(cross(newZ, float3(0,1,0)));
                float3 newY = normalize(cross(newX, newZ));
                
                //垂直轴向只需把Z轴y分量压为0即可
                

                float3 WorldZeroPos = float3(0,0,0);
                
                float3 offset = v.vertex.xyz - WorldZeroPos;
                
                float3 newPos = newX * offset.x + newY * offset.y + newZ * offset.z;
                
                o.vertex = UnityObjectToClipPos(newPos);
                

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                return col;
            }
            ENDCG
        }
    }
}
