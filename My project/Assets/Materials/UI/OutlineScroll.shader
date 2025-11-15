using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEditor.ShaderData;

Shader "UI/OutlineScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        _Speed("Scroll Speed", Float) = 0.3
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off ZWrite Off ZTest Always Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
#pragma fragment frag
# include "UnityCG.cginc"

            struct appdata
{
    float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

struct v2f
{
    float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _Color;
float _Speed;

v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    float t = _Time.y * _Speed;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(t, 0);
    return o;
}

fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
return col;
            }
            ENDCG
        }
    }
}
