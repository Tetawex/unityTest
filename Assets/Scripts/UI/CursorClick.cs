using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorClick : MonoBehaviour
{
    private Button currentButton;
    private Collider2D col2D;

    private void Start()
    {
        col2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        //print(currentButton);
        if (Input.GetMouseButtonDown(0) && currentButton != null && currentButton.isActiveAndEnabled && currentButton.interactable)
        {
            print("aaa");
            currentButton.onClick.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrigger(collision, true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentButton == null)
            HandleTrigger(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleTrigger(collision, false);
    }

    void HandleTrigger(Collider2D collision, bool inside)
    {
        print(collision);
        print(inside);
        var colliderButton = collision.GetComponent<Button>();
        if (colliderButton != null)
        {
            if (inside)
                currentButton = colliderButton;
            else if (colliderButton == currentButton)
                colliderButton = null;
        }
        else
        {
            Physics2D.IgnoreCollision(col2D, collision);
        }
        print(currentButton);
    }
}
