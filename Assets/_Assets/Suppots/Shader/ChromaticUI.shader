// Shader "UI/ChromaticSplit2D"
// {
//     Properties
//     {
//         _MainTex ("Sprite Texture", 2D) = "white" {}
//         _IntensityX ("Intensity X", Range(-1,1)) = 0
//         _IntensityY ("Intensity Y", Range(-1,1)) = 0
//         _Alpha ("Alpha", Range(0,1)) = 1
//         _Brightness ("Brightness", Range(0,2)) = 1   <-- thêm thuộc tính Brightness
//     }
//     SubShader
//     {
//         Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
//         Cull Off
//         Lighting Off
//         ZWrite Off
//         Blend SrcAlpha OneMinusSrcAlpha

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #include "UnityCG.cginc"

//             struct appdata_t {
//                 float4 vertex : POSITION;
//                 float4 color : COLOR;
//                 float2 texcoord : TEXCOORD0;
//             };

//             struct v2f {
//                 float4 pos : SV_POSITION;
//                 fixed4 color : COLOR;
//                 float2 uv : TEXCOORD0;
//             };

//             sampler2D _MainTex;
//             float4 _MainTex_ST;
//             float _IntensityX;
//             float _IntensityY;
//             float _Alpha;
//             float _Brightness; <-- biến Brightness

//             v2f vert (appdata_t v)
//             {
//                 v2f o;
//                 o.pos = UnityObjectToClipPos(v.vertex);
//                 o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
//                 o.color = v.color;
//                 return o;
//             }

//             fixed4 frag (v2f i) : SV_Target
//             {
//                 float2 dir = float2(_IntensityX, _IntensityY);

//                 sample 3 lần theo kênh R/G/B
//                 fixed4 sr = tex2D(_MainTex, i.uv + dir);
//                 fixed4 sg = tex2D(_MainTex, i.uv);
//                 fixed4 sb = tex2D(_MainTex, i.uv - dir);

//                 fixed4 col = fixed4(sr.r, sg.g, sb.b, (sr.a + sg.a + sb.a)/3.0);
//                 col.rgb *= _Brightness; <-- áp dụng Brightness vào kênh RGB
//                 col.a *= _Alpha;

//                 return col * i.color;
//             }
//             ENDCG
//         }
//     }
// }


Shader "UI/ChromaticSplit2D"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _IntensityX ("Intensity X", Range(-1,1)) = 0
        _IntensityY ("Intensity Y", Range(-1,1)) = 0
        _Alpha ("Alpha", Range(0,1)) = 1
        _Brightness ("Brightness", Range(0,2)) = 1
        _Reveal ("Reveal Amount", Range(0,1)) = 1 // <-- thêm tham số điều khiển
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _IntensityX;
            float _IntensityY;
            float _Alpha;
            float _Brightness;
            float _Reveal;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 dir = float2(_IntensityX, _IntensityY);

                // sample RGB split
                fixed4 sr = tex2D(_MainTex, i.uv + dir);
                fixed4 sg = tex2D(_MainTex, i.uv);
                fixed4 sb = tex2D(_MainTex, i.uv - dir);

                fixed4 col = fixed4(sr.r, sg.g, sb.b, (sr.a + sg.a + sb.a)/3.0);

                // Brightness
                col.rgb *= _Brightness;
                col.a *= _Alpha;

                // Reveal mask từ dưới lên
                float revealMask = step(_Reveal, 1.0 - i.uv.y);

                col *= revealMask;

                return col * i.color;
            }
            ENDCG
        }
    }
}
