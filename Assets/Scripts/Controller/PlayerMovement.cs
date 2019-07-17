using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

public class PlayerMovement : MonoBehaviour
{
    private bool enableMovement = true;
    public bool EnableMovement
    {
        get { return enableMovement; }
        set { enableMovement = value; snapBackTimer = snapBackTime; }
    }
    Vector3 initialPosition;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Vector2 xBounds;
    [SerializeField]
    private float snapBackTime;
    [SerializeField]
    private float cameraAngleExtreme;
    [SerializeField]
    private float cursorRotationAdditionMult = 60f;
    [SerializeField]
    private float speedToPointRotation = 90f;
    [SerializeField]
    private float jumpDuration = .5f;
    [SerializeField]
    private float jumpHeight = .5f;
    [SerializeField]
    private AnimationCurve jumpCurve;
    [SerializeField]
    private AnimationCurve jumpLookCurve;
    [SerializeField]
    private float jumpLookExtreme = 40f;
    [SerializeField]
    private AnimationCurve gunRecoilCurve;
    [SerializeField]
    private float gunRecoilTime;
    [SerializeField]
    private float gunRecoilExtreme;

    private float gunRecoilTimer = 999f;
    public void TriggerGunRecoil() { gunRecoilTimer = 0f; }

    private bool useTiltControls = false;

    float snapBackTimer;
    float direction;
    float lastJumpTime = -100f;
    float initialY;

    LevelController levelController;
    PlayerController playerController;
    Vector3 pointRotation;

    public Camera camera;

    public bool IsJumping => Time.time <= lastJumpTime + jumpDuration;

    void Start()
    {
        useTiltControls = PlatformTypeUtil.IsMobileplatform();

        levelController = Utils.getSingleton<LevelController>();
        playerController = Utils.getSingleton<PlayerController>();
        initialPosition = transform.position;
        camera = GetComponentInChildren<Camera>();
        initialY = transform.position.y;
    }

    void Update()
    {
        if (playerController.Dead)
            return;
        
        if (useTiltControls)
        {
            float xAccel = Input.acceleration.x;
            if (xAccel < -0.05f || xAccel > 0.05f)
            {
                direction = 3 * xAccel;
                if (direction > 1)
                    direction = 1f;
            }
        }
        else
        {
            direction = 0f;
            if (Input.GetKey(KeyCode.A))
                direction -= 1f;
            if (Input.GetKey(KeyCode.D))
                direction += 1f;
        }
        if (IsJumping)
        {
            var t = (Time.time - lastJumpTime) / jumpDuration;
            transform.position = new Vector3(transform.position.x,
                initialY + jumpCurve.Evaluate(t) * jumpHeight,
                transform.position.z);
        }
        if (EnableMovement && levelController.FightActive)
        {
            if (!Utils.getSingleton<TimeController>().IsFocusing)
            {
                if (direction != 0f)
                {
                    //if (!levelController.FightActive)
                    //    playerController.Draw();

                    var newX = transform.position.x + direction * moveSpeed * Time.deltaTime;
                    newX = Mathf.Clamp(newX, xBounds.x, xBounds.y);
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                }
                if (!IsJumping && Input.GetKeyDown(KeyCode.Space))
                    Jump();
            }
        }
        else
        {
            snapBackTimer -= Time.deltaTime;
            if (snapBackTimer <= 0f)
            {
                transform.moveTowards(initialPosition, moveSpeed);
            }
        }
        float distance = xBounds.y;
        float cameraAngle = Mathf.Lerp(0f, cameraAngleExtreme, Mathf.Abs(transform.position.x) / distance) * -Mathf.Sign(transform.position.x);
        transform.localEulerAngles = Vector3.up * cameraAngle;

        var screenRes = new Vector2(Screen.width, Screen.height);
        var cursorController = Utils.getSingleton<CursorController>();
        var cursorPos = ((Vector2)cursorController.ScreenPosition - (screenRes / 2f));
        cursorPos /= screenRes / 2f;
        var cursorRot = new Vector3(-cursorPos.y, cursorPos.x) * cursorRotationAdditionMult;

        pointRotation = Vector3.MoveTowards(pointRotation, cursorRot, speedToPointRotation * Time.deltaTime * (cursorRot - pointRotation).magnitude);
        transform.localEulerAngles += pointRotation;

        // Jump animation
        if (IsJumping)
        {
            var t = (Time.time - lastJumpTime) / jumpDuration;
            var jumpAngleAddition = jumpLookCurve.Evaluate(t) * jumpLookExtreme;
            transform.localEulerAngles += Vector3.right * jumpAngleAddition;
        }

        // Recoil animation
        //if (gunRecoilTimer < gunRecoilTime)
        //{
        //    gunRecoilTimer += Time.deltaTime;
        //    var t = gunRecoilTimer / gunRecoilTime;
        //    var recoilAngleAddition = gunRecoilCurve.Evaluate(t) * gunRecoilExtreme;
        //    transform.localEulerAngles += Vector3.right * -recoilAngleAddition;
        //}
    }

    public bool CanFocus => EnableMovement && levelController.FightActive && !IsJumping;


    void Jump()
    {
        lastJumpTime = Time.time;
    }
}
