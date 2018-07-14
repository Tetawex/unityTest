﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Vector2 xBounds;

    LevelController levelController;
    PlayerController playerController;
    
	void Start ()
    {
        levelController = Utils.getSingleton<LevelController>();
        playerController = Utils.getSingleton<PlayerController>();
    }
	
	void Update ()
    {
        float direction = 0f;
        if (Input.GetKey(KeyCode.A))
            direction -= 1f;
        if (Input.GetKey(KeyCode.D))
            direction += 1f;
        if (direction != 0f)
        {
            if (!levelController.FightActive)
                playerController.Draw();

            var newX = transform.position.x + direction * moveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, xBounds.x, xBounds.y);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
