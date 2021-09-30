using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSlots : MonoBehaviour
{
    void ShowResult()
    {
        GameManager.Inst().UiManager.MainUI.Center.Synthesis.ShowResultWindow();
    }
}
