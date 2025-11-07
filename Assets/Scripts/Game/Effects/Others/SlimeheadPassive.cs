using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlimeheadPassive : Effect
    {
        public SlimeheadPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.SlimeheadPassive)
        {
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;
            
            if (action.Target == Piece.Pos && Piece.IsDead()) //TODO: Fix bug: IsDead() is checked before this piece is killed so this action doesn't happen.
            {
                var maker = BoardUtils.PieceOn(action.Maker);
                int buffEffect = 0;
                for (int i = 0; i < maker.Effects.Count; i++)
                {
                    if (maker.Effects[i].Category == EffectCategory.Buff)
                    {
                        buffEffect++;
                    }
                }

                if (buffEffect >= 2)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Infected(maker)));
                    ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, maker)));
                }
            }
        }
    }
}