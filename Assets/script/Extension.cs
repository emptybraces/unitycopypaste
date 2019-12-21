using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

public static class Extensions
{
	#region Integer

	/// <summary>指定したビットが全て立っているかどうか調べる</summary>
	/// <param name='src'>元の値</param>
	/// <param name='taget'>対象ビット</param>
	public static bool CheckBitAll(this int src, int target) { return (src & target) == target; }

	/// <summary>指定したビットのうち、どれか立っているか調べる</summary>
	/// <param name='src'>元の値</param>
	/// <param name='taget'>対象ビット</param>
	public static bool CheckBitAny(this int src, int target) { return (src & target) != 0; }

	/// <summary>両方向のMathf.Repeat</summary>
	/// <param name='src'>対象値</param>
	/// <param name='min'>最小値</param>
	/// <param name='max'>最大値</param>
	public static int BiRepeat(this int src, int min, int max)
	{
		// min以下の場合
		// 自分でももうよくわからん
		if (src < min)
		{
			var d = min - src;
			var ridx = (d % (max - min + 1)) - 1;
			if (ridx < 0)
				ridx = max - min;
			return Enumerable.Range(min, max - min + 1).Reverse().ElementAt(ridx);
		}
		// min以上の場合
		// min-max範囲の数字の数だけ余剰取って、minからのオフセット値とする
		else
		{
			var d = (src - min) % (max - min + 1);
			return min + d;
		}
	}

	public static int ToCharsNonAlloc(this int self, char[] output, int start = 0)
	{
		if (self == 0)
		{
			output[start] = '0';
			return 1;
		}
		int digitsNum = (int)System.Math.Log10(self) + 1;
		int zero = '0';
		for (int i = digitsNum - 1; i >= 0; i--)
		{
			int digit = self % 10;
			output[start + i] = (char)(digit + zero);
			self /= 10;
		}
		return digitsNum;
	}

	/// <summery>パーセントを加算する</summery>
	public static int PctAdd(this int src, int pct) { return src + (int)(src * pct / 100f + 0.99f * Mathf.Sign(pct)); }
	public static float PctAdd(this float src, float pct) { return src + (float)(src * pct / 100f); }
	/// <summery>パーセントを減産する</summery>
	public static int PctDec(this int src, int pct) { return src - (int)(src * pct / 100f + 0.99f * Mathf.Sign(pct)); }
	public static float PctDec(this float src, float pct) { return src - (float)(src * pct / 100f); }
	/// <summery>パーセントで計算する</summery>
	public static int Pct(this int src, int pct) { return (int)(src * pct / 100f + 0.99f * Mathf.Sign(pct)); }
	public static float Pct(this float src, float pct) { return (float)(src * pct / 100f); }

	#endregion

	#region String

	public static bool Any(this string src) { return !String.IsNullOrEmpty(src); }

	#endregion

	#region Vector

	///// alias
	public static float min(this Vector3 v3) { return v3.x; }
	public static float max(this Vector3 v3) { return v3.y; }
	///// Vector3 extensions
	public static void SetX(this Vector3 v3, float rhs) { v3.Set(rhs, v3.y, v3.z); }
	public static void SetY(this Vector3 v3, float rhs) { v3.Set(v3.x, rhs, v3.z); }
	public static void SetZ(this Vector3 v3, float rhs) { v3.Set(v3.x, v3.y, rhs); }
	public static void SetXY(this Vector3 v3, float rhs1, float rhs2) { v3.Set(rhs1, rhs2, v3.z); }
	public static void SetYZ(this Vector3 v3, float rhs1, float rhs2) { v3.Set(v3.x, rhs1, rhs2); }
	public static void SetXZ(this Vector3 v3, float rhs1, float rhs2) { v3.Set(rhs1, v3.y, rhs2); }
	public static Vector3 xx(this Vector3 v3, float x) { return new Vector3(x, v3.y, v3.z); }
	public static Vector3 yy(this Vector3 v3, float y) { return new Vector3(v3.x, y, v3.z); }
	public static Vector3 zz(this Vector3 v3, float z) { return new Vector3(v3.x, v3.y, z); }
	public static Vector3 xy(this Vector3 v3, float x, float y) { return new Vector3(x, y, v3.z); }
	public static Vector3 xy(this Vector3 v3, Vector2 xy) { return new Vector3(xy.x, xy.y, v3.z); }
	public static Vector3 xz(this Vector3 v3, float x, float z) { return new Vector3(x, v3.y, z); }
	public static Vector3 yz(this Vector3 v3, float y, float z) { return new Vector3(v3.x, y, z); }
	public static Vector3 xyz(this Vector3 v3, float x, float y, float z) { return new Vector3(x, y, z); }
	public static Vector3 ax(this Vector3 v3, float x) { return new Vector3(v3.x + x, v3.y, v3.z); }
	public static Vector3 ay(this Vector3 v3, float y) { return new Vector3(v3.x, v3.y + y, v3.z); }
	public static Vector3 az(this Vector3 v3, float z) { return new Vector3(v3.x, v3.y, v3.z + z); }
	public static Vector3 ayz(this Vector3 v3, float y, float z) { return new Vector3(v3.x, v3.y + y, v3.z + z); }
	public static Vector3 axy(this Vector3 v3, float x, float y) { return new Vector3(v3.x + x, v3.y + y, v3.z); }
	public static Vector3 axy(this Vector3 v3, Vector2 v2) { return new Vector3(v3.x + v2.x, v3.y + v2.y, v3.z); }
	public static Vector3 axz(this Vector3 v3, float x, float z) { return new Vector3(v3.x + x, v3.y, v3.z + z); }
	public static Vector3 mx(this Vector3 v3, float x) { return new Vector3(v3.x * x, v3.y, v3.z); }
	public static Vector3 my(this Vector3 v3, float y) { return new Vector3(v3.x, v3.y * y, v3.z); }
	public static Vector3 mz(this Vector3 v3, float z) { return new Vector3(v3.x, v3.y, v3.z * z); }
	public static Vector3 Nagate(this Vector3 v3) { return new Vector3(-v3.x, -v3.y, -v3.z); }
	public static Vector3 nx(this Vector3 v3) { return new Vector3(-v3.x, v3.y, v3.z); }
	public static Vector3 ny(this Vector3 v3) { return new Vector3(v3.x, -v3.y, v3.z); }
	public static Vector3 nz(this Vector3 v3) { return new Vector3(v3.x, v3.y, -v3.z); }
	public static Vector3 Random(this Vector3 v3, Vector3 r) { return new Vector3(UnityEngine.Random.Range(v3.x, r.x), UnityEngine.Random.Range(v3.y, r.y), UnityEngine.Random.Range(v3.z, r.z)); }
	public static Vector3 Clone(this Vector3 v3) { return new Vector3(v3.x, v3.y, v3.z); }
	public static Vector2 ToVector2(this Vector3 v3) { return new Vector2(v3.x, v3.y); }
	public static Vector3 FixIfNaN(this Vector3 v)
	{
		if (float.IsNaN(v.x))
			v.x = 0;
		if (float.IsNaN(v.y))
			v.y = 0;
		if (float.IsNaN(v.z))
			v.z = 0;
		return v;
	}
	///// Vector2 extensions
	public static float min(this Vector2 v2) { return v2.x; }
	public static float max(this Vector2 v2) { return v2.y; }
	public static Vector2 xx(this Vector2 v2, float x) { return new Vector2(x, v2.y); }
	public static Vector2 yy(this Vector2 v2, float y) { return new Vector2(v2.x, y); }
	public static Vector2 ax(this Vector2 v2, float x) { return new Vector2(v2.x + x, v2.y); }
	public static Vector2 ay(this Vector2 v2, float y) { return new Vector2(v2.x, v2.y + y); }
	public static Vector2 mx(this Vector2 v2, float x) { return new Vector2(v2.x * x, v2.y); }
	public static Vector2 my(this Vector2 v2, float y) { return new Vector2(v2.x, v2.y * y); }
	public static Vector2 Clone(this Vector2 v2) { return new Vector2(v2.x, v2.y); }
	public static bool Between(this Vector2 v2, float t) { return v2.x <= t && t <= v2.y; }
	public static Vector3 ToVector3(this Vector2 v2) { return new Vector3(v2.x, v2.y); }
	public static float Random(this Vector2 v2) { return UnityEngine.Random.Range(v2.x, v2.y); }
	public static int RandomInt(this Vector2 v2) { return UnityEngine.Random.Range((int)v2.x, (int)v2.y); }
	public static float ToAngle(this Vector3 lhs, Vector3 rhs) { return Mathf.Atan2(rhs.y - lhs.y, rhs.x - lhs.x); }
	public static float ToAngleDegree(this Vector3 lhs, Vector3 rhs) { return lhs.ToAngle(rhs) * 180 / Mathf.PI; }

	#endregion

	#region Quaternion

	public static Quaternion LerpEx(Quaternion p, Quaternion q, float t, bool shortWay)
	{
		if (shortWay)
		{
			float dot = Quaternion.Dot(p, q);
			if (dot < 0.0f)
				return LerpEx(ScalarMultiply(p, -1.0f), q, t, true);
		}
		Quaternion r = Quaternion.identity;
		r.x = p.x * (1f - t) + q.x * (t);
		r.y = p.y * (1f - t) + q.y * (t);
		r.z = p.z * (1f - t) + q.z * (t);
		r.w = p.w * (1f - t) + q.w * (t);
		return r;
	}

	public static Quaternion SlerpEx(Quaternion p, Quaternion q, float t, bool shortWay)
	{
		float dot = Quaternion.Dot(p, q);
		if (shortWay)
		{
			if (dot < 0.0f)
				return SlerpEx(ScalarMultiply(p, -1.0f), q, t, true);
		}

		float angle = Mathf.Acos(dot);
		Quaternion first = ScalarMultiply(p, Mathf.Sin((1f - t) * angle));
		Quaternion second = ScalarMultiply(q, Mathf.Sin((t) * angle));
		float division = 1f / Mathf.Sin(angle);
		return ScalarMultiply(Add(first, second), division);
	}

	public static Quaternion ScalarMultiply(Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}

	public static Quaternion Add(Quaternion p, Quaternion q)
	{
		return new Quaternion(p.x + q.x, p.y + q.y, p.z + q.z, p.w + q.w);
	}

	#endregion

	#region Object

	const string SEPARATOR = ","; // 区切り記号として使用する文字列
	const string FORMAT = "{0}:{1}"; // 複合書式指定文字列

	/// <summary>
	/// すべての公開フィールドの情報を文字列にして返します
	/// </summary>
	public static string ToStringFields<T>(this T obj, BindingFlags flags = 0)
	{
		return typeof(T) + " fields are: " + String.Join(SEPARATOR, obj
			.GetType()
			.GetFields(BindingFlags.Instance | BindingFlags.Public | flags)
			.Select(c =>
			{
				var o = c.GetValue(obj);
				return String.Format(FORMAT, c.Name, o);
			})
			.ToArray());
	}

	/// <summary>
	/// すべての公開プロパティの情報を文字列にして返します
	/// </summary>
	public static string ToStringProperties<T>(this T obj, BindingFlags flags = 0)
	{
		return typeof(T) + " props are: " + String.Join(SEPARATOR, obj
			.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public | flags)
			.Where(c => c.CanRead)
			.Select(c => String.Format(FORMAT, c.Name, c.GetValue(obj, null)))
			.ToArray());
	}

	// public static string ToStringMembers<T>(this T obj)
	// {
	// 	return String.Join(SEPARATOR, obj
	// 		.GetType()
	// 		.GetProperties(BindingFlags.Instance | BindingFlags.Public | )
	// 		.Where(c => c.CanRead)
	// 		.Select(c => String.Format(FORMAT, c.Name, c.GetValue(obj, null)))
	// 		.ToArray());		
	// }

	/// <summary>
	/// すべての公開フィールドと公開プロパティの情報を文字列にして返します
	/// </summary>
	public static string ToStringMembers<T>(this T obj, BindingFlags flags = 0)
	{
		return String.Join(SEPARATOR, new[] { obj.ToStringFields(flags), obj.ToStringProperties(flags) });
	}

	public static string ToStringEnumerable<T>(this IEnumerable<T> source, string separator = SEPARATOR)
	{
		return source + ":\n" + String.Join(separator, source
				   .Select((e, i) => "\t" + String.Format(FORMAT, "[" + i + "]", e.ToString()) + "\n")
				   .ToArray());
	}

	public static string ToStringKVPEnumerable<T, U>(this IEnumerable<KeyValuePair<T, U>> source, string separator = SEPARATOR)
	{
		return source + ":\n" + String.Join(separator, source
				   .Select((e) => "\t" + String.Format(FORMAT, "[" + e.Key + "]", e.Value.ToString()) + "\n")
				   .ToArray());
	}

	public static T Random<T>(this T[] ar)
	{
		if (ar.Length == 0)
			return default(T);
		return ar[UnityEngine.Random.Range(0, ar.Length)];
	}

	#endregion

	#region Enumeration

	public static bool HasFlag<T>(this System.Enum type, T value)
	{
		try
		{
			return ((int)(object)type & (int)(object)value) == (int)(object)value;
		}
		catch
		{
			return false;
		}
	}

	public static bool Is<T>(this System.Enum type, T value)
	{
		try
		{
			return (int)(object)type == (int)(object)value;
		}
		catch
		{
			return false;
		}
	}

	public static T Add<T>(this System.Enum type, T value)
	{
		try
		{
			return (T)(object)(((int)(object)type | (int)(object)value));
		}
		catch (Exception ex)
		{
			throw new ArgumentException(
				string.Format(
					"Could not append value from enumerated type '{0}'.",
					typeof(T).Name
				), ex);
		}
	}

	public static T Remove<T>(this System.Enum type, T value)
	{
		try
		{
			return (T)(object)(((int)(object)type & ~(int)(object)value));
		}
		catch (Exception ex)
		{
			throw new ArgumentException(
				string.Format(
					"Could not remove value from enumerated type '{0}'.",
					typeof(T).Name
				), ex);
		}
	}


	#endregion

	#region IEnumerable

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
	{
		Assert.IsNotNull(enumeration);
		Assert.IsNotNull(action);
		foreach (T item in enumeration)
			action(item);
		return enumeration;
	}

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
	{
		Assert.IsNotNull(enumeration);
		Assert.IsNotNull(action);
		int i = 0;
		foreach (var item in enumeration)
			action(item, i++);
		return enumeration;
	}

	public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
	{
		return new HashSet<T>(source);
	}

	public static IEnumerable<T> Merge<T>(this IEnumerable<T> source, params T[] args)
	{
		foreach (T i in source) yield return i;
		foreach (T i in args) yield return i;
	}

	public static T Random<T>(this IEnumerable<T> enumerable)
	{
		return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count()));
	}

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
	{
		return enumerable.OrderBy(e => UnityEngine.Random.value);
	}

	public static IEnumerable<T> EnumerableCreate<T>(int count) where T : new()
	{
		return EnumerableCreate(count, () => new T());
	}

	public static IEnumerable<T> EnumerableCreate<T>(int count, Func<T> creator)
	{
		for (int i = 0; i < count; i++)
		{
			yield return creator();
		}
	}

	#endregion

	#region Collection

	public static void Resize<T>(this List<T> list, int sz, T c)
	{
		int cur = list.Count;
		if (sz < cur)
			list.RemoveRange(sz, cur - sz);
		else if (sz > cur)
		{
			if (sz > list.Capacity) //this bit is purely an optimisation, to avoid multiple automatic capacity changes.
				list.Capacity = sz;
			list.AddRange(Enumerable.Repeat(c, sz - cur));
		}
	}

	public static void Resize<T>(this List<T> list, int sz) where T : new()
	{
		Resize(list, sz, new T());
	}

	#endregion

	#region Func Action

	public static void InvokeEx(this Action action)
	{
		if (action != null) action();
	}

	public static void InvokeEx<T>(this Action<T> action, T arg1)
	{
		if (action != null) action(arg1);
	}

	public static void InvokeEx<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
	{
		if (action != null) action(arg1, arg2);
	}

	public static TResult InvokeEx<TResult>(this Func<TResult> func, TResult result = default(TResult))
	{
		return func != null ? func() : result;
	}

	public static TResult InvokeEx<T, TResult>(this Func<T, TResult> func, T arg, TResult result = default(TResult))
	{
		return func != null ? func(arg) : result;
	}

	public static TResult InvokeEx<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1, T2 arg2, TResult result = default(TResult))
	{
		return func != null ? func(arg1, arg2) : result;
	}

	public static TResult InvokeEx<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, TResult result = default(TResult))
	{
		return func != null ? func(arg1, arg2, arg3) : result;
	}

	#endregion

	#region UnityEvent

	public static void InvokeEx(this UnityEngine.Events.UnityEvent a)
	{
		if (a != null)
			a.Invoke();
	}

	#endregion

	#region LayerMask

	public static bool Hit(this LayerMask rhs, int layerIndex)
	{
		return rhs.value.CheckBitAny(1 << layerIndex);
	}

	#endregion

	#region Transform

	/// <summary>距離を取得</summary>
	public static float SqrDistance(this Transform transform, Transform target)
	{
		return (transform.position - target.position).sqrMagnitude;
	}

	/// <summary>距離を取得(2d)</summary>
	public static float SqrDistance2D(this Transform transform, Transform target)
	{
		return (transform.position.ToVector2() - target.position.ToVector2()).sqrMagnitude;
	}

	/// <summary>2d version's Transform#Lookat </summary>
	public static void LookAt2D(this Transform transform, Vector3 target)
	{
		var dir = target - transform.position;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public static void SmoothLookAt(this Transform transform, Vector3 newDirection)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), Time.deltaTime);
	}

	/// <summary>近くにあるオブジェクトを探す</summary>
	/// <param name='current'>自オブジェクト</param>
	/// <param name='targets'>検索対象オブジェクト</param>
	public static Transform FindClosestObject(this Transform current, Transform[] targets)
	{
		// 返却対象
		Transform best_target = null;
		// 距離
		float closest_distance = Mathf.Infinity;
		// 自オブジェクト位置
		Vector3 current_position = current.position;
		// ターゲット走査
		foreach (Transform t in targets)
		{
			Vector3 direction2target = t.position - current_position;
			float d_sqr2target = direction2target.sqrMagnitude;
			if (d_sqr2target < closest_distance)
			{
				closest_distance = d_sqr2target;
				best_target = t;
			}
		}
		return best_target;
	}

	/// <summary>対象のオブジェクトがどの方向にいるか</summary>
	// TODO: 各値が１なので単純にどっちかしかわからない
	public static Vector3 ObjectDirection(this Transform current, Transform target)
	{
		var p = current.position - target.position;
		p.x = 0 < p.x ? -1f : 1f;
		p.y = 0 < p.y ? -1f : 1f;
		p.z = 0 < p.z ? -1f : 1f;
		return p;
	}

	/// <summary>対象のオブジェクトがどの方向にいるか</summary>
	// TODO: 各値が１なので単純にどっちかしかわからない
	public static Vector3 ObjectDirection(this Transform current, Vector3 target)
	{
		var p = current.position - target;
		p.x = 0 < p.x ? -1f : 1f;
		p.y = 0 < p.y ? -1f : 1f;
		p.z = 0 < p.z ? -1f : 1f;
		return p;
	}

	/// <summary>子オブジェクトを順序通りに取得する</summary>
	public static Transform[] GetChildren(this Transform parent)
	{
		var list = new List<Transform>();
		foreach (Transform t in parent)
		{
			list.Add(t);
		}
		return list.ToArray();
	}

	public static Transform[] GetAllChildren(this Transform root, bool includeRoot = false)
	{
		var list = new List<Transform>();
		if (includeRoot)
			list.Add(root);
		foreach (Transform t in root)
		{
			list.AddRange(t.GetAllChildren(true));
		}
		return list.ToArray();
	}

	public static void SetScaleIgnoreParent(this Transform transform, Vector3 scale)
	{
		var lossScale = transform.lossyScale;
		var localScale = transform.localScale;
		transform.localScale = new Vector3(
			localScale.x / lossScale.x * scale.x,
			localScale.y / lossScale.y * scale.y,
			localScale.z / lossScale.z * scale.z
		);
	}

	public static bool Overlaps(this RectTransform a, RectTransform b)
	{
		return a.WorldRect().Overlaps(b.WorldRect());
	}

	public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
	{
		return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
	}

	public static Rect WorldRect(this RectTransform rectTransform)
	{
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
		float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

		Vector3 position = rectTransform.position;
		return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
	}

	public static Vector3 To(this Transform lhs, Transform rhs)
	{
		return (rhs.position - lhs.position).normalized;
	}

	public static Vector3 To(this Transform lhs, Vector3 pos)
	{
		return (pos - lhs.position).normalized;
	}

	public static Vector2 SetWorldPositionToRectTransformAnchoredPosition(Vector3 position, Canvas canvas, Camera worldCamera, Camera uiCamera)
	{
		var pos = Vector2.zero;
		var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, position);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, uiCamera, out pos);
		return pos;
	}

	#endregion

	#region GameObject

	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void UpdateName(this GameObject lhs, string name)
	{
		lhs.name = name;
	}

	/// <summary>コンポーネントのコピー</summary>
	public static T CopyComponent<T>(this GameObject original, GameObject destination) where T : Component
	{
		var original_component = original.GetComponent<T>();
		var type = original_component.GetType();
		var copy = destination.AddComponent(type);
		/*System.Reflection.FieldInfo[]*/
		var fields = type.GetFields();
		foreach (var field in fields)
		{
			field.SetValue(copy, field.GetValue(original_component));
		}
		return (T)copy;
	}

	/// <summary>GetComponent method omit the null checking</summary>
	public static GameObject GetComponentThen<T>(this GameObject lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponent<T>();
		if (t == null)
			return lhs;
		action.Invoke(t);
		return lhs;
	}

	public static GameObject GetComponentsThen<T>(this GameObject lhs, Action<T> action) where T : Component
	{
		lhs.GetComponents<T>().ForEach(action.Invoke);
		return lhs;
	}

	public static GameObject GetComponentInChildrenThen<T>(this GameObject lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponentInChildren<T>();
		if (t == null)
			return lhs;
		action.Invoke(t);
		return lhs;
	}

	public static GameObject GetComponentsInChildrenThen<T>(this GameObject lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponentsInChildren<T>();
		t.ForEach(action.Invoke);
		return lhs;
	}

	/// <summary>GetComponent method omit the null checking</summary>
	public static T[] GetComponentsChildrenExceptSelf<T>(this GameObject lhs) where T : Component
	{
		var t = lhs.GetComponentsInChildren<T>();
		return t.Where(e => e != lhs).ToArray();
	}

	#endregion

	#region Component

	/// <summary>IFクラスの取得</summary>
	public static T GetInterface<T>(this Component lhs) where T : class
	{
		return lhs.GetComponent<T>();
	}

	/// <summary>IFクラスの取得</summary>
	public static Component GetInterfaceThen<T>(this Component lhs, Action<T> action) where T : class
	{
		var t = lhs.GetInterface<T>();
		if (t == null)
			return lhs;
		action.Invoke(t);
		return lhs;
	}

	/// <summary>GetComponent method omit the null checking</summary>
	public static Component GetComponentThen<T>(this Component lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponent<T>();
		if (t == null)
			return lhs;
		action.Invoke(t);
		return lhs;
	}

	public static Component GetComponentsThen<T>(this Component lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponents<T>();
		t.ForEach(action.Invoke);
		return lhs;
	}

	public static Component GetComponentInChildrenThen<T>(this Component lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponentInChildren<T>();
		if (t == null)
			return lhs;
		action.Invoke(t);
		return lhs;
	}

	public static Component GetComponentsInChildrenThen<T>(this Component lhs, Action<T> action) where T : Component
	{
		var t = lhs.GetComponentsInChildren<T>();
		t.ForEach(action.Invoke);
		return lhs;
	}

	/// <summary>GetComponent method omit the null checking</summary>
	public static T[] GetComponentsChildrenExceptSelf<T>(this Component lhs) where T : Component
	{
		var t = lhs.GetComponentsInChildren<T>();
		return t.Where(e => e != lhs).ToArray();
	}

	#endregion

	#region Animator

	public static bool CheckExistParameter(this Animator animator, string name)
	{
		// 名前検索
		return animator.parameters.Any(acp => acp.name == name);
	}

	#endregion

	#region ParticleSystem

	public static ParticleSystem SetPosition(this ParticleSystem ps, Vector3 world)
	{
		ps.transform.position = world;
		return ps;
	}

	public static ParticleSystem SetDuration(this ParticleSystem ps, float duration)
	{
		var m = ps.main;
		m.duration = duration;
		return ps;
	}

	public static ParticleSystem SetLoop(this ParticleSystem ps, bool b)
	{
		var m = ps.main;
		m.loop = b;
		return ps;
	}

	public static ParticleSystem SetPlayOnAwake(this ParticleSystem ps, bool b)
	{
		var m = ps.main;
		m.playOnAwake = b;
		return ps;
	}

	public static ParticleSystem SetSimulationSpace(this ParticleSystem ps, ParticleSystemSimulationSpace space)
	{
		var m = ps.main;
		m.simulationSpace = space;
		return ps;
	}

	public static ParticleSystem SetStartSize(this ParticleSystem ps, float size)
	{
		var m = ps.main;
		m.startSize = new ParticleSystem.MinMaxCurve(size);
		return ps;
	}

	public static ParticleSystem SetStartColor(this ParticleSystem ps, Color color)
	{
		var m = ps.main;
		m.startColor = new ParticleSystem.MinMaxGradient(color);
		return ps;
	}

	public static ParticleSystem SetStartColorRGB(this ParticleSystem ps, Color color)
	{
		var m = ps.main;
		color.a = m.startColor.color.a;
		m.startColor = new ParticleSystem.MinMaxGradient(color);
		return ps;
	}

	public static ParticleSystem SetStartRotation(this ParticleSystem ps, float r)
	{
		var m = ps.main;
		m.startRotation = new ParticleSystem.MinMaxCurve(r * Mathf.Deg2Rad);
		return ps;
	}

	public static ParticleSystem SetStartRotationX(this ParticleSystem ps, float x)
	{
		var m = ps.main;
		m.startRotationX = new ParticleSystem.MinMaxCurve(x * Mathf.Deg2Rad);
		return ps;
	}

	public static ParticleSystem SetStartRotationY(this ParticleSystem ps, float y)
	{
		var m = ps.main;
		m.startRotationY = new ParticleSystem.MinMaxCurve(y * Mathf.Deg2Rad);
		return ps;
	}

	public static ParticleSystem SetStartRotationZ(this ParticleSystem ps, float z)
	{
		var m = ps.main;
		m.startRotationZ = new ParticleSystem.MinMaxCurve(z * Mathf.Deg2Rad);
		return ps;
	}

	public static ParticleSystem SetStartRotationXYZ(this ParticleSystem ps, Vector3 v3)
	{
		var m = ps.main;
		m.startRotationX = new ParticleSystem.MinMaxCurve(v3.x * Mathf.Deg2Rad);
		m.startRotationY = new ParticleSystem.MinMaxCurve(v3.y * Mathf.Deg2Rad);
		m.startRotationZ = new ParticleSystem.MinMaxCurve(v3.z * Mathf.Deg2Rad);
		return ps;
	}

	#endregion

	#region Graphics

	/// <summary>アルファを設定する</summary>
	public static void SetAlpha(this SpriteRenderer renderer, float alpha)
	{
		Color color = renderer.color;
		color.a = alpha;
		renderer.color = color;
	}

	/// <summary>アルファを設定する</summary>
	public static void SetAlpha(this Renderer renderer, float alpha, bool shared = true)
	{
		if (shared)
		{
			var c = renderer.sharedMaterial.color;
			c.a = alpha;
			renderer.sharedMaterial.color = c;
		}
		else
		{
			var c = renderer.material.color;
			c.a = alpha;
			renderer.material.color = c;
		}
	}

	/// <summary>アルファを設定する</summary>
	/// <param name='text mesh'>TextMeshクラス</param>
	/// <param name='alpha'>アルファ値</param>
	public static void SetAlpha(this TextMesh textMesh, float alpha)
	{
		Color color = textMesh.color;
		color.a = alpha;
		textMesh.color = color;
	}

	#endregion

	#region Collider

	/// <summary>flip points</summary>
	public static void FlipPoints(this PolygonCollider2D collider, float minusOrPlus)
	{
		for (int i = 0; i < collider.pathCount; ++i)
		{
			var path = collider.GetPath(i);
			for (int j = 0; j < path.Length; ++j)
			{
				path[j].x *= minusOrPlus;
			}
			collider.SetPath(i, path);
		}
	}

	#endregion

	#region UI Element

	static Slider.SliderEvent sliderEmptyEvent = new Slider.SliderEvent();
	public static void SetValueWithoutCB(this Slider slider, float value)
	{
		var cb = slider.onValueChanged;
		slider.onValueChanged = sliderEmptyEvent;
		slider.value = value;
		slider.onValueChanged = cb;
	}

	static Toggle.ToggleEvent toggleEmptyEvent = new Toggle.ToggleEvent();

	public static void SetValueWithoutCB(this Toggle ui, bool value)
	{
		var cb = ui.onValueChanged;
		ui.onValueChanged = toggleEmptyEvent;
		ui.isOn = value;
		ui.onValueChanged = cb;
	}

	public static void SetSprite(this Image image, Sprite sprite)
	{
		image.sprite = sprite;
		image.SetAlpha(image.sprite == null ? 0 : 1);
	}

	public static void Enable(this CanvasGroup cg, bool enabled, Selectable selectable = null)
	{
		// interactableがオンのときのみEventSystemの更新が届く
		if (enabled)
		{
			cg.interactable = cg.blocksRaycasts = true;
			foreach (var sel in Selectable.allSelectablesArray)
				sel.OnPointerExit(null);
			if (selectable != null && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != selectable.gameObject)
				selectable.Select();
		}
		else
		{
			foreach (var sel in Selectable.allSelectablesArray)
				sel.OnPointerExit(null);
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			cg.interactable = cg.blocksRaycasts = false;
		}
	}

	public static void InteractableAndRaycastTarget(this Selectable s, bool enabled)
	{
		s.interactable = enabled;
		var gra = s.GetComponent<Graphic>();
		if (gra != null)
			gra.raycastTarget = enabled;
		if (s.image != null)
			s.image.raycastTarget = enabled;
	}

	public static void InteractableAndRaycastTarget(this CanvasGroup cg, bool enabled)
	{
		cg.interactable = cg.blocksRaycasts = enabled;
	}

	public static void On(this CanvasGroup cg)
	{
		cg.interactable = cg.blocksRaycasts = true;
	}
	public static void Off(this CanvasGroup cg)
	{
		cg.interactable = cg.blocksRaycasts = false;
	}
	public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
	{
		canvasGroup.alpha = alpha;
	}
	public static void SetAlpha(this Graphic graphic, float alpha)
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;
	}

	#endregion

	#region Color

	public static Color A(this Color color, float a)
	{
		color.a = a;
		return color;
	}

	#endregion

	#region MonoBehaviour

	public static void StopCoroutineEx(this MonoBehaviour m, Coroutine c)
	{
		if (c != null)
		{
			m.StopCoroutine(c);
			c = null;
		}
	}

	#endregion

	public static char[] cacheChars = new char[8];
	public static void SetIntegerNonAlloc(this TMPro.TMP_Text tmpro, int value)
	{
		var length = value.ToCharsNonAlloc(cacheChars);
		tmpro.SetCharArray(cacheChars, 0, length);
	}

}
