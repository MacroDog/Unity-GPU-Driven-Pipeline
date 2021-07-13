using UnityEngine;
using UnityEngine.Rendering;
using  UnityEngine.Profiling;
namespace CustomPipeline
{
    public partial class CameraRender
    {
#if UNITY_EDITOR
        static ShaderTagId[] legacyShaderIds =
        {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")

    };
        Material errorMater;
        partial void DrawUnsupportedShader()
        {
            if (errorMater == null)
            {
                errorMater = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }
            var drawSetting = new DrawingSettings()
            {
                overrideMaterial = errorMater,
                sortingSettings = new SortingSettings(camera)
            };
            for (int i = 0; i < legacyShaderIds.Length; i++)
            {
                drawSetting.SetShaderPassName(1, legacyShaderIds[i]);
            }
            var filterSetting = FilteringSettings.defaultValue;
            context.DrawRenderers(cullResults, ref drawSetting, ref filterSetting);
        }
        partial void PreparBuff()
        {
            Profiler.BeginSample("Editor.Only");
            cameraBuffer.name = camera.name;
            Profiler.EndSample();
        }
#endif
    }
}
