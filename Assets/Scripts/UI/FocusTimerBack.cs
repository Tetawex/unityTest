using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusTimerBack : MonoBehaviour
{

    [SerializeField]
    private int latency = 20;
    [SerializeField]
    private Image gaugeImage;
    [SerializeField]
    private float speedToFillAmount = 100f;

    private Queue<float> percents;
    private Image image;
    float currentTotalFillAmount;
    float fillGoal;

    void Start()
    {
        percents = new Queue<float>();
        for (int i = 0; i < latency; i++)
        {
            percents.Enqueue(0f);
        }

        image = GetComponent<Image>();
        image.fillAmount = gaugeImage.fillAmount;
        image.fillAmount = 0f;
    }
    
    void FixedUpdate()
    {
        //int runInstances = (int)(1f / Time.timeScale);
        int runInstances = 1;
        for (int i = 0; i < runInstances; i++)
        {
            percents.Enqueue(gaugeImage.fillAmount);
            fillGoal = percents.Dequeue();
            if (gaugeImage.fillAmount <= 0f)
                fillGoal = 0f;
        }
    }

    private void LateUpdate()
    {
        currentTotalFillAmount = Mathf.MoveTowards(currentTotalFillAmount, fillGoal, Time.deltaTime * speedToFillAmount);
        var amountDiff = currentTotalFillAmount - gaugeImage.fillAmount;
        if (amountDiff > 0f)
        {
            var rotation = 180f + (gaugeImage.fillAmount * 360f);
            transform.localEulerAngles = Vector3.forward * rotation;
            image.fillAmount = amountDiff;
        }
        else
            image.fillAmount = 0f;
    }
}
