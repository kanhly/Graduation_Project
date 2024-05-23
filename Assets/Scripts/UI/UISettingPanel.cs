using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : MonoBehaviour
{
    public Slider sensitive;

    public void GetSenitive()
    {
        sensitive.value = MouseController.Instance.sensitivity;
    }

    public void SetSensitive()
    {
        MouseController.Instance.sensitivity = sensitive.value;
    }
}
