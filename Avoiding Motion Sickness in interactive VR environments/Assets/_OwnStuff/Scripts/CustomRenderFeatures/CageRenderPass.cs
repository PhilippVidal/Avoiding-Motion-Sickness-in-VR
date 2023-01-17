using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CageRenderPass : ScriptableRenderPass
{
    RenderTargetIdentifier m_Source;
    RenderTargetIdentifier m_Destination;

    public CageRenderPass(RenderPassEvent renderPassEvent)
    {
        this.renderPassEvent = renderPassEvent;
    }

    public void SetSource(RenderTargetIdentifier src)
    {
        m_Source = src;
    }

    public void SetTarget(RenderTargetIdentifier dest)
    {
        m_Destination = dest;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();

        cmd.Blit(m_Source, m_Destination);

        context.ExecuteCommandBuffer(cmd);
        Tunneling.Instance.Material.SetTexture("_CageTexture", Tunneling.Instance.m_CageRenderTexture);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
