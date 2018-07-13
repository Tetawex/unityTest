using Assets.Scripts.Interface;
using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Controller

{
    public class GenericDamageableEntityController : MonoBehaviour, IDamageableEntity
    {
        [SerializeField]
        private GameObject hitPrefab;
        [SerializeField]
        private AudioSource hitSound;

        private Animator animator;
        private Collider collider;

        public void ReceiveShot(Shot shot)
        {
            animator.SetTrigger("Destroy");
            hitSound.Play();

            var rotation = Quaternion.LookRotation(shot.OriginPoint).eulerAngles;
            rotation.x = 0;
            Instantiate(hitPrefab, shot.ImpactPoint, Quaternion.Euler(rotation));
            collider.enabled = false;
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
