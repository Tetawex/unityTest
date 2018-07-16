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

    private bool useTiltControls = true;

    float snapBackTimer;

    LevelController levelController;
    PlayerController playerController;

    public Camera camera;

    void Start()
    {
        useTiltControls = PlatformTypeUtil.IsMobileplatform();

        levelController = Utils.getSingleton<LevelController>();
        playerController = Utils.getSingleton<PlayerController>();
        initialPosition = transform.position;
        camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Debug.Log("Reeee " + Input.acceleration);
        if (playerController.Dead)
            return;

        float direction = 0f;
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
            if (Input.GetKey(KeyCode.A))
                direction -= 1f;
            if (Input.GetKey(KeyCode.D))
                direction += 1f;
        }
        if (EnableMovement && levelController.FightActive)
        {
            if (direction != 0f)
            {
                //if (!levelController.FightActive)
                //    playerController.Draw();

                var newX = transform.position.x + direction * moveSpeed * Time.deltaTime;
                newX = Mathf.Clamp(newX, xBounds.x, xBounds.y);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

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
    }
}
