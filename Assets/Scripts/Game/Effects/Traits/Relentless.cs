using Game.Action;
using Game.Action.Internal;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Relentless: Effect
    {

        private int deathDefianceCount;
        public Relentless(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_relentless")
        {
            this.deathDefianceCount = deathDefianceCount;
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (Piece.IsDead()) return;
			if (Piece.Effects.Any(e => e.EffectName == "effect_shield") 
				|| Piece.Effects.Any(e => e.EffectName == "effect_carapace") 
					|| Piece.Effects.Any(e => e.EffectName == "effect_hardened_shield")) return;
            if (action == null || action.Target != Piece.Pos || action.Maker == action.Target) {
                return;
            }

            action.Result = ResultFlag.Blocked;
            var target = PieceOn(action.Maker);
            if (target?.Effects != null && target.Effects.All(e => e.EffectName != "effect_snapping_strike"))
            {
                ActionManager.EnqueueAction(new KillPiece(action.Maker));
            }
            deathDefianceCount--;
            if (deathDefianceCount <= 0)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }


    }
}