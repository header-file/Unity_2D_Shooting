using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    //t
    public Alarm Alarm;
    public Text CoinText;
    public Text JewelText;

    //플레이어용 UI
    public GameObject PlayerUI;
    public Image PlayerTimerImg;
    public Text PlayerTimerText;
    public GameObject PlayerHPUI;
    public Image PlayerHPBar;
    public GameObject PlayerEquipUI;
    public Image PlayerEquipBar;
    public Image PlayerEquipIcon;

    //보스용 UI
    public GameObject BossHPBarCanvas;
    public Image BossHPBar;
    public Text BossHPBarText;
    public Image BossGaugeBar;

    public BossGauge BossGauge;
    public SideMenu SideMenu;
    public GameObject Resource;

    public Bottom Bottom;
    public Center Center;
    public ZzinBottom ZzinBottom;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        //Player UI Setting
        GameManager.Inst().Player.UI = PlayerUI;
        GameManager.Inst().Player.TimerImage = PlayerTimerImg;
        GameManager.Inst().Player.TimerText = PlayerTimerText;
        GameManager.Inst().Player.HPUI = PlayerHPUI;
        GameManager.Inst().Player.HPBar = PlayerHPBar;
        GameManager.Inst().Player.EquipUI = PlayerEquipUI;
        GameManager.Inst().Player.EquipBar = PlayerEquipBar;
        GameManager.Inst().Player.EquipIcon = PlayerEquipIcon;
    }

    public void OnClickMenu()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        Center.Menu.gameObject.SetActive(true);
    }

    public void OnClickResourceToggleBtn()
    {
        if (Resource.activeSelf)
            Resource.SetActive(false);
        else
        {
            Resource.SetActive(true);

            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 3)
                GameManager.Inst().Tutorials.Step++;
        }
    }
}
