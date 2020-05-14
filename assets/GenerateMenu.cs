using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateMenu : MonoBehaviour
{
    public void SetScale (float scale)
    {
        Debug.Log(scale);
    }

    public void SetParameters()
    {
        float sizeSliderGet = GameObject.Find("Scale Slider").GetComponent<Slider>().value;

        StaticValue.xSize = Mathf.RoundToInt(sizeSliderGet * StaticValue.xSize);
        StaticValue.zSize = Mathf.RoundToInt(sizeSliderGet * StaticValue.zSize);

        TMP_Dropdown dropdown = GameObject.Find("PortType Dropdown").GetComponent<TMP_Dropdown>();
        StaticValue.portType = dropdown.options[dropdown.value].text;

    }
}
