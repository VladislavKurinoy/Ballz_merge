using UnityEngine;

public class InputDetector : MonoBehaviour
{
	public GameEvent OnMouseDrag;
	public GameEvent OnMouseButtonPressed;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnMouseButtonPressed?.Invoke();
		}
		
		OnMouseDrag?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}
