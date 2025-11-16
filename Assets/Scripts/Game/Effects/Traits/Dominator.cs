using System.Collections.Generic;
using Game.Action.Captures;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, "effect_dominator")
        {}
        
        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            actions.RemoveAll(action =>
                action is ICaptures && PieceOn(action.Maker).PieceRank <= PieceOn(action.Target).PieceRank);
        }
    }
}