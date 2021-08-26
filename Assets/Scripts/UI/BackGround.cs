using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    void TurretDisappear()
    {
        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.Turrets[i].gameObject.SetActive(false);
    }

    void TurretAppear()
    {
        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.Turrets[i].gameObject.SetActive(true);
    }

    void Warning()
    {
        GameManager.Inst().UiManager.BossWarning();
    }
}
