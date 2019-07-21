using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using Assets.Scripts.Util;

public class ProjectileGraze : MonoBehaviour
{
    [SerializeField]
    private float maxDistance = 3f;
    [SerializeField]
    private float grazeSpeedAtZero = 3f;

    private Collider playerCollider;

    void Start()
    {
        playerCollider = Utils.getSingleton<PlayerController>().GetComponent<Collider>();
    }
    
    void Update()
    {
        var playerPos = playerCollider.transform.position;// + playerCollider.bounds.center;
        var distanceToPlayer = (playerPos - transform.position).magnitude;
        TimeController.CurrentGraze += Mathf.Lerp(maxDistance, 0f, distanceToPlayer / maxDistance) * Time.deltaTime;
    }
}
