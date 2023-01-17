using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TunnelingRenderPass : ScriptableRenderPass
{
    Material m_Material; 
    RenderTargetIdentifier m_TargetIdentifier;

    public TunnelingRenderPass()
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

        Tunneling tunneling = Tunneling.Instance;
        if (!tunneling || !tunneling.IsTunnelingEnabled)
            return;

       
        m_Material = Tunneling.Instance.Material;
        if (m_Material == null)
            return;

        //Get eye specific (stereo) projection matricies
        Matrix4x4 rightEyeMatrix = Camera.main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
        Matrix4x4 leftEyeMatrix = Camera.main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);

        //https://docs.unity3d.com/ScriptReference/GL.GetGPUProjectionMatrix.html
        rightEyeMatrix = GL.GetGPUProjectionMatrix(rightEyeMatrix, false).inverse;
        leftEyeMatrix = GL.GetGPUProjectionMatrix(leftEyeMatrix, false).inverse;

        //Set values for shader
        m_Material.SetMatrix("_RightEyeMatrix", rightEyeMatrix);
        m_Material.SetMatrix("_LeftEyeMatrix", leftEyeMatrix);

        m_Material.SetFloat("_Radius", tunneling.m_CurrentRadius);
        m_Material.SetFloat("_SmoothingOffset", tunneling.m_SmoothingOffset);

        //Draw quad over full screen with assigned material
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.SetRenderTarget(new RenderTargetIdentifier(m_TargetIdentifier, 0, CubemapFace.Unknown, -1));
        cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_Material);

        //clean up
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
