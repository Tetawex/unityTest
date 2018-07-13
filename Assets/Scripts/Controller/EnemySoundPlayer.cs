using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip shotClip;
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
        audioSource.PlayOneShot(shotClip, volumeScale);
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
        audioSource.PlayOneShot(shotClip, volumeScale);
    }
}
