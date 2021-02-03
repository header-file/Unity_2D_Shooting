using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSlot : MonoBehaviour
{
    public Image PlanetImage;
    public Text Name;

    void Start()
    {
        Name.gameObject.SetActive(false);
    }
}
