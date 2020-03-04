// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaMap ("Additional Alpha Map (Greyscale)", 2D) = "white" {} 
		_Blur ("BlurPix", Range(0,10)) = 0
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"DisableBatching" = "True"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaMap; 
			float4 _MainTex_ST;
			half _Blur;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};
	

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : SV_Target
			{
				//half4 c = tex2D(_MainTex, IN.texcoord); 
			    //c.a = c.a * tex2D(_AlphaMap, IN.texcoord).r; 
			    
			    fixed4 col;
			    
    			if (IN.color.r < 0.001)  
    			{
        			col = tex2D(_MainTex, IN.texcoord);  
        			float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));  
        			col.rgb = float3(grey, grey, grey);
					//col.a =  IN.color.a; 
    			}
    			else  
    			{  
        			col = tex2D(_MainTex, IN.texcoord) * IN.color;  
    			}  
    			col.a = col.a * tex2D(_AlphaMap, IN.texcoord).r;
    			
    			if (_Blur > 0)
    			{
    				half4 s1 = tex2D(_MainTex,IN.texcoord + float2(_Blur,0.00));
                	half4 s2 = tex2D(_MainTex,IN.texcoord + float2(-_Blur,0.00));
                	half4 s3 = tex2D(_MainTex,IN.texcoord + float2(0.00,_Blur));
                	half4 s4 = tex2D(_MainTex,IN.texcoord + float2(0.00,-_Blur));
                	
                	float pct=0.2;
                    col = col* (1- pct*4) + s1* pct + s2* pct+ s3* pct + s4* pct;
    			}
    			return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"DisableBatching" = "True"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			//ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
