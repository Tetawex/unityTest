using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Vector2 xBounds;
    
	void Start ()
    {
		
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
            var newX = transform.position.x + direction * moveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, xBounds.x, xBounds.y);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
