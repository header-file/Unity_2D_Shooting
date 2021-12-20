using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSlot : MonoBehaviour
{
    public GameObject PlanetImage;
    public Image[] Images;
    public Text Name;
    public Text Stage;
    public GameObject Lock;

    void Awake()
    {
        SetLock(true);
    }

    void Update()
    {
        //if(Name.gameObject.activeSelf)
        //{
        //    Vector3 pos = PlanetImage.GetComponent<RectTransform>().anchoredPosition;
        //    //pos.y -= 200.0f * PlanetImage.transform.localScale.x;
        //    Name.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        //}
            
    }

    public void SetLock(bool b)
    {
        Lock.SetActive(b);

        for (int i = 0; i < Images.Length; i++)
            Images[i].color = Color.black;
    }
}
