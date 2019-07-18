using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlider : MonoBehaviour
{
    [SerializeField]
    private Setting setting;

    public enum Setting
    {
        mouseSensitivity,
        mouseAcc
    }

    [SerializeField]
    private Vector2 xExtremes;
    [SerializeField]
    private Vector2 valueExtremes;

    private Vector3 initialLeft;
    private Vector3 initialRight;

    private RectTransform mouseTransform = null;
    private bool grabbed;
    Transform initialParent;

    void Start()
    {
        SetX(xExtremes.x);
        initialLeft = transform.position;
        SetX(xExtremes.y);
        initialRight = transform.position;

        initialParent = transform.parent;
        var xPos = Mathf.InverseLerp(valueExtremes.x, valueExtremes.y, GetValue());
        xPos = Mathf.Lerp(xExtremes.x, xExtremes.y, xPos);
        SetX(xPos);
    }
    
    void LateUpdate()
    {
        if (grabbed)
        {
            //print(mouseTransform.name);
            //print(mouseTransform.anchoredPosition.x);
            //SetX(Mathf.Clamp(mouseTransform.anchoredPosition.x, xExtremes.x, xExtremes.y));
            var pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, initialLeft.x, initialRight.x);
            pos.y = initialLeft.y;
            transform.position = pos;
        }

        if (!grabbed && mouseTransform != null && Input.GetMouseButtonDown(0))
            StartGrab();
        else if (grabbed && Input.GetMouseButtonUp(0))
            FinishGrab();
    }

    void StartGrab()
    {
        grabbed = true;
        transform.SetParent(mouseTransform);
        transform.SetSiblingIndex(0);
    }

    void FinishGrab()
    {
        grabbed = false;
        transform.SetParent(initialParent);
        var value = Mathf.InverseLerp(xExtremes.x, xExtremes.y, getX());
        value = Mathf.Lerp(valueExtremes.x, valueExtremes.y, value);
        SetValue(value);
    }

    float getX() => ((RectTransform)transform).anchoredPosition.x;

    void SetX(float x)
    {
        var pos = ((RectTransform)transform).anchoredPosition;
        ((RectTransform)transform).anchoredPosition = new Vector2(x, pos.y);
    }

    void SetValue(float value)
    {
        switch (setting)
        {
            case (Setting.mouseSensitivity):
                CursorController.sensitivity = value;
                break;
            case (Setting.mouseAcc):
                CursorController.movementAcc = value;
                break;
            default:
                break;
        }
    }

    float GetValue()
    {
        switch (setting)
        {
            case (Setting.mouseSensitivity):
                return CursorController.sensitivity;
            case (Setting.mouseAcc):
                return CursorController.movementAcc;
            default:
                return 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mouseTransform = (RectTransform)collision.transform.parent;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        mouseTransform = (RectTransform)collision.transform.parent;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mouseTransform = null;
    }
}
