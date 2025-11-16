using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlimeheadPassive : Effect
    {
        public SlimeheadPassive(PieceLogic piece) : base(-1, 1, piece, "slimehead_passive")
        {
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;

            if (action.Target != Piece.Pos || !Piece.IsDead()) return; //TODO: Fix bug: IsDead() is checked before this piece is killed so this action doesn't happen.
            var maker = BoardUtils.PieceOn(action.Maker);
            var buffEffect = maker.Effects.Count(t => t.Category == EffectCategory.Buff);

            if (buffEffect < 2) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Infected(maker)));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, maker)));
        }
    }
}