// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Juicy/GeneralObject"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Color("Color", Color) = (0.5735294,0,0,1)
		_EmissionColor("EmissionColor", Color) = (0.5735294,0,0,1)
		_EmissionAmount("EmissionAmount", Range( 0 , 2)) = 0
		_EmissionSpeed("EmissionSpeed", Float) = 0.3
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
		#pragma surface surf StandardSpecular keepalpha noshadow nolightmap  nodynlightmap nodirlightmap noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
		};

		uniform half4 _Color;
		uniform half _EmissionSpeed;
		uniform half _BaseEmission;
		uniform half4 _EmissionColor;
		uniform half _EmissionAmount;
		uniform half3 _Offset;
		uniform half _MinDistance;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float Distance = distance( ase_worldPos , _WorldSpaceCameraPos );
			v.vertex.xyz += ( ( _Offset / 1000.0 ) * pow( clamp( ( Distance - _MinDistance ) , 0.0 , 5000.0 ) , 2.0 ) );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Albedo = ( 0.8 * _Color ).rgb;
			float3 ase_worldNormal = i.worldNormal;
			float3 vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult50 = dot( i.viewDir , vertexNormal );
			float3 ase_worldPos = i.worldPos;
			float Distance = distance( ase_worldPos , _WorldSpaceCameraPos );
			o.Emission = ( ( ( dotResult50 * 0.3 ) * half4(1,1,1,1) ) + ( ( 0.5 + ( 0.5 * sin( ( Distance * _EmissionSpeed ) ) ) + _BaseEmission ) * _EmissionColor * _EmissionAmount ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=11001
-1359;178;1352;702;1557.837;662.6573;2.199999;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-824.777,420.5826;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceCameraPos;31;-861.5768,570.9827;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.DistanceOpNode;33;-570.3764,480.9828;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;-386.1375,479.1413;Float;False;Distance;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;67;-591.038,-43.05865;Float;False;65;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-583.1551,53.66549;Float;False;Property;_EmissionSpeed;EmissionSpeed;3;0;0.3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-335.5,-5.900589;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;66;-330.2379,737.7414;Float;False;65;0;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;20;-179.5553,-6.33455;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-202.6774,-79.21731;Float;False;Constant;_Float0;Float 0;4;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;45;-364.9996,-571.2003;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-318.1786,821.3825;Float;False;Property;_MinDistance;MinDistance;5;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;46;-300.9995,-731.2001;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;40;-123.6786,956.8821;Float;False;Constant;_Float2;Float 2;7;0;5000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;39;-116.0786,873.2822;Float;False;Constant;_Float1;Float 1;7;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-4.455064,-56.53452;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-51.67773,49.08235;Float;False;Property;_BaseEmission;BaseEmission;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;50;-108.9995,-619.2003;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-111.5788,744.4828;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-164.2766,-447.0187;Float;False;Constant;_Float6;Float 6;7;0;0.3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;35;76.52145,478.3822;Float;False;Property;_Offset;Offset;6;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;38;108.1214,787.7824;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;110.0219,932.1814;Float;False;Constant;_Float3;Float 3;7;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;2;-61.72947,135.3154;Half;False;Property;_EmissionColor;EmissionColor;1;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;52;119.5215,-352.3177;Float;False;Constant;_Color0;Color 0;7;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;58.12465,-552.0182;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;62.52188,639.5814;Float;False;Constant;_Float4;Float 4;7;0;1000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-122.5547,314.7655;Float;False;Property;_EmissionAmount;EmissionAmount;2;0;0;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;149.4225,-79.91722;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;41;288.6223,820.0817;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;281.0219,557.8815;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;322.0455,28.26542;Float;False;3;3;0;FLOAT;0.0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;1;311.8253,292.5001;Half;False;Property;_Color;Color;0;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;378.722,-414.7181;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;69;334.1628,218.7417;Float;False;Constant;_Float8;Float 8;7;0;0.8;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;60;618.6998,-217.9007;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;560.2628,247.2417;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;469.1214,610.7825;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;787.9001,261.9;Half;False;True;2;Half;ASEMaterialInspector;0;StandardSpecular;Juicy/GeneralObject;False;False;False;False;False;False;True;True;True;False;False;True;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;False;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;30;0
WireConnection;33;1;31;0
WireConnection;65;0;33;0
WireConnection;63;0;67;0
WireConnection;63;1;11;0
WireConnection;20;0;63;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;50;0;46;0
WireConnection;50;1;45;0
WireConnection;36;0;66;0
WireConnection;36;1;34;0
WireConnection;38;0;36;0
WireConnection;38;1;39;0
WireConnection;38;2;40;0
WireConnection;58;0;50;0
WireConnection;58;1;57;0
WireConnection;23;0;22;0
WireConnection;23;1;21;0
WireConnection;23;2;24;0
WireConnection;41;0;38;0
WireConnection;41;1;42;0
WireConnection;43;0;35;0
WireConnection;43;1;44;0
WireConnection;14;0;23;0
WireConnection;14;1;2;0
WireConnection;14;2;12;0
WireConnection;53;0;58;0
WireConnection;53;1;52;0
WireConnection;60;0;53;0
WireConnection;60;1;14;0
WireConnection;68;0;69;0
WireConnection;68;1;1;0
WireConnection;37;0;43;0
WireConnection;37;1;41;0
WireConnection;0;0;68;0
WireConnection;0;2;60;0
WireConnection;0;11;37;0
ASEEND*/
//CHKSM=2131395E6B1FD0C160987BC22A4A3AD48C935BE1