using System.Collections.Generic;
using Game.Board.Piece.PieceLogic;
using Game.Common;

namespace Game.Board.Effects.Buffs
{
    public class Camouflage: Effect
    {
        public Camouflage(PieceLogic piece, sbyte duration = -1) : base(duration, 1, piece, EffectName.Camouflage)
        {}

        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            if (BoardUtils.SideToMove() == Piece.Color) return;
            
            actions.RemoveAll(a => BoardUtils.Distance(a.Maker, Piece.Pos) >= 3 && a.Target == Piece.Pos);
        }
    }
}