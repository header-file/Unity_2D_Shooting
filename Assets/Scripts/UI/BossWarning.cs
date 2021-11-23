using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning : MonoBehaviour
{
    void StopWarning()
    {
        GameManager.Inst().UiManager.MainUI.Center.EndPlayWarning();
    }
}
