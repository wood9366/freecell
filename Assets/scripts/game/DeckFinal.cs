using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckFinal : Deck {
    public CardData.ECardType _CardType;
    public SpriteRenderer _Icon;

    public override int MaxNumCard { get { return 13; } }

    public override bool canGetOffCard(Card card) { return false; }

    #if UNITY_EDITOR
    public CardData.ECardType CardType {
        get { return _CardType; }
        set {
            _CardType = value;

            string[] namesCardType = new string[4] {
                "spade", "heart", "club", "diamond"
            };

            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/texture/poker_color_outline.png").OfType<Sprite>().ToArray();

            _Icon.sprite = null;

            foreach (var sprite in sprites) {
                if (namesCardType[(int)_CardType] == sprite.name) {
                    _Icon.sprite = sprite;
                    break;
                }
            }
        }
    }
    #endif

    protected override bool canPutOn(Card card) {
        if (card.CardType == _CardType) {
            if (TopCard == null) {
                return card.CardVal == 0;
            } else {
                return card.CardVal == TopCard.CardVal + 1;
            }
        }

        return false;
    }

    public override Vector3 CardStackOffset { get { return _cardStackOffset; } }

    Vector3 _cardStackOffset = new Vector3(0, 0, -0.1f);
}
