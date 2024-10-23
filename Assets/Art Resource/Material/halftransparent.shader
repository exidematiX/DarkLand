Shader"Custom/SimpleTransparentShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1) // 定义颜色属性，包括透明度
        _MainTex ("Base (RGB)", 2D) = "white" {} // 主纹理
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" } // 设置渲染队列为透明
//LOD200
        
        Blend
SrcAlpha OneMinusSrcAlpha // 使用 alpha 混合模式，实现半透明

Cull Off // 关闭剔除，让正反面都可见

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
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};

sampler2D _MainTex;
float4 _Color;

v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex); // 将顶点转换为屏幕空间
    o.uv = v.uv; // 传递 UV 坐标到片元着色器
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed4 texColor = tex2D(_MainTex, i.uv); // 获取纹理颜色
    return texColor * _Color; // 将纹理颜色与 _Color 混合，实现透明度控制
}
            ENDCG
        }
    }
    
FallBack"Diffuse"
}