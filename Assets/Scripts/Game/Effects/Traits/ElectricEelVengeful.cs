using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class  ElectricEelVengeful: Effect, IAfterPieceActionEffect
    {
        public ElectricEelVengeful(PieceLogic piece) : base(-1, 1, piece, "effect_electric_eel_vengeful")
        {}

        public void OnCallAfterPieceAction(Action.Action action)    
        {
            if (action.Target == Piece.Pos && action is ICaptures && action.Result == ResultFlag.Success)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, BoardUtils.PieceOn(action.Maker))));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 70;
        }
    }
}