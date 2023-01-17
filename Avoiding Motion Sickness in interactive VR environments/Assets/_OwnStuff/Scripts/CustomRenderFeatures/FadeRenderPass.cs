using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FadeRenderPass : ScriptableRenderPass
{

    RenderTargetIdentifier m_TargetIdentifier;

    public FadeRenderPass()
    {
        this.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public void SetTarget(RenderTargetIdentifier targetIdentifier)
    {
        m_TargetIdentifier = targetIdentifier;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(new RenderTargetIdentifier(m_TargetIdentifier, 0, CubemapFace.Unknown, -1));
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {

        if (renderingData.cameraData.camera.cameraType != CameraType.Game)
            return;

        CameraFade fade = CameraFade.Instance;
        if (!fade)
            return;

        Material fadeMaterial = fade.m_FadeMaterial;
        if (!fadeMaterial)
            return;


        CommandBuffer cmd = CommandBufferPool.Get();

        cmd.SetRenderTarget(new RenderTargetIdentifier(m_TargetIdentifier, 0, CubemapFace.Unknown, -1));
        cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, fadeMaterial);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}
