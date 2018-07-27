using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class MoveTransformTowards : MonoBehaviour
    {
        [SerializeField]
        private Vector3 direction;
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        [SerializeField]
        private float multiplier;
        public float Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //rigidbody.MovePosition(transform.position + (direction.normalized * multiplier * Time.deltaTime));
            //rigidbody.velocity = direction * multiplier;
            transform.position += direction * multiplier * Time.deltaTime;
        }
    }
}
