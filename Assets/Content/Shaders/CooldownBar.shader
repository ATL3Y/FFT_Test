Shader "Custom/CooldownBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HueStart ("HueStart", float) = 0
        _HueEnd ("HueEnd", float) = 1
        _Level ("Level", float) = 1
        _Subdivisions ("Subdivisions", float) = 20
    }
    SubShader
    {
   
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            uniform float _HueStart;
            uniform float _HueEnd;
            uniform float _Level;
            uniform float _Subdivisions;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

              // Converting pure hue to RGB
              float3 HUEtoRGB(in float H)
              {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
              }
              
              //Converting HSV to RGB
              float3 HSVtoRGB(in float3 HSV)
              {
                float3 RGB = HUEtoRGB(HSV.x);
                return ((RGB - 1) * HSV.y + 1) * HSV.z;
              }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 20 cycles from 0 to 1
                float val = -.5 + .5 * ( 1 + sin(_Subdivisions * 6.28 * i.uv.y) );
                
                // Omit lines between subdivisions. 
                if(val > 0){
                    // uv.y = 1 is top (green), and uv.y = 0 is bottom (green).
                    // _Level = 1 is full, _Level = 0 is empty.
                    if(i.uv.y < _Level){
                        float hue = _HueStart + i.uv.y * (_HueEnd - _HueStart);
                        col.rgb = HSVtoRGB(float3(hue, 1, 1));  
                    }else{
                        col = float4(0,0,0,0);
                    }
                }else{
                    col = float4(0,0,0,0);
                }

                return col;
            }
            ENDCG
        }
    }
}
