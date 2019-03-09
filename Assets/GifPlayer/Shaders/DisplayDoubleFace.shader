Shader "Custom/DisplayDoubleFace" {
	Properties{
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "" {}
	}
		SubShader{
		Pass{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			Alphatest Greater 0
			SetTexture[_MainTex] {
				constantColor[_Color]
					combine texture * constant }
		} }  }