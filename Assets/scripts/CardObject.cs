using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : CustomMonoBehavior {
	public SpriteRenderer _SpriteType;
	public SpriteRenderer _SpriteVal;

	public Card Data { get { return _card; } }

	public void set(Card card) {
		var isValid = card.IsValid;

		IsEnabled = isValid;

		if (isValid) {
			_card.set(card);

			_SpriteType.sprite = ResourceMgr.Instance.getSpriteCardType(_card.Type);
			_SpriteVal.color = ResourceMgr.Instance.getColorCardType(_card.Type);
			_SpriteVal.sprite = ResourceMgr.Instance.getSpriteCardVal(_card.Val);
		}

		setName();
	}

	public bool IsEnabled {
		get { return gameObject.activeInHierarchy; }
		set { gameObject.SetActive(value); }
	}

	void setName() {
        name = Data.Type.ToString() + "_" + Data.Val.ToString();
	}

	void Awake() {
		setName();
		IsEnabled = false;
	}

	void OnMouseDown() {
		Debug.LogFormat("mouse down on obj {0} card {1}", name, Data.ToString());
	}

	void OnMouseDrag() {
		//Debug.LogFormat("drag {0}", _card.ToString());
		//sprite.color -= Color.white * Time.deltaTime;
	}

	Card _card = new Card();
}
