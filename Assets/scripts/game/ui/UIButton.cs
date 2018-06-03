using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour {
    public Image _BG;
    public Text _Label;
    public Text _Delete;

    public Color _NormalColor = Color.white;
    public Color _DisableColor = Color.gray;

    public bool Enabled {
        get { return Btn.interactable; }
        set {
            Btn.interactable = value;

            _BG.color = value ? _NormalColor : _DisableColor;
            _Label.color = value ? _NormalColor : _DisableColor;
            _Delete.color = value ? _NormalColor : _DisableColor;
            _Delete.gameObject.SetActive(!value);
        }
    }

    public Button Btn {
        get {
            return GetComponent<Button>();
        }
    }
}
