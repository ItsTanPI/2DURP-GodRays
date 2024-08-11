using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


// GodRays.cs
// This script handles the creation and configuration of the God Rays effect in the Unity URP 2D environment.
// It manages the shader parameters and applies the post-processing effect to the scene.
//
// Project Repository: https://github.com/ItsTanPI/2DURP-GodRays
// My itch.io Profile: https://tan-pi.itch.io

namespace GodRays2D.Runtime.GodRays
{
    public class GodRays : ScriptableRendererFeature
    {
        public Material Material;

        class GodRaysPass : ScriptableRenderPass
        {
            const string RenderPassName = "GodRays";

            RenderTargetHandle Texture;

            readonly ProfilingSampler ProfileSampler;
            readonly Material GodRays;


            private RenderTexture downscaledTexture;
            private RenderTexture processedTexture;

            static readonly int ProcessedID = Shader.PropertyToID("_ProcessedTex");
            static readonly int Density = Shader.PropertyToID("_Density");
            static readonly int Samples = Shader.PropertyToID("_Calculations");
            static readonly int Weight = Shader.PropertyToID("_Weight");
            static readonly int Decay = Shader.PropertyToID("_Decay");
            static readonly int Exposure = Shader.PropertyToID("_Exposure");

            private bool Enabled;

            readonly GodRaysVolume volume;

            public GodRaysPass(Material material)
            {

                ProfileSampler = new ProfilingSampler(RenderPassName);
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
                GodRays = material;
                Texture.Init("_MainFrame");

                var stack = VolumeManager.instance.stack;


                volume = stack.GetComponent<GodRaysVolume>();
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                int width = cameraTextureDescriptor.width / volume.DownscaleFactor.value;
                int height = cameraTextureDescriptor.height / volume.DownscaleFactor.value;
                downscaledTexture = RenderTexture.GetTemporary(width, height, 0);
                processedTexture = RenderTexture.GetTemporary(width, height, 0);
                processedTexture.filterMode = (FilterMode)volume.filterMode.value;
            }


            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (GodRays == null)
                {
                    Debug.LogError("Material not assigned.");
                    return;
                }

                if (!volume.isActive()) 
                {
                    return;
                }

                var cmd = CommandBufferPool.Get(RenderPassName);
                cmd.Clear();

                using (new ProfilingScope(cmd, ProfileSampler))
                {
                    var source = renderingData.cameraData.renderer.cameraColorTarget;
                    var cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                    cmd.GetTemporaryRT(Texture.id, cameraTargetDescriptor);


                    GodRays.SetFloat(Decay, volume.Decay.value);
                    GodRays.SetFloat(Density, volume.Density.value);
                    GodRays.SetFloat(Weight, volume.Weight.value);
                    GodRays.SetFloat(Exposure, volume.Exposure.value);
                    GodRays.SetFloat(Samples, volume.Samples.value);

                    cmd.Blit(source, downscaledTexture);
                    cmd.Blit(downscaledTexture, processedTexture, GodRays, 0);

                    GodRays.SetTexture(ProcessedID, processedTexture);
                    cmd.Blit(source, Texture.Identifier(), GodRays, 1);
                    cmd.Blit(Texture.Identifier(), source);

                    cmd.ReleaseTemporaryRT(Texture.id);

                }
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (downscaledTexture) RenderTexture.ReleaseTemporary(downscaledTexture);
                if (processedTexture) RenderTexture.ReleaseTemporary(processedTexture);
            }
        }

        GodRaysPass m_ScriptablePass;

        public override void Create()
        {
            m_ScriptablePass = new GodRaysPass(Material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {

            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}
