using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    //public GameObject[] Buttons;
    //public GameObject[] Gauges;
    //public Image CurrentBuletImg;
    //public GameObject[] Grades;

    public void SetCurrentBulletImg(Sprite sprite) { /*CurrentBuletImg.sprite = sprite;*/ }

    public void SetButtons(int index, bool b, Sprite img, int grade)
    {
        //if(b)
        //{
        //    Buttons[index].transform.GetChild(2).gameObject.SetActive(false);
        //    Buttons[index].transform.GetChild(1).GetComponent<Image>().sprite = img;

        //    Grades[index].transform.GetChild(grade).gameObject.SetActive(true);
        //}
        //else
        //{
        //    Buttons[index].transform.GetChild(2).gameObject.SetActive(true);
        //    Buttons[index].transform.GetChild(1).GetComponent<Image>().sprite = img;

        //    for (int i = 0; i < 5; i++)
        //        Grades[index].transform.GetChild(i).gameObject.SetActive(false);
        //}
    }

    public void PaintGauge(int type, float value, Color color)
    {
        //for (int i = 1; i <= 10; i++)
        //{
        //    if (value >= i * 10)
        //        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
        //    else
        //        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
        //}
    }

    public void FlickerGauge(int before, int after, int type, Color gaugeColor, float alpha)
    {
        //for(int i = 1; i <= 10; i++)
        //{
        //    if (before >= i && after >= i)
        //        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = gaugeColor;
        //    else if (before < i && after < i)
        //        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
        //    else if (before < i || after < i)
        //    {
        //        Color color = gaugeColor;
        //        color.a = alpha;
        //        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
        //    }
        //}
    }

    public void OnClickButton(int index)
    {
        //GameManager.Inst().UiManager.OnClickEquipSlotBtn(index);
    }
}
