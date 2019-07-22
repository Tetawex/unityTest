using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

public class BurstSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private int shotCount;
    [SerializeField]
    private float timeBetweenShots;

    private int shotsFired;
    private float shotTimer;
    private Vector3 direction;

    private ProjectileController projectileController;

    void Start ()
    {
        projectileController = GetComponent<ProjectileController>();

        direction = Utils.getSingleton<PlayerController>().transform.position - transform.position;
        direction = new Vector3(direction.x, 0f, direction.z);

        fireShot();
        shotsFired++;
	}
	
	void Update ()
    {
        if (shotsFired >= shotCount || projectileController.enemy.Dead || !Utils.getSingleton<LevelController>().FightActive)
        {
            Destroy(gameObject);
            return;
        }

        shotTimer += Time.deltaTime;
        if (shotTimer >= timeBetweenShots)
        {
            fireShot();
            shotTimer -= timeBetweenShots;
            shotsFired++;
        }
	}

    void fireShot()
    {
        Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<MoveRigidbodyTowards>().Direction = direction;
    }
}
