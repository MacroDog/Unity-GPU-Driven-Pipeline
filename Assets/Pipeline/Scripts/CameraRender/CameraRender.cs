using UnityEngine;
using UnityEngine.Rendering;
namespace CustomPipeline
{
    public partial class CameraRender
    {
        ScriptableRenderContext context;
        Camera camera;

        string SRPID = "SRPDefaultUnlit";

        CommandBuffer cameraBuffer = new CommandBuffer
        {
            name = "Render Camera"
        };
        bool enableDynamicBatching;
        bool enableInstancing;

        public CameraRender(bool enableDynamicBatching, bool enableInstancing)
        {
            this.enableDynamicBatching = enableDynamicBatching;
            this.enableInstancing = enableInstancing;
        }
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.camera = camera;
            this.context = context;
            PreparBuff();
            if (!cull())
            {
                return;
            }
            context.SetupCameraProperties(camera);
            CameraClearFlags clearFlags = camera.clearFlags;
            var cleardathp = ((int)clearFlags & (int)CameraClearFlags.Depth) == 1;
            var clearcolor = ((int)clearFlags & (int)CameraClearFlags.Color) == 1;
            cameraBuffer.ClearRenderTarget(cleardathp, clearcolor, camera.backgroundColor);
            SetUp();
            DrawVisableGemoerty();
            DrawUnsupportedShader();
            Submit();
        }
        partial void PreparBuff();
        partial void DrawUnsupportedShader();


        void DrawVisableGemoerty()
        {
            var SortingSettings = new SortingSettings();
            var drawSetting = new DrawingSettings(new ShaderTagId(SRPID),SortingSettings)
            {
                enableInstancing = enableInstancing,
                enableDynamicBatching = enableDynamicBatching
            };
            drawSetting.SetShaderPassName(0, new ShaderTagId(SRPID));
            var filterSetting = new FilteringSettings();
            filterSetting.renderQueueRange = RenderQueueRange.opaque;
            SortingSettings.criteria = SortingCriteria.CommonOpaque;
            drawSetting.sortingSettings = SortingSettings;
            //绘制不透明
            context.DrawRenderers(cullResults, ref drawSetting, ref filterSetting);
            //绘制天空盒
            context.DrawSkybox(camera);
            //绘制半透明
            filterSetting.renderQueueRange = RenderQueueRange.transparent;
            SortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawSetting.sortingSettings = SortingSettings;
            context.DrawRenderers(cullResults, ref drawSetting, ref filterSetting);
        }



        CullingResults cullResults;
        ScriptableCullingParameters cullingParam;
        bool cull()
        {
            if (!camera.TryGetCullingParameters(camera, out cullingParam))
            {
                return false;
            }
            else
            {
#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView)
                {
                    ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
                }
#endif
                cullResults = context.Cull(ref cullingParam);
                return true;
            }
        }

        void SetUp()
        {
            cameraBuffer.BeginSample(cameraBuffer.name);
            ExecuteBuffer();
            context.SetupCameraProperties(camera);
        }

        void Submit()
        {
            cameraBuffer.EndSample(cameraBuffer.name);
            ExecuteBuffer();
            context.Submit();
        }

        void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(cameraBuffer);
            cameraBuffer.Clear();
        }
    }
}
