Shader"Project/Normal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            struct verdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (verdata v)
            {
                v2f o;

                //o.normal = TransformObjectToWorldNormal(v.normal)

                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.normal= UnityObjectToWorldNormal(v.normal);
                o.normal= (v.normal);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //i.normal

                fixed4 col = tex2D(_MainTex, i.uv);
                return float4(i.normal,1);
            }
            ENDCG
        }
    }
}
