//https://danielilett.com/2020-02-12-tut3-7-vintage-video/ 
Shader "UI/ScanlineShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _ScanlineTex ("Scanline Tex", 2D) = "white" {}
        _Strength ("Strength", Float) = 0.5
        _Size ("Size", float) = 1

        _InterferenceTex ("Interference Tex", 2D) = "white" {}
        _Speed ("Speed", float ) = 1
        _Extra ("Extra thing", float ) = 0.05
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            sampler2D _ScanlineTex;
            float _Strength;
            float _Size;

            sampler2D _InterferenceTex;
            float _Speed;
            float _Extra;

            v2f vert(appdata v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.uv = TRANSFORM_TEX(v.uv, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            // Generate time-sensitive random numbers between 0 and 1.
            float rand(float2 pos)
            {
                return frac(sin(dot(pos + _Time.y,
                                    float2(12.9898f, 78.233f))) * 43758.5453123f);
            }

            // Generate a random vector on the unit circle.
            float2 randUnitCircle(float2 pos)
            {
                const float PI = 3.14159265f;
                float randVal = rand(pos);
                float theta = 2.0f * PI * randVal;

                return float2(cos(theta), sin(theta));
            }

            // Quintic interpolation curve.
            float quinterp(float2 f)
            {
                return f * f * f * (f * (f * 6.0f - 15.0f) + 10.0f);
            }

            // Perlin gradient noise generator.
            float perlin2D(float2 pixel)
            {
                float2 pos00 = floor(pixel);
                float2 pos10 = pos00 + float2(1.0f, 0.0f);
                float2 pos01 = pos00 + float2(0.0f, 1.0f);
                float2 pos11 = pos00 + float2(1.0f, 1.0f);

                float2 rand00 = randUnitCircle(pos00);
                float2 rand10 = randUnitCircle(pos10);
                float2 rand01 = randUnitCircle(pos01);
                float2 rand11 = randUnitCircle(pos11);

                float dot00 = dot(rand00, pos00 - pixel);
                float dot10 = dot(rand10, pos10 - pixel);
                float dot01 = dot(rand01, pos01 - pixel);
                float dot11 = dot(rand11, pos11 - pixel);

                float2 d = frac(pixel);

                float x1 = lerp(dot00, dot10, quinterp(d.x));
                float x2 = lerp(dot01, dot11, quinterp(d.x));
                float y = lerp(x1, x2, quinterp(d.y));

                return y;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half4 color = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                float2 scanlineUV = i.uv * (_ScreenParams.y / _Size);

                fixed4 col = color;
                fixed4 scanlines = tex2D(_ScanlineTex, scanlineUV);

                float2 pos = i.uv * _ScreenParams.xy / 2.0f;
                float2 offset = float2(perlin2D(pos), perlin2D((pos + 0.5f)));

                float2 interferenceUV = i.uv.y + offset * 0.05f + _Time.y * _Speed;
                float interference = tex2D(_InterferenceTex, interferenceUV);

                col = lerp(col, col * scanlines, _Strength);
                col.a = lerp(1.0, 0.0, col.r);

                col = lerp(col, 1.0f, interference);
                return col;
            }
            ENDCG
        }
    }
}