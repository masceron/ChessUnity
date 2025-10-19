using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using UnityEngine;
using System.Linq;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Traits;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Relentless: Effect
    {

        private int deathDefianceCount;
        public Relentless(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, EffectName.Relentless)
        {
            this.deathDefianceCount = deathDefianceCount;
        }

        public override void OnCall(Action.Action action)
        {
            if (Piece.IsDead()) return;
			if (Piece.Effects.Any(e => e.EffectName == EffectName.Shield) 
				|| Piece.Effects.Any(e => e.EffectName == EffectName.Carapace) 
					|| Piece.Effects.Any(e => e.EffectName == EffectName.HardenedShield)) return;
            if (action == null || action.Target != Piece.Pos || action.Maker == action.Target) {
                return;
            }
            action.Result = ActionResult.Failed;
            ActionManager.EnqueueAction(new KillPiece(action.Maker));
            deathDefianceCount--;
            Debug.Log($"deathDefianceCount: {deathDefianceCount}");
            if (deathDefianceCount <= 0)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        
        }


    }
}