using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;

public class PopupStageopen : MonoBehaviour
{
    public Animation Anim;
    public Sprite[] PlanetImgs;
    public Animator Player;
    public SpriteResolver WeaponSkin;
    public Image Planet;
    public Text WeaponName;
    public Text PlanetName;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(int stage)
    {
        WeaponSkin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[stage + 5]);
        WeaponName.text = GameManager.Inst().TxtManager.BulletTypeNames[stage + 5];

        int color = GameManager.Inst().ShtManager.BaseColor[stage + 5];
        Player.SetInteger("Color", ++color);

        Planet.sprite = PlanetImgs[stage];
        PlanetName.text = GameManager.Inst().TxtManager.PlanetNames[stage];

        Anim.Play();
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }
}
