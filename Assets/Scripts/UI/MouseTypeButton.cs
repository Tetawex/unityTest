using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTypeButton : MonoBehaviour
{
    bool on;

    [SerializeField]
    private Text text;
    [SerializeField]
    [Multiline]
    private string onString;
    [Multiline]
    [SerializeField]
    private string offString;

    // Start is called before the first frame update
    void Start()
    {
        on = CursorController.NativeCursor;
        updateText();
    }

    // Update is called once per frame
    public void Press()
    {
        on = !on;
        CursorController.NativeCursor = !CursorController.NativeCursor;
        updateText();
    }

    void updateText()
    {
        text.text = on ? onString : offString;
    }
}
