Shader "Custom/fishAnimLimit_surf"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Phase ("phase", Float) = 0
        _Speed("Speed", Float) = 1
        _Period("Period", Float) = 1
        _FishSize("Fish Size", Float) = 1
        _MaxBendAngle("Max bend angle", Float) = 30
        _MaxTranslation("Max translation", Float) = 1
        _TransitionHigh("transition high", float) = 1
        _TransitionLow("transition low", float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Standard vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        
        
        
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        static const float PI = 3.14159265f;
        static const float degToRad = 3.14159265f / 180.0f;
            
        float _Phase;
        float _Speed;
        float _Period;
        float _FishSize;
        float _MaxBendAngle;
        
        float _MaxTranslation;
        
        float _TransitionHigh;
        float _TransitionLow;
        
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        
        
        
        float3 deformMesh( float3 v ) {
            
            /*if ( v.y > _HeightLimit ) {
                return v;
            }*/
            
            float localWeight = sin( (_Speed * _Phase - v.z/ _Period) * 2*PI );
            float3 trRotation = v;
            float3 trSin = v;
            float trRotationWeight = lerp( _TransitionLow, _TransitionHigh, v.y );
            float trSinWeight = 1-trRotationWeight;
            
            //rotation - bottom
            if ( v.y < _TransitionHigh ) {
                float bendAngle = degToRad * _MaxBendAngle * localWeight;
                bendAngle *= (v.x*v.x + v.z*v.z) / (_FishSize * _FishSize);
                
                float cosine = cos( bendAngle );
                float sinus = sin( bendAngle );
                trRotation.x = v.x * cosine - v.z * sinus;
                trRotation.z = v.x * sinus + v.z * cosine;   
            }
            
            //sin - top
            if ( v.y > _TransitionLow ) {
                trSin.x = v.x + 0.1 * localWeight;
            }
            
            v = trRotation * trRotationWeight + trSin * trSinWeight;
            
            
            //translation
            v.x += _MaxTranslation * -cos( _Speed * _Phase * 2*PI );
            
            return v;
        }
        
        void vert ( inout appdata_full v) {
            v.vertex = float4( deformMesh(v.vertex.xyz ), 1 );
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
