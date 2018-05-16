using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenPixelPerfect : CustomMonoBehavior {
	public Camera _Cam;
	public int _Height;

	#if UNITY_EDITOR
	public bool _IsDisplayPixelRect = false;
	#endif

	[ContextMenu("Calculate Pixel Perfect")]
	public void calculatePixelPerfect() {
		if (_Cam != null && _Cam.orthographic && _Height > 0) {
			var scale = _Cam.orthographicSize * 2 / _Height;

			transform.localScale = new Vector3(scale, scale, 1);
		}
	}

	public int width { get { return _Cam != null ? Mathf.RoundToInt(_Cam.aspect * _Height) : 0; } }
	public int height { get { return _Height; } }

#if UNITY_EDITOR
    void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;

		if (_IsDisplayPixelRect) {
			var size = new Vector2(width, height);
			GizmosUtility.DrawRect(Vector2.zero - size / 2, size, Color.blue);
		}
    }
#endif
}
