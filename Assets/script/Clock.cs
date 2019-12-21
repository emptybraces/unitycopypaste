using UnityEngine;

public class Clock : MonoBehaviour
{
	[SerializeField] Transform pivot;
	[SerializeField] Transform hour;
	[SerializeField] Transform min;
	[SerializeField] Transform sec;
	[SerializeField] bool smooth;

	Vector3 posOriginHour;
	Vector3 posOriginMin;
	Vector3 posOriginSec;

	void Start()
	{
		posOriginSec = pivot.position;
		posOriginSec.y += sec.localScale.y / 2;
		posOriginMin = pivot.position;
		posOriginMin.y += min.localScale.y / 2;
		posOriginHour = pivot.position;
		posOriginHour.y += hour.localScale.y / 2;
		Debug.Log(System.DateTime.Now);
		Debug.Log(System.DateTime.Now.Hour);
	}

	void Update()
	{
		var current_time = System.DateTime.Now;
		var time_sec = smooth ? current_time.Second + current_time.Millisecond / 1000f : current_time.Second;
		var angles = Vector3.back * (time_sec / 60f * 360);
		sec.SetPositionAndRotation(
			RotatePointAroundPivot(posOriginSec, pivot.position, angles),
			Quaternion.Euler(angles)
		);
		angles = Vector3.back * (current_time.Minute / 60f * 360);
		min.SetPositionAndRotation(
			RotatePointAroundPivot(posOriginMin, pivot.position, angles),
			Quaternion.Euler(angles)
		);
		angles = Vector3.back * (current_time.Hour / 12f * 360);
		hour.SetPositionAndRotation(
			RotatePointAroundPivot(posOriginHour, pivot.position, angles),
			Quaternion.Euler(angles)
		);
	}
	Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}
}