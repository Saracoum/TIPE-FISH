Shader "Custom/fishAnim_vertFrag"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Phase ("phase", Float) = 0
        _Speed("Speed", Float) = 1
        _Period("Period", Float) = 1
        _FishSize("Fish Size", Float) = 1
        _MaxBendAngle("Max bend angle", Float) = 30
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            //Tags {"RenderType"="Opaque"}
            CGPROGRAM
            #pragma vertex vert addshadow
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };
            
            static const float PI = 3.14159265f;
            static const float degToRad = 3.14159265f / 180.0f;
            
            float _Phase;
            float _Speed;
            float _Period;
            float _FishSize;
            float _MaxBendAngle;
            
            float3 deformMesh( float3 v ) {
                
                float bendAngle = degToRad * _MaxBendAngle * sin( _Speed * (_Phase - v.y/ _Period) * 2*PI );
                bendAngle *= (v.x*v.x + v.y*v.y) / (_FishSize * _FishSize);
                
                float cosine = cos( bendAngle );
                float sinus = sin( bendAngle );
                v.x = v.x * cosine - v.y * sinus;
                v.y = v.x * sinus + v.y * cosine;
                
                return v;
            }
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos( deformMesh( v.vertex) );
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }
            
            
            
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }

        // shadow casting support
        //UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        
    }
}