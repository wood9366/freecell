using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtility {
    static public void Assert(bool isTrue, string message = "") {
        if (!isTrue) Debug.LogError(message);
    }
}
