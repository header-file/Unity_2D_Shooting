using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Toggle BgmMute;
    public Toggle EffectMute;
    public Slider Bgm;
    public Slider Effect;
    public Text BgmVolume;
    public Text EffectVolume;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowSound()
    {
        gameObject.SetActive(true);

        BgmMute.isOn = GameManager.Inst().DatManager.GameData.IsMuteBGM;
        EffectMute.isOn = GameManager.Inst().DatManager.GameData.IsMuteEffect;

        Bgm.value = GameManager.Inst().DatManager.GameData.BGMVolume;
        BgmVolume.text = ((int)(GameManager.Inst().DatManager.GameData.BGMVolume * 100)).ToString();

        Effect.value = GameManager.Inst().DatManager.GameData.EffectVolume;
        EffectVolume.text = ((int)(GameManager.Inst().DatManager.GameData.EffectVolume * 100)).ToString();
    }

    public void IsBgmMute()
    {
        bool isMute = !BgmMute.isOn;
        BgmMute.isOn = isMute;
        GameManager.Inst().DatManager.GameData.IsMuteBGM = isMute;
    }

    public void IsEffectMute()
    {
        bool isMute = !EffectMute.isOn;
        EffectMute.isOn = isMute;
        GameManager.Inst().DatManager.GameData.IsMuteEffect = isMute;
    }

    public void OnClickBgmVolumeBtn(bool IsAdd)
    {
        if (IsAdd)
            GameManager.Inst().DatManager.GameData.BGMVolume += 0.05f;
        else
            GameManager.Inst().DatManager.GameData.BGMVolume -= 0.05f;

        Bgm.value = GameManager.Inst().DatManager.GameData.BGMVolume;
        BgmVolume.text = ((int)(GameManager.Inst().DatManager.GameData.BGMVolume * 100)).ToString();
    }

    public void OnClickEffectVolumeBtn(bool IsAdd)
    {
        if (IsAdd)
            GameManager.Inst().DatManager.GameData.EffectVolume += 0.05f;
        else
            GameManager.Inst().DatManager.GameData.EffectVolume -= 0.05f;

        Effect.value = GameManager.Inst().DatManager.GameData.EffectVolume;
        EffectVolume.text = ((int)(GameManager.Inst().DatManager.GameData.EffectVolume * 100)).ToString();
    }
}
