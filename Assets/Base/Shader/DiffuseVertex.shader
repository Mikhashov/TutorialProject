Shader "Custom/DiffuseVertex"
{
    Properties
    {
        _MainTex("Albedo Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _BlendScale("Blend Scale", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR; // Vertex color
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 vertexColor : COLOR; // Pass vertex color
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float _BlendScale;

            // Function to convert Gamma to Linear space
            float3 GammaToLinear(float3 gammaColor)
            {
                return pow(gammaColor, float3(2.2, 2.2, 2.2)); // Apply gamma correction
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.vertexColor = float4(GammaToLinear(v.color.rgb), v.color.a); // Convert vertex color to Linear
                return o;
            }

            float3 unpackNormalMap(sampler2D normalMap, float2 uv)
            {
                float4 normalTex = tex2D(normalMap, uv);
                return normalize(normalTex.xyz * 2.0 - 1.0); // Convert from [0,1] to [-1,1]
            }

            float4 frag(v2f i) : SV_Target
            {
                // Normalize world normal for blending weights
                float3 worldNormal = normalize(i.worldNormal);

                // Compute blending weights for Triplanar mapping
                float3 blendWeights = abs(worldNormal);
                blendWeights /= (blendWeights.x + blendWeights.y + blendWeights.z);

                // Sample albedo textures along 3 axes
                float4 xProj = tex2D(_MainTex, i.worldPos.yz * _BlendScale);
                float4 yProj = tex2D(_MainTex, i.worldPos.zx * _BlendScale);
                float4 zProj = tex2D(_MainTex, i.worldPos.xy * _BlendScale);

                // Combine albedo projections
                float4 triplanarColor = xProj * blendWeights.x +
                                        yProj * blendWeights.y +
                                        zProj * blendWeights.z;

                // Combine texture color with vertex color (Linear vertex color)
                float4 finalColor = triplanarColor * i.vertexColor;

                // Normal maps along 3 axes
                float3 xNormal = unpackNormalMap(_NormalMap, i.worldPos.yz * _BlendScale);
                float3 yNormal = unpackNormalMap(_NormalMap, i.worldPos.zx * _BlendScale);
                float3 zNormal = unpackNormalMap(_NormalMap, i.worldPos.xy * _BlendScale);

                // Combine normal projections
                float3 blendedNormal = normalize(
                    xNormal * blendWeights.x +
                    yNormal * blendWeights.y +
                    zNormal * blendWeights.z
                );

                // Simple lighting calculation for visualizing normals
                float3 lightDir = normalize(float3(0.5, 0.5, 0.5)); // Arbitrary light direction
                float lighting = saturate(dot(blendedNormal, lightDir)) * 0.5 + 0.5;

                return finalColor * lighting; // Apply lighting
            }
            ENDCG
        }
    }
}

