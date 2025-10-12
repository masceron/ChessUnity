using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

public abstract class RegionalEffect
{
    protected RegionalEffect(){
        MatchManager.Ins.GameState.OnIncreaseTurn += ApplyEffect;
    }
    protected abstract void ApplyEffect(int currentTurn);

}
