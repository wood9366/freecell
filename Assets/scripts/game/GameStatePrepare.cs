using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePrepare : State {
    public GameStatePrepare(Game game) {
        _game = game;
    }

    public override void enter() {
        showRoundStartAnimation();
    }

    void showRoundStartAnimation() {
        // todo, edit round start animation

        onRoundStartAnimationEnd();
    }

    void onRoundStartAnimationEnd() {
        _game.changeStatus(Game.EStatus.DEAL);
    }

    Game _game;
}
