using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndPanel : MonoBehaviour
{
    public Text title;

    public void SetTitle(bool isComplete)
    {
        if (isComplete)
        {
            title.text = "Success!";
        }
        else
        {
            title.text = "Failed";
        }
    }
}
