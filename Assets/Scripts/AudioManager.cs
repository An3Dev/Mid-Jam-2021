using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    const string enableSFXKey = "EnableSFX";
    const string enableBGMusicKey = "EnableBGMusic";

    bool isEnabledSFX, enableBGMusic;

    public AudioMixer sfxAudioMixer, bgMusicAudioMixer;
    public AudioSource SFXAudioSource, BGMusicAudioSource;

    public GameObject enabledSFX, disabledSFX;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        isEnabledSFX = bool.Parse(PlayerPrefs.GetString(enableSFXKey, "true"));
        enableBGMusic = bool.Parse(PlayerPrefs.GetString(enableBGMusicKey, "true"));
    }

    private void Start()
    {
        if (isEnabledSFX)
        {
            sfxAudioMixer.SetFloat("Volume", 0);
        }
        else
        {
            sfxAudioMixer.SetFloat("Volume", -80);
        }

        if (enableBGMusic)
        {
            bgMusicAudioMixer.SetFloat("Volume", 0);
        }
        else
        {
            bgMusicAudioMixer.SetFloat("Volume", -80);
        }

        if (disabledSFX != null)
        {
            disabledSFX.SetActive(!isEnabledSFX);
            enabledSFX.SetActive(isEnabledSFX);
        }
    }

    public void ChangeSFX(bool enable)
    {
        isEnabledSFX = enable;
        PlayerPrefs.SetString(enableSFXKey, enable.ToString());

        if (isEnabledSFX)
        {
            sfxAudioMixer.SetFloat("Volume", 0);
        }
        else
        {
            sfxAudioMixer.SetFloat("Volume", -80);
        }
    }
    
    public void ChangeBGMusic(bool enable)
    {
        enableBGMusic = enable;
        PlayerPrefs.SetString(enableBGMusicKey, enable.ToString());
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        if (clip != null)
            SFXAudioSource.PlayOneShot(clip);
    }
}
