using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
	public Transform ball;
	public Vector2 ballVelocity;
	public float predictionTime = 5f;
	public int maxPredictedBounces = 5;
	public LineRenderer lineRenderer;

	void Update()
	{
		SimulateTrajectory();
	}

	void SimulateTrajectory()
	{
		Vector2 currentPosition = ball.position;
		Vector2 currentVelocity = ballVelocity;
		float time = 0f;

		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, currentPosition);

		for (int i = 0; i < maxPredictedBounces; i++)
		{
			if (time >= predictionTime)
				break;

			RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentVelocity.normalized, currentVelocity.magnitude * (predictionTime - time));
            
			if (hit.collider != null)
			{
				time += hit.distance / currentVelocity.magnitude;
				currentPosition = hit.point;

				lineRenderer.positionCount++;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);

				currentVelocity = Vector2.Reflect(currentVelocity, hit.normal);
			}
			else
			{
				currentPosition += currentVelocity * (predictionTime - time);
				lineRenderer.positionCount++;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
				break;
			}
		}
	}
}