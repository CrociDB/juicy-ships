// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Juicy/GeneralObject"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Color("Color", Color) = (0.5735294,0,0,1)
		_EmissionColor("EmissionColor", Color) = (0.5735294,0,0,1)
		_EmissionSpeed("EmissionSpeed", Float) = 0
		_EmissionAmount("EmissionAmount", Range( 0 , 1)) = 0
		_BaseEmission("BaseEmission", Float) = 0
		_MinDistance("MinDistance", Float) = 0
		_Offset("Offset", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			fixed filler;
		};

		uniform float _EmissionSpeed;
		uniform float _BaseEmission;
		uniform float _EmissionAmount;
		uniform half4 _EmissionColor;
		uniform half4 _Color;
		uniform float3 _Offset;
		uniform float _MinDistance;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			v.vertex.xyz += ( ( _Offset / 1000.0 ) * pow( clamp( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - _MinDistance ) , 0.0 , 5000.0 ) , 2.0 ) );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float mulTime18 = _Time.y * _EmissionSpeed;
			o.Emission = ( ( 0.5 + ( 0.5 * sin( mulTime18 ) ) + _BaseEmission ) * _EmissionAmount * _EmissionColor ).rgb;
			o.Specular = _Color.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=11001
-1316;207;1250;637;860.8781;-56.28183;1.9;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-580.4548,4.2655;Float;False;Property;_EmissionSpeed;EmissionSpeed;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-545.6776,631.1824;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceCameraPos;31;-582.4774,781.5825;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.SimpleTimeNode;18;-358.0546,9.06542;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;33;-291.277,687.1826;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-318.1786,821.3825;Float;False;Property;_MinDistance;MinDistance;5;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;40;-123.6786,956.8821;Float;False;Constant;_Float2;Float 2;7;0;5000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-111.5788,744.4828;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-202.6774,-79.21731;Float;False;Constant;_Float0;Float 0;4;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;39;-116.0786,873.2822;Float;False;Constant;_Float1;Float 1;7;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;20;-175.6553,10.66545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-4.455064,-56.53452;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;35;76.52145,478.3822;Float;False;Property;_Offset;Offset;5;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;62.52188,639.5814;Float;False;Constant;_Float4;Float 4;7;0;1000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;38;108.1214,787.7824;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;110.0219,932.1814;Float;False;Constant;_Float3;Float 3;7;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-49.07773,56.88235;Float;False;Property;_BaseEmission;BaseEmission;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;281.0219,557.8815;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ColorNode;2;-270.7294,113.2154;Half;False;Property;_EmissionColor;EmissionColor;1;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-3.154684,150.5655;Float;False;Property;_EmissionAmount;EmissionAmount;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;154.6225,-70.81722;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;41;288.6223,820.0817;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;322.0455,28.26542;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;469.1214,610.7825;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.ColorNode;1;-538.675,421.0001;Half;False;Property;_Color;Color;0;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;710,108;Float;False;True;2;Float;ASEMaterialInspector;0;StandardSpecular;Juicy/GeneralObject;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;11;0
WireConnection;33;0;30;0
WireConnection;33;1;31;0
WireConnection;36;0;33;0
WireConnection;36;1;34;0
WireConnection;20;0;18;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;38;0;36;0
WireConnection;38;1;39;0
WireConnection;38;2;40;0
WireConnection;43;0;35;0
WireConnection;43;1;44;0
WireConnection;23;0;22;0
WireConnection;23;1;21;0
WireConnection;23;2;24;0
WireConnection;41;0;38;0
WireConnection;41;1;42;0
WireConnection;14;0;23;0
WireConnection;14;1;12;0
WireConnection;14;2;2;0
WireConnection;37;0;43;0
WireConnection;37;1;41;0
WireConnection;0;2;14;0
WireConnection;0;3;1;0
WireConnection;0;11;37;0
ASEEND*/
//CHKSM=72000AB760C3FBB65A94111FE4D4A6CC309F4062