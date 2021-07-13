using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
namespace CustomPipeline
{
    public class Shadows
    {

        static int MAX_SHADOW_LIGHT = 1;
        static string buffname = "Shadows";
        ShadowSetting shadowSetting;
        CommandBuffer buffer = new CommandBuffer
        {
            name = buffname
        };
        CullingResults cullResults;
        static int dirShadowAtlasID = Shader.PropertyToID("_DirectionalShadowAtlas");

        struct ShadowDirctionalLight
        {
            public int visiableLightIndex;
        }

        //用于生成阴影的可见光索引
        ShadowDirctionalLight[] ShadowDirctionalLights = new ShadowDirctionalLight[MAX_SHADOW_LIGHT];
        ScriptableRenderContext context;
        int shadowDirctionalLightCount;
        public void SetUp(ScriptableRenderContext context, CullingResults cullResults,ShadowSetting shadowSetting)
        {
            this.context = context;
            this.shadowSetting = shadowSetting;
            this.cullResults = cullResults;
            this.shadowDirctionalLightCount = 0;
        }

        public void ReserverDirectionalShadow(Light ligth,int visiableLightIndex)
        {
            if (visiableLightIndex < MAX_SHADOW_LIGHT
            && ligth.shadows != LightShadows.None
            && ligth.shadowStrength > 0
            && cullResults.GetShadowCasterBounds(visiableLightIndex, out Bounds b))
            {
                ShadowDirctionalLights[shadowDirctionalLightCount++] = new ShadowDirctionalLight { visiableLightIndex = visiableLightIndex };
            }
        }

        void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        public void CleanUp()
        {
            buffer.ReleaseTemporaryRT(dirShadowAtlasID);
            ExecuteBuffer();
        }

        void Render()
        {
            if (shadowDirctionalLightCount > 0)
            {
                RenderDirctionalShadows();
            }
        }

        void RenderDirctionalShadows()
        {
            int atlassize = (int)shadowSetting.DirectionSetting.TextureSize;
            buffer.GetTemporaryRT(dirShadowAtlasID, atlassize, atlassize, 32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap);
            buffer.SetRenderTarget(dirShadowAtlasID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            buffer.ClearRenderTarget(true, false, Color.clear);
            ExecuteBuffer();
        }


    }
}