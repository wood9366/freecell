using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMonoBehavior : MonoBehaviour {
	protected SpriteRenderer sprite {
		get {
			if (_sprite == null) {
				_sprite = GetComponent<SpriteRenderer>();
			}

			return _sprite;
		}
	}

	SpriteRenderer _sprite = null;

	protected RectTransform trans2d {
		get {
			if (_trans2d == null) {
				_trans2d = GetComponent<RectTransform>();
			}

			return _trans2d;
		}
	}

	RectTransform _trans2d = null;
}
