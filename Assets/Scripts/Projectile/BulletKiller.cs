using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKiller : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile"))
            Destroy(other.gameObject);
    }
}
