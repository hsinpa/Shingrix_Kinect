Shader "Hsinpa/RepositionMaterial"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _MainTexAPosition("TexA Position", Vector) = (0.4, 0, 0, 0)
        _MainTexASize("TexA Size", Vector) = (0.2, 0.59259, 0, 0)
        _MainTexARePosition("TexA Reposition", Vector) = (0, 0, 0, 0)

        _MainTexBPosition("TexB Position", Vector) = (0.4, 0.59259, 0, 0)
        _MainTexBSize("TexB Size", Vector) = (0.2, 0.407, 0, 0)
        _MainTexBRePosition("TexB Reposition", Vector) = (0, 0, 0, 0)
        _MainTexBScale("TexB Scale", Vector) = (0.65625, 0.65625, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float2 _MainTexAPosition;
            float2 _MainTexASize;
            float2 _MainTexARePosition;

            float2 _MainTexBRePosition;
            float2 _MainTexBScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 ProcessTextureA(float2 uv) {
                fixed4 col = tex2D(_MainTex, uv + _MainTexAPosition);

                if (uv.y < 1.0 - _MainTexASize.y)
                    col *= 0;

                if (uv.x > _MainTexASize.x)
                    col *= 0;

                return col;
            }

            fixed4 ProcessTextureB(float2 uv) {

                //Scale
                float2 new_uv = (uv - 0.5) * _MainTexBScale + 0.5;

                //Reposition
                new_uv += _MainTexBRePosition * _MainTexBScale;

                fixed4 col = tex2D(_MainTex, new_uv);


                ////Cut bottom
                if (uv.y <  0.6885)
                    col *= 0;

                return col;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col_a = ProcessTextureA(i.uv);
                fixed4 col_b = ProcessTextureB(i.uv);

                return col_a + col_b;
            }
            ENDCG
        }
    }
}
