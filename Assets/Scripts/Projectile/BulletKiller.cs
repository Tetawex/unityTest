using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

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
        {
            other.GetComponent<ProjectileController>().harmless = true;
        }
    }
}
