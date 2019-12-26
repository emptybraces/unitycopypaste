using UnityEngine;

public class TestWindowMode : MonoBehaviour
{
	[SerializeField] Vector2Int screenSize;
	void Start()
	{
		Screen.SetResolution(screenSize.x, screenSize.y, false);
	}
	public void OnFulled()
	{
		StandaloneManager.Instance.SetResolution(screenSize.x, screenSize.y, StandaloneManager.WindowMode.Fullscreen);
	}
	public void OnBordered()
	{
		StandaloneManager.Instance.SetResolution(screenSize.x, screenSize.y, StandaloneManager.WindowMode.Borderless);
	}
	public void OnWindowed()
	{
		StandaloneManager.Instance.SetResolution(screenSize.x, screenSize.y, StandaloneManager.WindowMode.Windowed);
	}
}