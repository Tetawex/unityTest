using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using Assets.Scripts.Util;

namespace Assets.Scripts.Projectile
{
    public class ProjectileShrink : MonoBehaviour
    {
        [SerializeField]
        private float fadeTime = .2f;

        private Vector3 initialScale;
        private float timeRemaining;
        private LevelController levelController;
        private bool started;
        private SpriteRenderer spriteRenderer;
        
        void Start()
        {
            levelController = Utils.getSingleton<LevelController>();
            initialScale = transform.localScale;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        void Update()
        {
            if (!started && !levelController.FightActive)
            {
                started = true;
                timeRemaining = fadeTime;
            }
            if (started)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0f)
                    Destroy(gameObject);
                else
                {
                    transform.localScale = initialScale * (timeRemaining / fadeTime);
                    if (spriteRenderer != null)
                    {
                        Color c = spriteRenderer.color;
                        c.a = timeRemaining / fadeTime;
                        spriteRenderer.color = c;
                    }
                }
            }
            else
                initialScale = transform.localScale;
        }
    }
}