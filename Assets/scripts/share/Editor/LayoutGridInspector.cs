using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayoutGrid))]
public class LayoutGridInspector : Editor {

	void OnEnable() {
		mLayoutGrid = target as LayoutGrid;

		mNumCol = serializedObject.FindProperty("NumCol");
		mSpaceX = serializedObject.FindProperty("SpaceX");
		mSpaceY = serializedObject.FindProperty("SpaceY");
		mAlignment = serializedObject.FindProperty("Alignment");

		mNumCreate = serializedObject.FindProperty("NumCreate");
		mCreateNamePrefix = serializedObject.FindProperty("CreateNamePrefix");
		mCreateTemplate = serializedObject.FindProperty("CreateTemplate");
	}

	LayoutGrid mLayoutGrid;
	SerializedProperty mNumCol;
	SerializedProperty mSpaceX;
	SerializedProperty mSpaceY;
	SerializedProperty mAlignment;
	SerializedProperty mNumCreate;
    SerializedProperty mCreateNamePrefix;
	SerializedProperty mCreateTemplate;

	public override void OnInspectorGUI()
    {
		EditorGUILayout.BeginVertical();

		uiCreate();
		uiLayout();

		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
    }

	void uiCreate() {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.PropertyField(mCreateTemplate, new GUIContent("Create Template"));

        if (mCreateTemplate != null) {
            EditorGUILayout.PropertyField(mNumCreate, new GUIContent("Number"));
            EditorGUILayout.PropertyField(mCreateNamePrefix, new GUIContent("Name Prefix"));

            if (GUILayout.Button("Create")) {
				createChildrenObjs();
				mLayoutGrid.reLayout();
            }

            if (GUILayout.Button("Delete")) {
                List<GameObject> deletedObjs = new List<GameObject>();

                for (int i = 0; i < mLayoutGrid.transform.childCount; i++) {
                    var obj = mLayoutGrid.transform.GetChild(i).gameObject;

                    if (obj.GetInstanceID() != mCreateTemplate.objectReferenceInstanceIDValue) {
                        deletedObjs.Add(obj);
                    }
                }

                foreach (var obj in deletedObjs) {
                    GameObject.DestroyImmediate(obj);
                }
            }
        }

        EditorGUILayout.EndVertical();
    }

	void createChildrenObjs() {
        for (int i = 0; i < mNumCreate.intValue; i++) {
            var o = GameObject.Instantiate(
                mCreateTemplate.objectReferenceValue,
                Vector3.one, Quaternion.identity, mLayoutGrid.transform) as GameObject;

            o.name = string.Format("{0}_{1}", mCreateNamePrefix.stringValue, i);
            o.SetActive(true);
        }
    }

	void uiLayout() {
		EditorGUILayout.BeginVertical("box");

		bool changed = false;

		changed = uiIntField(mNumCol, 1, "Number of Col");
		changed = uiFloatField(mSpaceX, 0, "Space X") || changed;
		changed = uiFloatField(mSpaceY, 0, "Space Y") || changed;
		changed = uiEnumField(mAlignment, "Alignment") || changed;

		if (changed) {
			mLayoutGrid.reLayout();
		}

		if (GUILayout.Button("Layout")) {
			mLayoutGrid.reLayout();
		}

		EditorGUILayout.EndVertical();
	}

	bool uiFloatField(SerializedProperty property, float min, string name) {
		var val = property.floatValue;

		EditorGUILayout.PropertyField(property, new GUIContent(name));
		property.floatValue = Mathf.Max(property.floatValue, min);

		return val != property.floatValue;
	}

	bool uiIntField(SerializedProperty property, int min, string name) {
		var val = property.intValue;

		EditorGUILayout.PropertyField(property, new GUIContent(name));
		property.intValue = Mathf.Max(property.intValue, min);

		return val != property.intValue;
	}

	bool uiEnumField(SerializedProperty property, string name) {
		var val = property.enumValueIndex;

		EditorGUILayout.PropertyField(property, new GUIContent(name));

		return val != property.enumValueIndex;
	}
}
