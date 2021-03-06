﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardMgr : Singleton<MoveCardMgr> {
    public System.Action OnCommandCursorChange;

    public bool CanUndo {
        get { return  PrevCmdIdx >= 0 && PrevCmdIdx < _cmds.Count; }
    }

    public void reset() {
        _cmds.Clear();
        PrevCmdIdx = -1;
    }

    public void move(MoveCardCommand cmd) {
        cmd.run();

        // remove commands after prev cmd idx
        // becase new cmd comming, old cmd history branch should remove
        int n = _cmds.Count - (PrevCmdIdx + 1);

        if (n > 0) {
            _cmds.RemoveRange(PrevCmdIdx + 1, n);
        }

        _cmds.Add(cmd);
        PrevCmdIdx = _cmds.Count - 1;
    }

    public void undo() {
        if (PrevCmdIdx >= 0 && PrevCmdIdx < _cmds.Count) {
            _cmds[PrevCmdIdx].undo();
            PrevCmdIdx--;
        }
    }

    public void redo() {
        if (PrevCmdIdx < _cmds.Count - 1) {
            PrevCmdIdx++;
            if (OnCommandCursorChange != null) OnCommandCursorChange();
            _cmds[PrevCmdIdx].run();
        }
    }

    int PrevCmdIdx {
        get { return _prevCmdIdx; }
        set {
            _prevCmdIdx = value;
            if (OnCommandCursorChange != null) OnCommandCursorChange();
        }
    }

    int _prevCmdIdx = -1;
    List<MoveCardCommand> _cmds = new List<MoveCardCommand>();
}
