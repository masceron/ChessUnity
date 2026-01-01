using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Camouflage: Effect, IOnMoveGenEffect
    {
        public Camouflage(PieceLogic piece, sbyte duration = -1) : base(duration, 1, piece, "effect_camouflage")
        {}

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller.Color == Piece.Color) return;
            
            actions.RemoveAll(a => BoardUtils.Distance(a.Maker, Piece.Pos) >= 3 && a.Target == Piece.Pos);
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }
    }
}