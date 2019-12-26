using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
	public class Coroutine : SingletonMB<Coroutine>
	{
		[ExposeInspector] public int RunningCount { get; private set; }
		public static void Stop(UnityEngine.Coroutine c)
		{
			if (c != null)
				Instance.StopCoroutine(c);
		}
		public static void StopAll()
		{
			Instance.StopAllCoroutines();
			Instance.RunningCount = 0;
		}
		public static void Chain(params IEnumerator[] actions)
		{
			Instance.StartCoroutine(ChainLocal(actions));
		}
		public static void Chain(Cancelator c, params IEnumerator[] actions)
		{
			c.Interrupt = false;
			c.parent.Add(Instance.StartCoroutine(ChainLocal(c, actions)));
		}
		static IEnumerator ChainLocal(params IEnumerator[] actions)
		{
			foreach (var action in actions)
			{
				++Instance.RunningCount;
				yield return action;
				--Instance.RunningCount;
			}
		}
		static IEnumerator ChainLocal(Cancelator cancel, params IEnumerator[] actions)
		{
			foreach (var action in actions)
			{
				if (cancel.Interrupt)
					break;
				++Instance.RunningCount;
				var c = Instance.StartCoroutine(action);
				cancel.last.Add(c);
				yield return c;
				cancel.last.Remove(c);
				--Instance.RunningCount;
			}
		}
		public static IEnumerator When(params IEnumerator[] actions)
		{
			var l = actions.Length;
			var coroutines = new UnityEngine.Coroutine[l];
			for (int i = 0; i < l; ++i)
			{
				++Instance.RunningCount;
				coroutines[i] = Instance.StartCoroutine(actions[i]);
			}
			for (int i = 0; i < l; ++i)
			{
				yield return coroutines[i];
				--Instance.RunningCount;
			}
		}
		public static IEnumerator WaitForFrame(int n)
		{
			for (int i = 0; i < n; ++i)
				yield return null;
		}
		public static IEnumerator WaitForSeconds(float span, bool unscaled = false, Action then = null)
		{
			if (unscaled)
				yield return new WaitForSecondsRealtime(span);
			else
				yield return new WaitForSeconds(span);
			if (then != null)
				then.Invoke();
		}
		public static IEnumerator Repeat(int repeats, IEnumerator action)
		{
			for (int i = 0; i < repeats; ++i)
			{
				yield return action;
			}
		}
		public static IEnumerator While(Func<bool> func)
		{
			while (func())
				yield return null;
		}
		public static IEnumerator While(float interval, Func<bool> func)
		{
			var wait = new WaitForSeconds(interval);
			while (func())
				yield return wait;
		}
		public static IEnumerator WhileForSeconds(float span, Func<bool> func, bool unscaled = false)
		{
			if (unscaled)
			{
				var until = Time.unscaledTime + span;
				while (func() && Time.unscaledTime < until)
					yield return null;
			}
			else
			{
				var until = Time.time + span;
				while (func() && Time.time < until)
					yield return null;
			}
		}
		public static IEnumerator WhileForSeconds(float interval, float span, Func<bool> func, bool unscaled = false)
		{
			if (unscaled)
			{
				var wait = new WaitForSecondsRealtime(interval);
				var until = Time.unscaledTime + span;
				while (func() && Time.unscaledTime < until)
					yield return wait;
			}
			else
			{
				var wait = new WaitForSeconds(interval);
				var until = Time.time + span;
				while (func() && Time.time < until)
					yield return wait;
			}
		}
		public static IEnumerator If(Func<bool> condition, params IEnumerator[] actions)
		{
			if (condition())
				yield return ChainLocal(actions);
		}
		public static IEnumerator If(Func<bool> condition, Cancelator p, params IEnumerator[] actions)
		{
			if (condition())
				yield return ChainLocal(p, actions);
		}
		public static IEnumerator Then(Action action)
		{
			action();
			yield break;
		}
		public static IEnumerator Lerp01(float span, Action<float> cbUpdate, Action cbFinish = null, TweenEasing easing = TweenEasing.Linear, bool unscaled = false)
		{
			yield return LerpLocal(unscaled, 0, 1, span, cbUpdate, cbFinish, easing);
		}
		public static IEnumerator Lerp10(float span, Action<float> cbUpdate, Action cbFinish = null, TweenEasing easing = TweenEasing.Linear, bool unscaled = false)
		{
			yield return LerpLocal(unscaled, 1, 0, span, cbUpdate, cbFinish, easing);
		}
		public static IEnumerator Lerp(float from, float to, float span, Action<float> cbUpdate, Action cbFinish = null, TweenEasing easing = TweenEasing.Linear, bool unscaled = false)
		{
			yield return LerpLocal(unscaled, from, to, span, cbUpdate, cbFinish, easing);
		}
		public static IEnumerator Lerp(float from, float to, float span, Func<float, bool> cbUpdate, Action cbFinish = null, TweenEasing easing = TweenEasing.Linear, bool unscaled = false)
		{
			yield return LerpLocal(unscaled, from, to, span, cbUpdate, cbFinish, easing);
		}
		public static IEnumerator Lerp(Vector3 from, Vector3 to, float span, Action<Vector3> cbUpdate, Action cbFinish = null, TweenEasing easing = TweenEasing.Linear, bool unscaled = false)
		{
			yield return LerpLocal(unscaled, from, to, span, cbUpdate, cbFinish, easing);
		}
		static IEnumerator LerpLocal(bool isUnscaled, float from, float to, float span, Action<float> cbUpdate, Action cbFinish, TweenEasing easing)
		{
			if (span <= 0f)
			{
				cbUpdate(to);
			}
			else
			{
				var elapsed = 0f;
				var div = 1f / span;
				while (true)
				{
					elapsed += isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
					var t = Mathf.Min(1f, elapsed * div);
					cbUpdate(Tween.Apply(easing, t, from, to - from, 1f));
					if (1f <= t)
						break;
					yield return null;
				}
			}
			cbFinish.InvokeEx();
		}
		static IEnumerator LerpLocal(bool isUnscaled, float from, float to, float span, Func<float, bool> cbUpdate, Action cbFinish, TweenEasing easing)
		{
			if (span <= 0f)
			{
				cbUpdate(to);
			}
			else
			{
				var elapsed = 0f;
				var div = 1f / span;
				while (true)
				{
					elapsed += isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
					var t = Mathf.Min(1f, elapsed * div);
					var r = cbUpdate(Tween.Apply(easing, t, from, to - from, 1f));
					if (1f <= t || !r)
						break;
					yield return null;
				}
			}
			cbFinish.InvokeEx();
		}
		static IEnumerator LerpLocal(bool isUnscaled, Vector3 from, Vector3 to, float span, Action<Vector3> cbUpdate, Action cbFinish, TweenEasing easing)
		{
			if (span <= 0f)
			{
				cbUpdate(to);
			}
			else
			{
				var elapsed = 0f;
				var div = 1f / span;
				while (true)
				{
					elapsed += isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
					var t = Mathf.Min(1f, elapsed * div);
					var x = Tween.Apply(easing, elapsed, from.x, to.x - from.x, span);
					var y = Tween.Apply(easing, elapsed, from.y, to.y - from.y, span);
					var z = Tween.Apply(easing, elapsed, from.z, to.z - from.z, span);
					cbUpdate(new Vector3(x, y, z));
					if (1f <= t)
						break;
					yield return null;
				}
			}
			cbFinish.InvokeEx();
		}
		public class Cancelator : CustomYieldInstruction
		{
			public List<UnityEngine.Coroutine> parent = new List<UnityEngine.Coroutine>();
			public List<UnityEngine.Coroutine> last = new List<UnityEngine.Coroutine>();
			public override bool keepWaiting => Running;
			public bool Interrupt { get; set; }
			public bool Running => 0 < last.Count;
			public void Stop()
			{
				foreach (var i in last)
					if (i != null)
						Instance.StopCoroutine(i);
				last.Clear();
				foreach (var i in parent)
					if (i != null)
						Instance.StopCoroutine(i);
				parent.Clear();
				Interrupt = true;
			}
		}
	}
}