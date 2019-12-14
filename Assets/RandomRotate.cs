using UnityEngine;

namespace Utils
{
	public class RandomRotate : MonoBehaviour
	{
		[SerializeField] Vector3 spd;
		[SerializeField] Vector3 limit;

		void Update()
		{
			var t = Time.time * spd;
			transform.eulerAngles = new Vector3(
				(Mathf.PerlinNoise(t.x + 0, t.x + 0) - 0.5f) * limit.x,
				(Mathf.PerlinNoise(t.y + 1, t.y + 1) - 0.5f) * limit.y,
				(Mathf.PerlinNoise(t.z + 2, t.z + 2) - 0.5f) * limit.z);
		}
	}
}