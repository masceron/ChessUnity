using Game.Action;
using Game.Action.Internal;
using Game.Action.Captures;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumboldtSquidPassive : Effect
    {
        private int count;
        public HumboldtSquidPassive(PieceLogic piece) : base(-1, 1, piece, "effect_humboldt_squid_passive")
        {
            count = 0;

        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.Maker == Piece.Pos && action is ICaptures && action.Result == ActionResult.Failed)
            {
                var target = PieceOn(action.Target);
                if (target != null && target.Color != Piece.Color)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, target)));
                }
                
            }
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                {
                    count++;
                }
            }
            if (count >= 4)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Frienzied(Piece)));
                count = 0;
            }
        }
    }
}
