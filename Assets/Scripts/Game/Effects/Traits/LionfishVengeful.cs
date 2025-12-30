using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LionfishVengeful: Effect
    {
        public LionfishVengeful(PieceLogic piece) : base(-1, 1, piece, "effect_lionfish_vengeful")
        {}
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;
            
            if (action.Target == Piece.Pos && action.Result == ResultFlag.Success)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(3, BoardUtils.PieceOn(action.Maker))));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}