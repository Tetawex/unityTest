using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float focusMult = .5f;
    [SerializeField]
    private float lerpTime = .4f;
    [SerializeField]
    private float cameraZoomedFOV = 65f;

    [SerializeField]
    private float drainSpeed = 15f;
    [SerializeField]
    private float drainSpeedAcc = 30f;
    [SerializeField]
    private float drainSpeedDec = 20f;
    [SerializeField]
    private float drainSpeedShotDelta = 15f;
    [SerializeField]
    private float drainSpeedRechargeDelay = .5f;
    [SerializeField]
    private float rechargeSpeed = 20f;
    [SerializeField]
    private float slowKillDelta = -15f;
    [SerializeField]
    private float slowMissDelta = -20f;
    [SerializeField]
    private float normalKillDelta = 30f;

    float initialTimescale;
    Camera mainCamera;
    private float cameraOriginalFOV;
    float t = 0f;
    private bool buttonPressed = false;
    private PlayerMovement playerMovement;
    private float initialDrainSpeed;
    private float drainSpeedRechargeTimer = 0f;

    private float charge = 100f;
    public float Charge => charge;

    void Start()
    {
        initialTimescale = 1f;
        mainCamera = Camera.main;
        cameraOriginalFOV = mainCamera.fieldOfView;
        playerMovement = Utils.getSingleton<PlayerMovement>();
        initialDrainSpeed = drainSpeed;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && drainSpeedRechargeTimer <= 0f)
        {
            buttonPressed = true;
        }
        if (charge <= 0f || !Input.GetMouseButton(1))
            buttonPressed = false;

        t = Mathf.MoveTowards(t, IsFocusing ? 1f : 0f, Time.unscaledDeltaTime / lerpTime);
        Time.timeScale = Mathf.Lerp(initialTimescale, initialTimescale * focusMult, t);
        mainCamera.fieldOfView = Mathf.Lerp(cameraOriginalFOV, cameraZoomedFOV, t);

        drainSpeedRechargeTimer -= Time.unscaledDeltaTime;
        if (IsFocusing)
        {
            drainSpeed += drainSpeedAcc * Time.unscaledDeltaTime;
            drainSpeedRechargeTimer = drainSpeedRechargeDelay;
        }
        else
            drainSpeed = Mathf.MoveTowards(drainSpeed, initialDrainSpeed, drainSpeedDec * Time.unscaledDeltaTime);
        charge = Mathf.MoveTowards(charge, IsFocusing ? 0f : 100f, (IsFocusing ? drainSpeed : rechargeSpeed) * Time.fixedDeltaTime);
    }

    public void RegisterShot(bool isHit)
    {
        if (IsFocusing)
        {
            charge += isHit ? slowKillDelta : slowMissDelta;
            drainSpeed += drainSpeedShotDelta;
        }
        else
            charge += isHit ? normalKillDelta : 0f;
        charge = Mathf.Clamp(charge, 0f, 100f);
    }

    public bool IsFocusing => buttonPressed && charge > 0f && playerMovement.CanFocus;
}
