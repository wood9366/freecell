using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sprite2d : CustomMonoBehavior {
	public bool _AutoAdjustColliderSize = true;

	void Update() {
		#if UNITY_EDITOR
        if (!Application.isPlaying) {
            if (_AutoAdjustColliderSize && collider2d != null) {
                BoxCollider2D box = collider2d as BoxCollider2D;

                if (box != null) {
                    box.size = sprite.size;
                }
            }
        }
		#endif
	}

}
