using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Camouflage: Effect
    {
        public Camouflage(PieceLogic piece, sbyte duration = -1) : base(duration, 1, piece, "effect_camouflage")
        {}

        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            if (BoardUtils.SideToMove() == Piece.Color) return;
            
            actions.RemoveAll(a => BoardUtils.Distance(a.Maker, Piece.Pos) >= 3 && a.Target == Piece.Pos);
        }
    }
}