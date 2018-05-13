using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectUtility {

	class WindowCreateInput : EditorWindow {
		static public void Show(GameObject template) {
			var win = ScriptableObject.CreateInstance<WindowCreateInput>();

			win._templateGameObject = template;

			win.wantsMouseMove = true;
			win.position = new Rect(100, 100, 250, 150);
			win.ShowPopup();
		}

		GameObject _templateGameObject;

		void OnGUI() {
			GUILayout.BeginVertical();

			_nameCreatePrefix = EditorGUILayout.TextField("Name Prefix: ",
                                                          _nameCreatePrefix);

			_numCreate = EditorGUILayout.IntField("Number of Create: ", _numCreate);

			if (GUILayout.Button("Create")) {
				create();
				this.Close();
			}

			GUILayout.EndVertical();
		}

		void create() {
            if (_templateGameObject != null) {
                for (int i = 0; i < _numCreate; i++) {
                    var go = GameObject.Instantiate(_templateGameObject) as GameObject;

                    go.name = _nameCreatePrefix + "_" + i.ToString();

					UnityEditor.GameObjectUtility.SetParentAndAlign(
						go, _templateGameObject.transform.parent.gameObject);

                    Undo.RegisterCreatedObjectUndo(go, "create " + go.name);
				}
            }
        }

		int _numCreate = 1;
		string _nameCreatePrefix = "";
	}

	[MenuItem("GameObject/Custom/Create", false, 10)]
	static void ContextMenuItemGameObjectCreate (MenuCommand command) {
		WindowCreateInput.Show(command.context as GameObject);
	}
}
