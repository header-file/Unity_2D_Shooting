using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string, AudioClip> SoundLists;

    public float BgmVolume;
    public float EffectVolume;
    public bool IsBgmMute;
    public bool IsEffectMute;

    AudioSource BGMSource;
    AudioSource[] EffectSources;

    int EffectIndex = 0;


    void Awake()
    {
        SoundLists = new Dictionary<string, AudioClip>();

        object[] temp = Resources.LoadAll("Sounds");
        for(int i = 0; i < temp.Length; i++)
        {
            AudioClip clip = temp[i] as AudioClip;
            SoundLists.Add(clip.name, clip);
        }

        BgmVolume = 0.5f;
        EffectVolume = 0.5f;
        BGMSource = new AudioSource();
        EffectSources = new AudioSource[Constants.MAX_EFFECT_LAYER];
    }

    void Start()
    {
        SetAudioSources();
    }

    public void SetAudioSources()
    {
        GameObject[] source = GameObject.FindGameObjectsWithTag("Audio");
        for (int i = 0; i < source.Length; i++)
        {
            if (i == 0)
                BGMSource = source[i].GetComponent<AudioSource>();
            else
                EffectSources[i - 1] = source[i].GetComponent<AudioSource>();
        }
    }

    public void SetBGMVolume(float volume)
    {
        BgmVolume = volume;
        BGMSource.volume = BgmVolume;
    }

    public void SetBGMMute(bool isMute)
    {
        IsBgmMute = isMute;
        BGMSource.mute = IsBgmMute;
    }

    public void SetEffectMute(bool isMute)
    {
        IsEffectMute = isMute;
        for (int i = 0; i < EffectSources.Length; i++)
            EffectSources[i].mute = IsEffectMute;
    }

    public void PlayBGM(string fileName)
    {
        AudioClip clip = null;
        if (SoundLists.ContainsKey(fileName))
            clip = SoundLists[fileName];
        else
        {
            clip = Resources.Load("Resources/Sounds/" + fileName) as AudioClip;
            SoundLists.Add(clip.name, clip);
        }

        if (clip == null)
            return;
        if (BGMSource == null)
            SetAudioSources();        

        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.volume = BgmVolume;
        BGMSource.Play();
    }

    public void PlayEffect(string fileName)
    {
        AudioClip clip = null;
        if (SoundLists.ContainsKey(fileName))
            clip = SoundLists[fileName];
        else
        {
            clip = Resources.Load("Resources/Sounds/" + fileName) as AudioClip;
            SoundLists.Add(clip.name, clip);
        }

        if (clip == null)
            return;
        if (EffectSources[EffectIndex] == null)
            SetAudioSources();

        EffectSources[EffectIndex].PlayOneShot(clip, EffectVolume);

        EffectIndex++;
        if (EffectIndex >= Constants.MAX_EFFECT_LAYER)
            EffectIndex = 0;

        //Debug.Log("Play " + clip.name + ", " + EffectVolume);
    }
}
