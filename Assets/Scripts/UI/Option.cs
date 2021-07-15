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

        BgmMute.isOn = GameManager.Inst().SodManager.IsBgmMute;
        EffectMute.isOn = GameManager.Inst().SodManager.IsEffectMute;

        Bgm.value = GameManager.Inst().SodManager.BgmVolume;
        BgmVolume.text = ((int)(GameManager.Inst().SodManager.BgmVolume * 100)).ToString();

        Effect.value = GameManager.Inst().SodManager.EffectVolume;
        EffectVolume.text = ((int)(GameManager.Inst().SodManager.EffectVolume * 100)).ToString();
    }

    public void IsBgmMute()
    {
        bool isMute = !BgmMute.isOn;
        BgmMute.isOn = isMute;
        GameManager.Inst().SodManager.IsBgmMute = isMute;
    }

    public void IsEffectMute()
    {
        bool isMute = !EffectMute.isOn;
        EffectMute.isOn = isMute;
        GameManager.Inst().SodManager.IsEffectMute = isMute;
    }

    public void OnClickBgmVolumeBtn(bool IsAdd)
    {
        if (IsAdd)
            GameManager.Inst().SodManager.BgmVolume += 0.01f;
        else
            GameManager.Inst().SodManager.BgmVolume -= 0.01f;

        Bgm.value = GameManager.Inst().SodManager.BgmVolume;
        BgmVolume.text = ((int)(GameManager.Inst().SodManager.BgmVolume * 100)).ToString();
    }

    public void OnClickEffectVolumeBtn(bool IsAdd)
    {
        if (IsAdd)
            GameManager.Inst().SodManager.EffectVolume += 0.01f;
        else
            GameManager.Inst().SodManager.EffectVolume -= 0.01f;

        Effect.value = GameManager.Inst().SodManager.EffectVolume;
        EffectVolume.text = ((int)(GameManager.Inst().SodManager.EffectVolume * 100)).ToString();
    }

    public void BgmChange()
    {
        if (Bgm.value == 0.5f)
            return;

        GameManager.Inst().SodManager.BgmVolume = Bgm.value;
        BgmVolume.text = ((int)(GameManager.Inst().SodManager.BgmVolume * 100)).ToString();
    }

    public void EffectChange()
    {
        if (Effect.value == 0.5f)
            return;

        GameManager.Inst().SodManager.EffectVolume = Effect.value;
        EffectVolume.text = ((int)(GameManager.Inst().SodManager.EffectVolume * 100)).ToString();
    }

    public void OnClickBackBtn()
    {
        gameObject.SetActive(false);
    }
}
