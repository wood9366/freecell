using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDeckCard : MonoBehaviour {

	static public DragDeckCard Instance { get { return sInstance; } }
	static DragDeckCard sInstance = null;

	void Awake() {
		sInstance = this;
	}

	void onDragCard(CardObject card) {
		if (_isDragging) {
            // Debug.Log("dargging " + card.Data.ToString());
            updatePositionAtMouse();
		} else {
            // Debug.Log("start drag " + card.Data.ToString());

            CardObject dragCard = null;

			if (card.deckCard != null && card.deckCard.isDragable(card)) {
				card.deckCard.dragCardOut(card);
				dragCard = card;
			} else if (card.deckSwitch != null) {
				dragCard = card;
			}

            if (dragCard != null) {
				drag(dragCard);
				updatePositionAtMouse();
            }
        }
	}

	void updatePositionAtMouse() {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = -1;

		transform.position = pos;
	}

	void onDragCardEnd(CardObject cardObj) {
		// Debug.Log("end drag " + cardObj.Data.ToString());

		if (_isDragging && cardObj == _draggingCard) {
			if (_draggingCard.deckCard != null) {
				_draggingCard.deckCard.dragCardIn(_draggingCard);
			}

			_isDragging = false;
		}
	}

	void drag(CardObject card) {
		_isDragging = true;
		_draggingCard = card;

		card.transform.SetParent(transform);
		card.transform.localPosition = new Vector3(0, 0, -0.1f);
		card.transform.localRotation = Quaternion.identity;
		card.transform.localScale = Vector3.one;
	}

	bool _isDragging = false;
	CardObject _draggingCard = null;
}
