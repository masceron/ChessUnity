using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class GluttonousJawPassive : Effect, IAfterPieceActionEffect
    {
        

        public GluttonousJawPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_gluttonous_jaw_passive")
        { }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action == null || action is not ICaptures) return;
            if (action.Maker != Piece.Pos) return;
            
            var maker = PieceOn(action.Maker);
            if (maker.Effects.Any(e => e.EffectName == "effect_consume"))
            {
                if (action.Result == ResultFlag.Success)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new LongReach(maker, 2, 5)));
                }
            }
        }
    }
}