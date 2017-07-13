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
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
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
			float3 ase_worldNormal = i.worldNormal;
			float3 vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult50 = dot( i.viewDir , vertexNormal );
			o.Specular = ( _Color + ( ( dotResult50 * 0.4 ) * float4(1,1,1,1) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=11001
-1316;207;1250;637;1868.476;-109.8816;1.6;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-545.6776,631.1824;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceCameraPos;31;-582.4774,781.5825;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;11;-580.4548,4.2655;Float;False;Property;_EmissionSpeed;EmissionSpeed;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;33;-291.277,687.1826;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;45;-1619.399,427.1998;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;46;-1555.399,267.1998;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-318.1786,821.3825;Float;False;Property;_MinDistance;MinDistance;5;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;18;-358.0546,9.06542;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;39;-116.0786,873.2822;Float;False;Constant;_Float1;Float 1;7;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;40;-123.6786,956.8821;Float;False;Constant;_Float2;Float 2;7;0;5000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;50;-1363.399,379.1998;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-111.5788,744.4828;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;20;-175.6553,10.66545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-202.6774,-79.21731;Float;False;Constant;_Float0;Float 0;4;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-1418.676,551.3813;Float;False;Constant;_Float6;Float 6;7;0;0.4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;110.0219,932.1814;Float;False;Constant;_Float3;Float 3;7;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;38;108.1214,787.7824;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;35;76.52145,478.3822;Float;False;Property;_Offset;Offset;5;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;52;-1134.878,646.0823;Float;False;Constant;_Color0;Color 0;7;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-4.455064,-56.53452;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-49.07773,56.88235;Float;False;Property;_BaseEmission;BaseEmission;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;62.52188,639.5814;Float;False;Constant;_Float4;Float 4;7;0;1000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1196.275,446.3819;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;281.0219,557.8815;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-875.6774,583.6819;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;1;-1034.875,212.7001;Half;False;Property;_Color;Color;0;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-3.154684,150.5655;Float;False;Property;_EmissionAmount;EmissionAmount;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;154.6225,-70.81722;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;41;288.6223,820.0817;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;2;-270.7294,113.2154;Half;False;Property;_EmissionColor;EmissionColor;1;0;0.5735294,0,0,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;469.1214,610.7825;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-760.4775,383.6818;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;322.0455,28.26542;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;710,108;Float;False;True;2;Float;ASEMaterialInspector;0;StandardSpecular;Juicy/GeneralObject;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;30;0
WireConnection;33;1;31;0
WireConnection;18;0;11;0
WireConnection;50;0;46;0
WireConnection;50;1;45;0
WireConnection;36;0;33;0
WireConnection;36;1;34;0
WireConnection;20;0;18;0
WireConnection;38;0;36;0
WireConnection;38;1;39;0
WireConnection;38;2;40;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;58;0;50;0
WireConnection;58;1;57;0
WireConnection;43;0;35;0
WireConnection;43;1;44;0
WireConnection;53;0;58;0
WireConnection;53;1;52;0
WireConnection;23;0;22;0
WireConnection;23;1;21;0
WireConnection;23;2;24;0
WireConnection;41;0;38;0
WireConnection;41;1;42;0
WireConnection;37;0;43;0
WireConnection;37;1;41;0
WireConnection;54;0;1;0
WireConnection;54;1;53;0
WireConnection;14;0;23;0
WireConnection;14;1;12;0
WireConnection;14;2;2;0
WireConnection;0;2;14;0
WireConnection;0;3;54;0
WireConnection;0;11;37;0
ASEEND*/
//CHKSM=3807C6D13297F307B3FC84F9E78FA9D14A6DD290