using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Util
{
    public class SelfDestructor : MonoBehaviour
    {
        public float Lifespan = 1f;
        private float currentLifespan = 0f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            currentLifespan += Time.deltaTime;
            if (currentLifespan >= Lifespan)
                Destroy(gameObject);
        }
    }
}
