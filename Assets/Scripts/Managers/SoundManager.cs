using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string, AudioClip> SoundLists;
    public AudioSource BGMSource;
    public AudioSource[] EffectSources;

    int EffectIndex = 0;

    void Awake()
    {
        object[] temp = Resources.LoadAll("Sounds");
        for(int i = 0; i < temp.Length; i++)
        {
            AudioClip clip = temp[i] as AudioClip;
            SoundLists.Add(clip.name, clip);
        }

        EffectSources = new AudioSource[Constants.MAX_EFFECT_LAYER];
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

        if (clip == null || BGMSource == null)
            return;

        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.volume = GameManager.Inst().DatManager.GameData.BGMVolume;
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

        if (clip == null || EffectSources[EffectIndex] == null)
            return;

        EffectSources[EffectIndex].PlayOneShot(clip, GameManager.Inst().DatManager.GameData.EffectVolume);

        EffectIndex++;
        if (EffectIndex > Constants.MAX_EFFECT_LAYER)
            EffectIndex = 0;
    }
}
