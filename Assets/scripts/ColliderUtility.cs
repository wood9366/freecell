using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUtility {
    static public bool IsColliderOverlap(Collider2D a, Collider2D b) {
        return GetRect(a).Overlaps(GetRect(b));
    }

    static Rect GetRect(Collider2D c) {
        return new Rect(c.bounds.min.x, c.bounds.min.y, c.bounds.size.x, c.bounds.size.y);
    }
}
