using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;

public class CursorController : MonoBehaviour
{
    public static float sensitivity = 1f;
    public static float movementAcc = 1.2f;

    [SerializeField]
    private float movementMult = 10f;
    [SerializeField]
    private float movementMultOverEnemy = 2f;
    [SerializeField]
    private float movementMultOverEnemyFocus = 2f;
    [SerializeField]
    private float relativeFollowSpeed = .1f;
    [SerializeField]
    private float relativeFollowSpeedEnemy = .02f;
    [SerializeField]
    private float followSpeedAcc = .1f;
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color enemyColor = Color.white;
    [SerializeField]
    private Image image;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private CanvasScaler parentScaler;

    public bool isOverEnemy;
    private float currentFollowSpeed;

    private static Vector2 lastPosition = Vector2.zero;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = lastPosition;
        parentCanvas = transform.parent.GetComponent<Canvas>();
        parentScaler = transform.parent.GetComponent<CanvasScaler>();
        currentFollowSpeed = relativeFollowSpeed;
    }

    void LateUpdate()
    {
        // Update position
        var canvasResolution = parentScaler.referenceResolution;
        var cursorPos = CanvasPosition;

        var timeController = Utils.getSingleton<TimeController>();
        var currentMovementMult =
            isOverEnemy
            ? (
                (timeController != null && timeController.IsFocusing)
                ? movementMultOverEnemyFocus
                : movementMultOverEnemy)
            : movementMult;

        //var newSensitivity = parentScaler.referenceResolution.y / 180f;
        //currentMovementMult *= newSensitivity;
        currentMovementMult *= sensitivity;

        var inputVector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            inputVector /= 2f;
        if (inputVector.x != 0f)
            cursorPos.x += Mathf.Pow(Mathf.Abs(inputVector.x), movementAcc) * currentMovementMult * Mathf.Sign(inputVector.x);
        if (inputVector.y != 0f)
            cursorPos.y += Mathf.Pow(Mathf.Abs(inputVector.y), movementAcc) * currentMovementMult * Mathf.Sign(inputVector.y);

        cursorPos.y = Mathf.Clamp(cursorPos.y, -canvasResolution.y / 2f, canvasResolution.y / 2f);
        var xMult = (float)Screen.width / (float)Screen.height;
        cursorPos.x = Mathf.Clamp(cursorPos.x, -canvasResolution.y * xMult / 2f, canvasResolution.y * xMult / 2f);
        rectTransform.anchoredPosition = cursorPos;

        lastPosition = rectTransform.anchoredPosition;


        //var currentPos = CanvasPosition;
        //var goalPos = CanvasPositionRaw;
        //currentFollowSpeed = Mathf.MoveTowards(currentFollowSpeed, isOverEnemy ? relativeFollowSpeedEnemy : relativeFollowSpeed, followSpeedAcc);
        //currentPos = Vector3.MoveTowards(currentPos, goalPos, Time.unscaledDeltaTime * canvasResolution.y * (goalPos - currentPos).magnitude
        //    * currentFollowSpeed);
        //CanvasPosition = currentPos;

        image.color = isOverEnemy ? enemyColor : normalColor;
    }

    void getRelativeMovementMult()
    {

    }

    public Vector2 CanvasPosition
    {
        get { return rectTransform.anchoredPosition; }
        set { rectTransform.anchoredPosition = value; }
    }
    public Vector2 CanvasPositionRaw => ScreenToCanvasPosition(Input.mousePosition);

    public Vector2 ScreenPosition
    {
        get { return CanvasToScreenPosition(rectTransform.anchoredPosition); }
        set { rectTransform.anchoredPosition = ScreenToCanvasPosition(value); }
    }
    public Vector2 ScreenPositionRaw => Input.mousePosition;


    //Vector2 GetFractionalPosition() => rectTransform.anchoredPosition / (parentScaler.referenceResolution / 2f);

    public Vector2 ScreenToCanvasPosition(Vector2 screenPosition)
    {
        var screenRes = new Vector2(Screen.width, Screen.height);
        var uvPos = (screenPosition - (screenRes / 2f));
        uvPos /= (screenRes / 2f);
        //return Vector2.Lerp(-parentScaler.referenceResolution / 2f, parentScaler.referenceResolution / 2f,
        //    screenPosition / screenRes);
        return uvPos * (parentScaler.referenceResolution / 2f);

    }

    Vector2 CanvasToScreenPosition(Vector2 canvasPosition)
    {
        var uvPos = canvasPosition;
        uvPos /= (parentScaler.referenceResolution / 2f);
        var screenRes = new Vector2(Screen.width, Screen.height);
        return (uvPos * (screenRes / 2f)) + (screenRes / 2f);
    }
}
