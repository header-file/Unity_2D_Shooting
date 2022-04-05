using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBossclear : MonoBehaviour
{
    public GameObject LevelUp;
    public GameObject LevelMax;
    public Text Before;
    public Text After;
    public GameObject Max;
    public Image BossIcon;
    public Sprite[] BossImages;

    void Start()
    {
        gameObject.SetActive(false);
    }
}
