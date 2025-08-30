Shader "UI/CircleMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0,1)) = 1
        _Feather ("Feather", Range(0.001,0.5)) = 0.05
        _Invert ("Invert (0=normal,1=invert)", Float) = 0
        _Alpha ("Alpha Fade", Range(0,1)) = 1    // thêm alpha chung
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Feather;
            float _Invert;
            float _Alpha;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);

                fixed4 col = tex2D(_MainTex, uv);

                // --- Radius mask như cũ ---
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 diff = uv - center;
                diff.x *= aspect;
                float dist = length(diff);

                float alpha = smoothstep(_Radius, _Radius - _Feather, dist);

                if (_Radius <= 0.0001) alpha = 0; 
                if (_Radius >= 1.0) alpha = 1;

                if (_Invert > 0.5) alpha = 1.0 - alpha;

                // Nhân thêm alpha tổng
                col.a *= alpha * _Alpha;

                return col;
            }
            ENDCG
        }
    }
}
