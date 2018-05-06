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
	public bool _IsDisplayPreferRect = false;
	public bool _IsDisplayPixelRect = false;
	#endif

	[ContextMenu("Calculate Pixel Perfect")]
	public void calculatePixelPerfect() {
		if (_Cam != null && _Cam.orthographic && _Height > 0) {
			// Debug.LogFormat("Screen: {0} x {1}, Aspect: {2}", 
			// 	Screen.width, Screen.height, (float)Screen.width / Screen.height);
			// Debug.LogFormat("Camera: size: {0}, size: {1} x {2} | (scaled) {3} x {4}, Aspect: {5}",
			// 	Camera.main.orthographicSize,
			// 	Camera.main.pixelWidth, Camera.main.pixelHeight,
			// 	Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight,
			// 	Camera.main.aspect);
			
			var scale = _Cam.orthographicSize * 2 / _Height;

			transform.localScale = new Vector3(scale, scale, 1);

			if (trans2d != null) {
				trans2d.sizeDelta = new Vector3(_Height * _Cam.aspect, _Height);
			}
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

		if (_IsDisplayPreferRect) {
			if (trans2d != null) {
				GizmosUtility.DrawRect(trans2d.rect.min, trans2d.rect.size, Color.red);
			}
		}
    }
#endif
}
