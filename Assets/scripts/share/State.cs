using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {
    public virtual void enter() { }
    public virtual void update(float deltaTime) { }
    public virtual void exit() { }
}
