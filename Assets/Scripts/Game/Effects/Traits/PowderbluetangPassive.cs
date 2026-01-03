using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Tile;
using Game.Managers;
using Game.Action;
using Game.Action.Internal;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PowderbluetangPassive : Effect, IAfterPieceActionEffect
    {
        private int count;

        public PowderbluetangPassive(PieceLogic piece) : base(-1, 1, piece, "effect_powderbluetang_passive")
        {
            count = 2;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.Maker == Piece.Pos &&
                action.Result == ResultFlag.Success &&
                count > 0 &&
                FormationManager.Ins.GetFormation(action.Target) is HydroidThicket)
            {
                count--;
                ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(Piece, 1)));
            }
        }
    }
}