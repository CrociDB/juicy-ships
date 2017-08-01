// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Juicy/FadeEffect"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Texture("Texture", 2D) = "white" {}
		_FadeVal("FadeVal", Range( 0 , 20)) = 0
		_Color("Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest Always
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform half4 _Color;
		uniform float _FadeVal;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _MaskClipValue = 0.5;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _Color.rgb;
			o.Alpha = 1;
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			clip( ( _FadeVal * tex2D( _Texture, uv_Texture ) ) - ( _MaskClipValue ).xxxx );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=11001
-1364;181;1352;702;1447.55;332.45;1.3;True;True
Node;AmplifyShaderEditor.RangedFloatNode;2;-698.1005,78.49995;Float;False;Property;_FadeVal;FadeVal;1;0;0;0;20;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-713.2007,165;Float;True;Property;_Texture;Texture;0;0;Assets/Art/Textures/FadeMap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-1375.301,178.6997;Float;False;Constant;_Float0;Float 0;4;0;-1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1214.101,-87.79982;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-1154.299,66.90002;Float;False;Constant;_Float1;Float 1;4;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;12;-1192,185.2;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;5;-398.9995,-111.3;Half;False;Property;_Color;Color;3;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-374.3001,95.40001;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.RotatorNode;7;-964.5004,100.7001;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;Unlit;Juicy/FadeEffect;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;Back;1;7;False;0;0;Custom;0.5;True;True;0;True;Transparent;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;11;0
WireConnection;4;0;2;0
WireConnection;4;1;1;0
WireConnection;7;0;8;0
WireConnection;7;1;13;0
WireConnection;7;2;12;0
WireConnection;0;2;5;0
WireConnection;0;10;4;0
ASEEND*/
//CHKSM=9BF907771CFF1F0EC461C59F224F0EE3D73E8EAB