using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float focusMult = .5f;

    float initialTimescale;

    void Start()
    {
        initialTimescale = 1f;
    }
    
    void Update()
    {
        Time.timeScale = (Input.GetMouseButton(1) ? focusMult * initialTimescale : initialTimescale);
    }
}
