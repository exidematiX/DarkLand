Shader"Custom/UnlitColorSRP"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1) // 颜色属性
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
Name"Pass0"
            Tags
{"LightMode"="UniversalForward"
} // 表明这是一个URP的通道

ZWrite Off

Cull Off

Blend SrcAlpha
OneMinusSrcAlpha // 开启透明度混合

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct appdata
{
    float4 vertex : POSITION;
};

struct v2f
{
    float4 pos : SV_POSITION;
};

float4 _Color;

v2f vert(appdata v)
{
    v2f o;
    o.pos = TransformObjectToHClip(v.vertex);
    return o;
}

half4 frag(v2f i) : SV_Target
{
    return _Color; // 输出颜色
}
            ENDHLSL
        }
    }

FallBack"Unlit/Color"
}
