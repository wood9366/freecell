using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenPixelPerfect))]
public class ScreenPixelPerfectInspector : Editor {
	void OnEnable() {
		_owner = (ScreenPixelPerfect)target;
		_cam = serializedObject.FindProperty("_Cam");
		_height = serializedObject.FindProperty("_Height");
		_isDisplayPreferRect = serializedObject.FindProperty("_IsDisplayPreferRect");
		_isDisplayPixelRect = serializedObject.FindProperty("_IsDisplayPixelRect");
	}

	ScreenPixelPerfect _owner = null;
    SerializedProperty _cam = null;
	SerializedProperty _height = null;
	SerializedProperty _isDisplayPixelRect = null;
	SerializedProperty _isDisplayPreferRect = null;

	public override void OnInspectorGUI() {
		GUILayout.BeginVertical("box");

		uiInfo();

		EditorGUILayout.PropertyField(_cam, new GUIContent("Camera"));

		if (uiSerializedFieldInt(_height, "Height")) {
			_owner.calculatePixelPerfect();
		}

		GUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();

		// DrawDefaultInspector();
	}

	void uiInfo() {
		GUILayout.BeginHorizontal();

		GUILayout.Space(5);
		if (GUILayout.Button("c", GUILayout.Width(20), GUILayout.Height(20))) {
			_owner.calculatePixelPerfect();
		}

		GUILayout.Label(string.Format("Prefer Size <color=#ff0000>{0} x {1}</color>",
			_owner.width, _owner.height), labelStyle);
		_isDisplayPreferRect.boolValue = EditorGUILayout.Toggle(_isDisplayPreferRect.boolValue);

		GUILayout.Label(string.Format("Pixel Size <color=#0000ff>{0} x {1}</color>",
			camera.pixelWidth, camera.pixelHeight), labelStyle);
		_isDisplayPixelRect.boolValue = EditorGUILayout.Toggle(_isDisplayPixelRect.boolValue);

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();
	}

	GUIStyle labelStyle {
		get {
			if (_labelStyle == null) {
				_labelStyle = new GUIStyle(GUI.skin.label);
				_labelStyle.alignment = TextAnchor.MiddleLeft;
				_labelStyle.richText = true;
				_labelStyle.fixedHeight = 25;
			}

			return _labelStyle;
		}
	}

	GUIStyle _labelStyle = null;

	Camera camera { get { return _owner._Cam; } }

	bool uiSerializedFieldInt(SerializedProperty prop, string name) {
		int val = prop.intValue;

		EditorGUILayout.PropertyField(prop, new GUIContent(name));

		return prop.intValue != val;
	}
}
