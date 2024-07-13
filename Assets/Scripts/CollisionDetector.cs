using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
	public Action<Vector2> OnTryToMoveBlock;
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 direction = GetCollisionDirection(collision);
		OnTryToMoveBlock?.Invoke(direction);
	}

	Vector2 GetCollisionDirection(Collision2D collision)
	{
		ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
		collision.GetContacts(contacts);

		foreach (ContactPoint2D contact in contacts)
		{
			Vector2 normal = contact.normal;
			if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
			{
				if (normal.x > 0)
				{
					return Vector2.right;
				}
				else
				{
					return Vector2.left;
				}
			}
			else
			{
				if (normal.y > 0)
				{
					return Vector2.up;
				}
				else
				{
					return Vector2.down;
				}
			}
		}

		return Vector2.zero;
	}
}