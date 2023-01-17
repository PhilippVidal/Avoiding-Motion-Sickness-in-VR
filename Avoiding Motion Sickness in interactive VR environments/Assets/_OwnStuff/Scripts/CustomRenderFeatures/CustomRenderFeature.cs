using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderFeature : ScriptableRendererFeature
{
    //Render passes
    TunnelingRenderPass m_TunnelingRenderPass = null;
    CageRenderPass m_CageRenderPass = null;
    FadeRenderPass m_FadeRenderPass = null;

    public RenderPassEvent m_RenderPassEvent = RenderPassEvent.AfterRendering;

    RenderTargetIdentifier m_CageRenderTextureIdentifier;
    bool m_CageRenderTextureIdentifierInitialized = false;

    public override void Create()
    {
        m_TunnelingRenderPass = new TunnelingRenderPass();
        m_CageRenderPass = new CageRenderPass(m_RenderPassEvent);
        m_FadeRenderPass = new FadeRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        //Setup pass for screen fading
        if(renderingData.cameraData.camera.tag == "MainCamera")
        {
            m_FadeRenderPass.SetTarget(renderer.cameraColorTarget);
            renderer.EnqueuePass(m_FadeRenderPass);
        }

        //Setup passes for tunneling
        Tunneling tunneling = Tunneling.Instance;
        if (!tunneling || !tunneling.IsTunnelingEnabled)
            return;
               
        switch (renderingData.cameraData.camera.tag)
        {
            case "MainCamera":
                m_TunnelingRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
                m_TunnelingRenderPass.SetTarget(renderer.cameraColorTarget);
                renderer.EnqueuePass(m_TunnelingRenderPass);
                break;
            case "CageCamera":
                if (tunneling.TunnelingMode != TunnelingModes.CAGE)
                    return;

                if (!tunneling.m_CageRenderTexture)
                    return;

                if (!m_CageRenderTextureIdentifierInitialized)
                    m_CageRenderTextureIdentifier = new RenderTargetIdentifier(tunneling.m_CageRenderTexture);
                   
                m_CageRenderPass.SetSource(renderer.cameraColorTarget);
                m_CageRenderPass.SetTarget(m_CageRenderTextureIdentifier);
                renderer.EnqueuePass(m_CageRenderPass);
                break;
        }
    }
}
