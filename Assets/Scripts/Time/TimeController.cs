using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

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
    public bool isTransitioning => (IsFocusing && t < .5f) || (!IsFocusing && t > .5f);
    private bool buttonPressed = false;
    private PlayerMovement playerMovement;
    private float initialDrainSpeed;
    private float drainSpeedRechargeTimer = 0f;

    private float charge = 100f;
    public float Charge
    {
        get { return charge; }
        set { charge = value; if (value <= 0f) overdrawn = true; else if (value >= 100f) overdrawn = false; }
    }
    private bool overdrawn;
    public bool Overdrawn => overdrawn;

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
        if (Input.GetMouseButton(1) && drainSpeedRechargeTimer <= 0f && !Overdrawn)
        {
            buttonPressed = true;
        }
        if (Charge <= 0f || !Input.GetMouseButton(1))
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

        var currentRechargeSpeed = Utils.getSingleton<LevelController>().FightActive ? rechargeSpeed : rechargeSpeed * 4f;
        Charge = Mathf.MoveTowards(Charge, IsFocusing ? 0f : 100f, (IsFocusing ? drainSpeed : currentRechargeSpeed) * Time.fixedDeltaTime);
    }

    public void RegisterShot(bool isHit)
    {
        if (IsFocusing)
        {
            Charge += isHit ? slowKillDelta : slowMissDelta;
            drainSpeed += drainSpeedShotDelta;
        }
        else
            Charge += isHit ? normalKillDelta : 0f;
        Charge = Mathf.Clamp(Charge, 0f, 100f);
    }

    public bool IsFocusing => buttonPressed && Charge > 0f && playerMovement.CanFocus;
}
