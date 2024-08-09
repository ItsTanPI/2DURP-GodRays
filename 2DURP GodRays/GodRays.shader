Shader "Custom/CombineWithGodRays"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Count("Number of Lights", Int) = 1
        _Density ("Density", Float) = 100.0
        _Calculations("Calculations", Float) = 64
        _Weight ("Weight", Float) = 0.5
        _Decay ("Decay", Float) = 0.95
        _Exposure ("Exposure", Float) = 0
    }
    SubShader
    {
        Pass
        {
            Name "GodRays"
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_ST;
            int _Count;
            float4 _LightPosition[5];
            float _Width[5];
            float _Height[5];
            
            float4 _Color[5];
            float _Dependent;
            float _Density;
            float _Calculations;
            float _Weight;
            float _Decay;
            float _Exposure;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            bool isRectangle(float2 uv, float a)
            {             
                float Width = _Width[a];
                float Height = _Height[a];

                return (uv.x >= (_LightPosition[a].x - Width) && uv.x <= (_LightPosition[a].x + Width) 
                && uv.y >= (_LightPosition[a].y - Height) && uv.y <= (_LightPosition[a].y + Height));
            }

            float4 ConvertNonWhiteToBlack(float2 uv, float4 color, float a)
            {

                if(isRectangle(uv, a))
                {
                    if (color.r != 1.0 || color.g != 1.0 || color.b != 1.0)
                    {
                        return float4(0.0, 0.0, 0.0, color.a);
                    }
                    
                    return _Color[a];
                }
                else
                {
                    return float4(0.0, 0.0, 0.0, color.a);
                }
            }

            float4 ToBlack(float4 color)
            {
                if (color.r != 1.0 || color.g != 1.0 || color.b != 1.0)
                {
                   return float4(0.0, 0.0, 0.0, color.a);
                }
                return color;
            }

            float4 ApplyGodRays(float2 uv, float4 color, int a)
            {
                float2 deltaTextCoord = uv - _LightPosition[a].xy;
                deltaTextCoord *= 1.0 / _Density;
                float2 textCoo = uv;
                float illuminationDecay = 1.0;
                for (int i = 0; i < _Calculations; i++)
                {
                    textCoo -= deltaTextCoord;
                    float4 sample = tex2D(_MainTex, textCoo);
                    sample = ConvertNonWhiteToBlack(textCoo, sample, a);
                    sample *= illuminationDecay * _Weight;
                    color += sample;
                    illuminationDecay *= _Decay;
                }
                color *= _Exposure;
                return color;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 godRaysColor = {0, 0, 0, 0};
                float4 BaseColor = tex2D(_MainTex, i.uv);
                for(int a = 0; a < _Count; a++)
                {
                    float4 processedColor = ConvertNonWhiteToBlack(i.uv, BaseColor, a);
                    godRaysColor += ApplyGodRays(i.uv, processedColor, a);
                }
                return godRaysColor;
            }
            ENDCG
        }

        Pass
        {
            Name "Combine"
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_combine

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _ProcessedTex;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            float4 frag_combine(v2f i) : SV_Target
            {
                float4 mainTexCol = tex2D(_MainTex, i.uv);
                float4 processedTexCol = tex2D(_ProcessedTex, i.uv);
                // Combine the main texture and processed texture
                //return lerp(mainTexCol, processedTexCol, 0.5); // Example blend
                return mainTexCol + processedTexCol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}