﻿Shader "test/test-shader" {
	Properties {
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader {
		Pass {
			CGPROGRAM
			
			// pragmas
			#pragma vertex vert
			#pragma fragment frag		
			
			// user defined var
			float4 _Color;
			
			// base input structs
			struct vertexInput {
				float4 vertex : POSITION;
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
			};
			
			// vertex function
			vertexOutput vert(vertexInput v) {
				vertexOutput o;		
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return o;
			}
			
			// fragment function
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color;
			}
			
			ENDCG
		}
	}
	// fallback commented out during development
	//Fallback "Diffuse"
}