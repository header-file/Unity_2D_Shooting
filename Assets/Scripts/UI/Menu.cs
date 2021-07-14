using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Option Option;


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
        GameManager.Inst().UiManager.OnClickUploadDataBtn();
    }

    public void OnClickDownloadBtn()
    {
        GameManager.Inst().UiManager.OnClickDownloadDataBtn();
    }
}
