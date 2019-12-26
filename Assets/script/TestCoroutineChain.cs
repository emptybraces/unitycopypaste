using UnityEngine;

public class TestCoroutineChain : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] Transform[] points;
	int i;
	Utils.Coroutine.Cancelator cancelator = new Utils.Coroutine.Cancelator();
	void Update()
	{
		if (cancelator.Running)
		{
			if (Input.anyKeyDown)
				cancelator.Stop();
			return;
		}
		i = (i + 1) % 3;
		var rand = Random.Range(0, 3);
		// Debug.Log($"{i}, {rand}");
		var e_type = (Utils.TweenEasing)Random.Range(0, (int)Utils.TweenEasing.InOutBounce + 1);
		switch (rand)
		{
			case 0:
				var p = target.position;
				Utils.Coroutine.Chain(cancelator,
					Utils.Coroutine.Lerp01(.5f, t =>
					{
						target.position = Vector3.Lerp(p, points[i].position, t);
					}, easing: e_type),
					Utils.Coroutine.WaitForSeconds(.5f)
				);
				break;
			case 1:
				var r = target.rotation;
				Utils.Coroutine.Chain(cancelator,
					Utils.Coroutine.Lerp01(.5f, t =>
					{
						target.rotation = Quaternion.Lerp(r, points[i].rotation, t);
					}, easing: e_type),
					Utils.Coroutine.WaitForSeconds(.5f)
				);
				break;
			case 2:
				Utils.Coroutine.Chain(cancelator,
					Utils.Coroutine.Lerp(0, Mathf.PI, .5f, t =>
					{
						target.localScale = Vector3.one * Mathf.Cos(t);
					}, easing: e_type),
					Utils.Coroutine.WaitForSeconds(.5f)
				);
				break;
		}
	}
}