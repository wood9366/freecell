using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sprite2d : CustomMonoBehavior {
	public bool _AutoAdjustSize = true;

	void Update() {
		#if UNITY_EDITOR
		if (_AutoAdjustSize) {
			if (sprite.drawMode == SpriteDrawMode.Simple) {
				trans2d.sizeDelta = new Vector2(
					sprite.sprite.rect.width / sprite.sprite.pixelsPerUnit,
					sprite.sprite.rect.height / sprite.sprite.pixelsPerUnit);
			} else {
				sprite.size = trans2d.rect.size;
			}
		}
		#endif
	}

}
