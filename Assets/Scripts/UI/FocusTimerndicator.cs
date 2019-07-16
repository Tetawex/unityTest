using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;

public class FocusTimerndicator : MonoBehaviour
{
    private Image image;
    public float fillAmount = 1f;

    void Start()
    {
        image = GetComponent<Image>();
    }
    
    void LateUpdate()
    {
        image.fillAmount = Utils.getSingleton<TimeController>().Charge / 100f;
    }
}
