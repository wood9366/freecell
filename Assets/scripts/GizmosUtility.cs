using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosUtility {
	static public void DrawRect(Vector2 lt, Vector2 size, Color color) {
		Gizmos.color = color;

		Vector2[] verts = new Vector2[] {
			lt,
			lt + new Vector2(size.x, 0),
			lt + size,
			lt + new Vector2(0, size.y)
		};

		Gizmos.DrawLine(verts[0], verts[1]);
		Gizmos.DrawLine(verts[1], verts[2]);
		Gizmos.DrawLine(verts[2], verts[3]);
		Gizmos.DrawLine(verts[3], verts[0]);
	}
}
