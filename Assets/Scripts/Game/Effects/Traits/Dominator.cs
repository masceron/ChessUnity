using System.Collections.Generic;
using Game.Action.Captures;
using Game.Effects.Triggers;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Dominator: Effect, IOnMoveGenTrigger
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, "effect_dominator")
        {}
        
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            actions.RemoveAll(action =>
                action is ICaptures && PieceOn(action.Maker).PieceRank <= PieceOn(action.Target).PieceRank);
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 50;
        }
    }
}