using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Animator BgAnim;

    void TurretDisappear()
    {
        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.MainUI.Center.Turrets[i].gameObject.SetActive(false);
    }

    void TurretAppear()
    {
        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.MainUI.Center.Turrets[i].gameObject.SetActive(true);
    }

    void Warning()
    {
        //GameManager.Inst().UiManager.BossWarning();
    }
}
