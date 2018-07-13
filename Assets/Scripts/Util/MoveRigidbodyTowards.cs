using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Util
{
    public class MoveRigidbodyTowards : MonoBehaviour
    {
        [SerializeField]
        private Vector3 direction;
        [SerializeField]
        private float multiplier;

        private Rigidbody rigidbody;

        // Use this for initialization
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            rigidbody.MovePosition(transform.position + direction * multiplier * Time.deltaTime);
            //rigidbody.velocity = direction * multiplier;
            //transform.position += direction * multiplier * Time.deltaTime;
        }
    }
}
