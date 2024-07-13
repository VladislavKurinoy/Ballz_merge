using System;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
	public Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

	public GameEvent OnMouseButtonPressed;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnMouseButtonPressed?.Invoke();
		}
	}
}
