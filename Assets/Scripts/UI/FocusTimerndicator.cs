using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;

public class FocusTimerndicator : MonoBehaviour
{
    private Image image;
    public float fillAmount = 1f;


    [SerializeField]
    Gradient colorGradient;
    [SerializeField]
    private Color rechargeColor;

    private TimeController timeController;

    void Start()
    {
        image = GetComponent<Image>();
        timeController = Utils.getSingleton<TimeController>();
    }
    
    void LateUpdate()
    {
        image.fillAmount = timeController.Charge / 100f;
        image.color = timeController.Overdrawn ? rechargeColor : colorGradient.Evaluate(timeController.Charge / timeController.MaxCharge);
    }
}
