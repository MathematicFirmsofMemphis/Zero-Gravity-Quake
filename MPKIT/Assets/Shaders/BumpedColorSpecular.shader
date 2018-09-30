Shader "Bumped ColoredSpecular" {

Properties {

    _Color ("Main Color", Color) = (1,1,1,1)

    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

    _Shininess ("Shininess", Range (0.03, 1)) = 0.078125

    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

    _BumpMap ("Normalmap", 2D) = "bump" {}

	_AO("Ambient Occlusion Texture", 2D) = "white" {}

    

    _SpecTex ("Spec (RGB)", 2D) = "white" {}

}

SubShader { 

    Tags { "RenderType"="Opaque" }

    LOD 400

    

CGPROGRAM

#pragma surface surf BlinnPhong

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _SpecTex;
sampler2D _AO;

float4 _Color;

//float4 _SpecColor;

float _Shininess;

 

struct Input {

    float2 uv_MainTex;

    float2 uv_BumpMap;

	float2 uv_AO;

};

 

void surf (Input IN, inout SurfaceOutput o) {

    half4 tex = tex2D(_MainTex, IN.uv_MainTex);

    half4 spectex = tex2D(_SpecTex, IN.uv_MainTex);

	half4 ao = tex2D(_AO, IN.uv_AO);

    _SpecColor = spectex;

    

    o.Albedo = tex.rgb * _Color.rgb;

    o.Gloss = tex.a;

    o.Alpha = tex.a * _Color.a;

    o.Specular = _Shininess;

    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

}

ENDCG

}

 

FallBack "Specular"

}