using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMonoBehavior : MonoBehaviour {
	public SpriteRenderer sprite {
		get {
			if (_sprite == null) {
				_sprite = GetComponent<SpriteRenderer>();
			}

			return _sprite;
		}
	}

	SpriteRenderer _sprite = null;

    public Collider2D collider2d {
        get {
            if (_collider2d == null) {
                _collider2d = GetComponent<Collider2D>();
            }

            return _collider2d;
        }
    }

    Collider2D _collider2d;
}
