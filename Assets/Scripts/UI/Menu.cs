using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Option Option;
    public GameObject Credit;


    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);

        gameObject.SetActive(false);
    }

    public void OnClickOptionBtn()
    {
        Option.ShowSound();
    }

    public void OnClickUploadBtn()
    {
        GameManager.Inst().DatManager.UploadSaveData();
    }

    public void OnClickDownloadBtn()
    {
        GameManager.Inst().DatManager.DownloadSaveData();
    }

    public void OnClickCreditBtn()
    {
        Credit.SetActive(true);
    }

    public void OnClickCreditBackBtn()
    {
        Credit.SetActive(false);
    }
}
