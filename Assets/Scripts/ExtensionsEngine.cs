using UnityEngine;

public static class ExtensionsEngine
{
	private const float DIM = 1.0f;
	private const float AXIS_GAP = 0.7f;
	private const float DEFAULT_SIZE = .1f;

	public static void DebugDrawSector(this Vector3[] vectors, Vector3 center, Color color, bool isGradient, float duration = 0f)
	{
		if(vectors.Length == 0)
		{
			return;
		}
		Debug.DrawLine(center, vectors[0], color, duration);
		for(var index = 1; index < vectors.Length; index++)
		{
			var colorTemp = isGradient
				? Color.Lerp(color, color * .3f, (float)index / (vectors.Length - 1))
				: color;
			Debug.DrawLine(center, vectors[index], colorTemp);
			Debug.DrawLine(vectors[index - 1], vectors[index], colorTemp);
		}
	}

	public static void DebugDrawPoint(this Vector3 position, Color color, float size = DEFAULT_SIZE, float duration = 0f)
	{
		var oneOpposite = new Vector3(-1f, 1f);
		Debug.DrawLine(position + Vector3.one * size, position - Vector3.one * size, color, duration);
		Debug.DrawLine(position + oneOpposite * size, position - oneOpposite * size, color, duration);
	}

	public static void DebugDrawCross(this Vector3 position, Quaternion rotation, Color color, float size = DEFAULT_SIZE, float duration = 0f)
	{
		Debug.DrawLine(position + rotation * Vector3.up * size * AXIS_GAP, position + rotation * Vector3.up * size, Color.green * DIM, duration);
		Debug.DrawLine(position, position + rotation * Vector3.up * size * AXIS_GAP, color * DIM, duration);
		Debug.DrawLine(position, position - rotation * Vector3.up * size, color * DIM, duration);

		Debug.DrawLine(position + rotation * Vector3.right * size * AXIS_GAP, position + rotation * Vector3.right * size, Color.red * DIM, duration);
		Debug.DrawLine(position, position + rotation * Vector3.right * size * AXIS_GAP, color * DIM, duration);
		Debug.DrawLine(position, position - rotation * Vector3.right * size, color * DIM, duration);

		Debug.DrawLine(position + rotation * Vector3.forward * size * AXIS_GAP, position + rotation * Vector3.forward * size, Color.blue * DIM, duration);
		Debug.DrawLine(position, position + rotation * Vector3.forward * size * AXIS_GAP, color * DIM, duration);
		Debug.DrawLine(position, position - rotation * Vector3.forward * size, color * DIM, duration);
	}

	public static void DebugDrawArrow(this Vector3 start, Vector3 pointTo, Color color, Color colorArrow, float duration = 0)
	{
		var dir = pointTo - start;
		Debug.DrawLine(start, start + dir * .8f, color, duration);
		Debug.DrawLine(start + dir * .8f, start + dir, colorArrow, duration);
	}

	public static void DebugDraw(this Plane source, Color color, float size = DEFAULT_SIZE, float duration = 0f)
	{
		var origin = source.normal * source.distance;
		const float STEP = Mathf.PI / 12f;
		var unit = Quaternion.FromToRotation(Vector3.up, source.normal) * Vector3.left * size;
		for(var delta = 0f; delta < 2f * Mathf.PI; delta += Mathf.PI / 12f)
		{
			Debug.DrawLine(
				origin + Quaternion.AngleAxis(delta * Mathf.Rad2Deg, source.normal) * unit,
				origin + Quaternion.AngleAxis((delta + STEP) * Mathf.Rad2Deg, source.normal) * unit,
				color,
				duration);
		}
		Debug.DrawLine(origin, origin + source.normal * size * 1.2f, Color.cyan, duration);
		DebugDrawCross(origin, Quaternion.LookRotation(source.normal), color, size, duration);
	}

	public static void DebugDraw(this Rect source, Color color, float duration = 0f)
	{
		var right = source.size;
		right.Scale(Vector2.right);
		var up = source.size;
		up.Scale(Vector2.up);
		Debug.DrawLine(source.position, source.position + up, color, duration);
		Debug.DrawLine(source.position, source.position + right, color, duration);
		Debug.DrawLine(source.position + source.size, source.position + up, color, duration);
		Debug.DrawLine(source.position + source.size, source.position + right, color, duration);
	}
}
