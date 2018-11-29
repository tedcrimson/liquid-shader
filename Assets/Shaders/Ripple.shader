// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Ripple" {
	Properties {
		_Color("Color", Color) = (1, 1, 1, 1)
		_PointColor("Point Color", Color) = (1, 0, 0, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0, 1)) = 0.5
		_Metallic("Metallic", Range(0, 1)) = 0.0
		_ImpactSize("Impact Size", Range(0, 1)) = 0.5
		_ImpactStrength("Impact Strength", Range(0, 5)) = 0.5
		_Scale ("Scale X", Range(0, 0.5)) = 0
		_ScaleY("Scale Y", Range(0, 0.5)) = 0
		_ScaleZ("Scale Z", Range(0, 0.5)) = 0
		_Speed("Speed", Range(0,1)) = 0.5
		_Frequency("Freq", Range(0,100)) = 0.5
	}
	SubShader {
		Tags {"RenderType"="Transparent" "Queue"="Transparent"}
		LOD 200
		ZWrite Off
		// ColorMaterial Emission
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma vertex vert
			
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
			
		sampler2D _MainTex;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 customValue;
		};
		
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _PointColor;
		float _ImpactSize;
		float _ImpactStrength;
		float _Scale;
		float _ScaleY;
		float _ScaleZ;
		float _Speed;
		float _Frequency;
		
		int _PointsSize;
		uniform fixed4 _Points[50];
		
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
			
		
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			
			half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));
			half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);
			half valuey = _ScaleY * sin(_Time.w * _Speed + offsetvert * _Frequency);
			half valuez = _ScaleZ * sin(_Time.w * _Speed + offsetvert * _Frequency);
			v.vertex.x += value;
			v.vertex.y += valuey;
			v.vertex.z += valuez;
			o.customValue.x = value;
			o.customValue.y = valuey;
			o.customValue.z = valuez;
		}
		
		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			
			fixed emissive = 0;
			float3 objPos = mul(unity_WorldToObject, float4(IN.worldPos, 1)).xyz;
			for(int i = 0; i < _PointsSize; i++ )
			{
				emissive += -sin(frac(max(0, (_Points[i].w * _ImpactSize) - distance(_Points[i].xyz, objPos.xyz)) / _ImpactSize) * (1 - _Points[i].w)) * _ImpactStrength;
			}
			o.Normal += emissive ; // * c.rgb;
			o.Normal += IN.customValue;
			o.Emission += emissive * c.rgb * _PointColor;
			
			//   o.Normal = UnpackNormal (tex2D (_NormalTex, IN.uv_MainTex)) * emissive჻;
			
			o.Albedo = _Color;
			// // Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
