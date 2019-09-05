// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShaderSchool/FirstShader"
{
    Properties
    {
        _Color("Shader Color", Color) = (1,1,1,1)
        
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct vertInput
            {
                float4 pos : POSITION;
            };

            struct vertOutput
            {
                float4 pos : SV_POSITION;
            };

            half4 _Color;

            vertOutput vert (vertInput i)
            {
                vertOutput o;
                o.pos = UnityObjectToClipPos(i.pos);
                return o;
            }

            half4 frag (vertOutput o) : COLOR
            {
                return _Color;
            }

            ENDCG
        }
    }
}
