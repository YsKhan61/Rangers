using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityView : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;

    [SerializeField]
    private ParticleSystem m_DisappearPS;

    [SerializeField]
    private ParticleSystem m_AppearPS;

    [SerializeField]
    private AudioClip m_DisappearAudioClip;

    [SerializeField]
    private AudioClip m_AppearAudioClip;

    /// <summary>
    /// Duration of the appear particle system.
    /// </summary>
    public float AppearPSDuration => m_AppearPS.main.duration;

    public void PlayDisappearPS()
    {
        m_DisappearPS.Play();
    }

    public void PlayAppearPS()
    {
        m_AppearPS.Play();
    }

    public void StopDisappearPS()
    {
        m_DisappearPS.Stop();
    }

    public void StopAppearPS()
    {
        m_AppearPS.Stop();
    }

    public void PlayDisappearAudio()
    {
        m_AudioSource.PlayOneShot(m_DisappearAudioClip);
    }

    public void PlayAppearAudio()
    {
        m_AudioSource.PlayOneShot(m_AppearAudioClip);
    }
}
