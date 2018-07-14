using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Util
{
    public class TargetPlayer : MonoBehaviour
    {

        void Start()
        {
            var player = Utils.getSingleton<PlayerController>();
            var moveComponent = GetComponent<MoveRigidbodyTowards>();
            moveComponent.Direction = player.transform.position - transform.position;
            moveComponent.Direction = new Vector3(moveComponent.Direction.x, 0f, moveComponent.Direction.z);
        }

    }
}