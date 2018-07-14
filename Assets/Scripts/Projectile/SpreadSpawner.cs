using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

public class SpreadSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private float angleSpread;
    [SerializeField]
    private float mainSpeed;
    [SerializeField]
    private float sideSpeed;

    void Start ()
    {
        Vector3 direction = Utils.getSingleton<PlayerController>().transform.position - transform.position;
        float flatAngle = new Vector2(direction.x, direction.z).getAngle();

        fireShot(flatAngle, mainSpeed);
        fireShot(flatAngle - angleSpread, sideSpeed);
        fireShot(flatAngle + angleSpread, sideSpeed);
	}

    void fireShot(float flatAngle, float speed)
    {
        var flatVector = MathHelper.getVector2FromAngle(flatAngle, 1f);
        var direction = new Vector3(flatVector.x, 0f, flatVector.y);
        var projectile = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<MoveRigidbodyTowards>();
        projectile.Direction = direction;
        projectile.Multiplier = speed;
    }
}
