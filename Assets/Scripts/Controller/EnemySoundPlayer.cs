using UnityEngine;

namespace Assets.Scripts.Controller
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemySoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] shotClips;
        [SerializeField]
        private AudioClip drawClip;
        [SerializeField]
        private AudioClip bounceClip;
        [SerializeField]
        private AudioClip hitClip;


        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {

        }

        public void PlayShootSound(float volumeScale = 1)
        {
            audioSource.PlayOneShot(shotClips[Random.Range(0, shotClips.Length)], volumeScale);
        }
        public void PlayHitSound(float volumeScale = 1)
        {
            audioSource.PlayOneShot(hitClip, volumeScale);
        }
        public void PlayBounceSound(float volumeScale = 1)
        {
            audioSource.PlayOneShot(bounceClip, volumeScale);
        }
        public void PlayDrawSound(float volumeScale = 1)
        {
            audioSource.PlayOneShot(drawClip, volumeScale);
        }
    }
}
