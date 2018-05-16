using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGrid : MonoBehaviour {
	public int NumCol = 0;

	public float SpaceX = 0;
	public float SpaceY = 0;

	public enum AlignmentType : int {
		TOP_LEFT = 0,
		TOP,
		TOP_RIGHT,
		LEFT,
		CENTER,
		RIGHT,
		BOTTOM_LEFT,
		BOTTOM,
		BOTTOM_RIGHT
	}

	public AlignmentType Alignment = AlignmentType.TOP_LEFT;

	#if UNITY_EDITOR

	public int NumCreate = 0;
	public GameObject CreateTemplate = null;
    public string CreateNamePrefix = "";

	#endif

	public void reLayout() {
        if (NumCol <= 0) return;
        
		Vector2 size = new Vector2((NumCol - 1) * SpaceX, (NumActiveChildren / NumCol - 1) * SpaceY);
		Vector3 offset = Vector2.zero;

		switch (Alignment) {
			case LayoutGrid.AlignmentType.TOP_LEFT: offset.Set(0, 0, 0); break;
			case LayoutGrid.AlignmentType.TOP: offset.Set(-size.x / 2.0f, 0, 0); break;
			case LayoutGrid.AlignmentType.TOP_RIGHT: offset.Set(-size.x, 0, 0); break;
			case LayoutGrid.AlignmentType.LEFT: offset.Set(0, size.y / 2.0f, 0); break;
			case LayoutGrid.AlignmentType.CENTER: offset.Set(-size.x / 2.0f, size.y / 2.0f, 0); break;
			case LayoutGrid.AlignmentType.RIGHT: offset.Set(-size.x, size.y / 2.0f, 0); break;
			case LayoutGrid.AlignmentType.BOTTOM_LEFT: offset.Set(0, size.y, 0); break;
			case LayoutGrid.AlignmentType.BOTTOM: offset.Set(-size.x / 2.0f, size.y, 0); break;
			case LayoutGrid.AlignmentType.BOTTOM_RIGHT: offset.Set(-size.x, size.y, 0); break;
		}

		for (int i = 0, idx = 0; i < transform.childCount; i++) {
			var sp = transform.GetChild(i);

            if (!sp.gameObject.activeSelf) continue;

			int r = idx / NumCol;
			int c = idx % NumCol;

			sp.transform.localPosition = offset + new Vector3(c * SpaceX, -r * SpaceY);

            idx++;
		}
	}

    int NumActiveChildren {
        get {
            int num = 0;

            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).gameObject.activeSelf) {
                    num++;
                }
            }

            return num;
        }
    }

	public int NumberOfCol { get { return NumCol; } }
}
