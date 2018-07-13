using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Util
{
    public class LookAtPlayer : MonoBehaviour
    {
        private Transform playerTransform;
        // Use this for initialization
        void Start()
        {
            //pls replace with something better than Find()
            playerTransform = GameObject.Find("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(playerTransform.position - transform.position, Vector3.up);
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }
}
