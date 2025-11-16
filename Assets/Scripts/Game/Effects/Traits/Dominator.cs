using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, EffectName.Dominator)
        {}
        
        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            actions.RemoveAll(action =>
                action is ICaptures && PieceOn(action.Maker).PieceRank <= PieceOn(action.Target).PieceRank);
        }
    }
}