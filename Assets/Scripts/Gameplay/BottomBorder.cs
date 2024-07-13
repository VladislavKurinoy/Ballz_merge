using System;
using UnityEngine;

public class BottomBorder : MonoBehaviour
{
	public Action<Vector2> OnBallCollided;
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 ballPosition = collision.gameObject.transform.position;
		Destroy(collision.gameObject);
		OnBallCollided?.Invoke(ballPosition);
	}
}
