using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

	public static CameraShake instance;

	public float xShake, yShake, shakeSpeed, shakeCoolRate = 1f;
	private Vector3 initialRotation;
    private Vector3 currentRotation;
    private Vector3 goalRotation;
    private float initialXShake, initialYShake;

	void Awake()
	{
		instance = this;

		initialRotation = transform.localEulerAngles;
        initialXShake = xShake;
        initialYShake = yShake;
		goalRotation = currentRotation = initialRotation;
	}
	
	void LateUpdate ()
	{	
		updateScreenshake();
	}

	/// <summary>
	/// Set the amplitude for the screen to shake
	/// </summary>
	/// <param name="shake"></param>
	public void setScreenShake(float shake)
	{
		xShake = yShake = shake;
	}

	/// <summary>
	/// Adds to the screenshake amplitude instead of overriding it
	/// </summary>
	/// <param name="shake"></param>
	public void addScreenShake(float shake)
	{
		xShake += shake;
		yShake += shake;
	}

    /// <summary>
    /// Resets shake x and y values to initial values;
    /// </summary>
    public void resetShake()
    {
        xShake = initialXShake;
        yShake = initialYShake;
    }

	/// <summary>
	/// Centers camera and stops shake
	/// </summary>
	public void reset()
	{
		xShake = yShake = shakeSpeed = 0f;
		shakeCoolRate = 1f;
		transform.localEulerAngles = initialRotation;
		goalRotation = initialRotation;
	}

	void resetGoal()
	{
		goalRotation = new Vector3(Random.Range(-1f * xShake, xShake), Random.Range(-1f * yShake, yShake), initialRotation.z);
	}

	void updateScreenshake()
	{
        initialRotation = Vector3.zero;
		if (xShake == 0f && yShake == 0f && transform.localEulerAngles == goalRotation)
			return;

		if (shakeSpeed <= 0f)
		{
			resetGoal();
			transform.localEulerAngles = goalRotation;
		}
		else if ((xShake + yShake > 0f))
		{
            currentRotation = Vector3.MoveTowards(currentRotation, goalRotation, shakeSpeed * Time.deltaTime);
            transform.localEulerAngles = currentRotation;
            if (currentRotation.Equals(goalRotation)) 
                resetGoal();
		}


		if (xShake > 0f)
		{
			xShake -= shakeCoolRate * Time.deltaTime;
			xShake = Mathf.Max(xShake, 0f);
		}

		if (yShake > 0f)
		{
			yShake -= shakeCoolRate * Time.deltaTime;
			yShake = Mathf.Max(yShake, 0f);
		}


		if (xShake + yShake == 0f)
		{
			goalRotation = initialRotation;
            currentRotation = Vector3.MoveTowards(currentRotation, goalRotation, shakeSpeed * Time.deltaTime);
            transform.localEulerAngles = currentRotation;
			//transform.moveTowardsLocal2D((Vector2)goalRotation, shakeSpeed);
		}
	}
}
