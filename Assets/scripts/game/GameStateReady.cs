using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateReady : State {
    public GameStateReady(Game game) {
        _game = game;
    }

    public override void enter() {
        _game.resetRound();
        _game.prepareRoundCards();

        _game._GameTopMenu.hideRoundTime();
        _game._GameBottomMenu._ButtonUndo.Enabled = MoveCardMgr.Instance.CanUndo;
        _game._GameMenu._ButtonNewRound.Enabled = false;
        _game._GameMenu._ButtonRestart.Enabled = false;

        _game.changeStatus(Game.EStatus.PREPARE);
    }

    Game _game;
}
