using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Utils
{
	public class Singleton<T>
	{
		static T instance;
		protected Singleton()
		{
		}
		public static T Instance
		{
			get
			{
				if (instance == null)
					instance = Activator.CreateInstance<T>();
				return instance;
			}
		}
	}

	public abstract class SingletonMB<T> : Utils.CustomBehaviour where T : MonoBehaviour
	{
		protected static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
					Init(true);
				return instance;
			}
			protected set
			{
				instance = value;
			}
		}

		protected static void Init(bool isPersistent)
		{
			var t = typeof(T);
			var results = FindObjectsOfType<T>();
			Assert.IsFalse(1 < results.Length, $"[SingletonMB]{t}が{results.Length}つ存在しています。");
			if (0 < results.Length)
				instance = results[0];
			if (instance == null)
			{
				var singleton = new GameObject("(singleton) " + t);
				instance = singleton.AddComponent<T>();
				if (isPersistent)
				DontDestroyOnLoad(singleton);
#if DEBUG
				Debug.LogWarning($"[SingletonMB]{t}を作成しました。");
#endif
			}
#if DEBUG
			else
			{
				Debug.LogWarning($"[SingletonMB]{t}は既に存在しています。" + instance.gameObject.name);
			}
#endif
		}
	}

	public class SingletonMBForScene<T> : SingletonMB<T> where T : Utils.CustomBehaviour
	{
		new public static T Instance
		{
			get
			{
				if (instance == null)
					Init(false);
				return instance;
			}
			protected set
			{
				instance = value;
			}
		}
	}
}