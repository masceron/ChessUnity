using Game.Piece.PieceLogic;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.DeathDefianceUI;
using System.Linq;

namespace Game.Effects.Traits
{
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class DeathDefiance: Effect
	{
		private int deathDefianceCount;
		public DeathDefiance(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, EffectName.DeathDefiance)
		{
		this.deathDefianceCount = deathDefianceCount;
		}

		public override void OnCallPieceAction(Action.Action action)
		{
			//còn né nữa chưa tính
			if(Piece.IsDead()) return;
			if (Piece.Effects.Any(e => e.EffectName == EffectName.Shield) 
				|| Piece.Effects.Any(e => e.EffectName == EffectName.Carapace) 
					|| Piece.Effects.Any(e => e.EffectName == EffectName.HardenedShield)) return;
			if (action == null || action.Target != Piece.Pos || action.Maker == action.Target) {
                return;
            }
			if (deathDefianceCount <= 1) return;
            var ui = Object.FindAnyObjectByType<DeathDefianceUI>(FindObjectsInactive.Include);
			if (!ui)
			{
				var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
				ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.DeathDefianceUI), canvas.transform).GetComponent<DeathDefianceUI>();
			}
			else
			{
				ui.gameObject.SetActive(true);
			}

			ui.Load(Piece.Pos);
			deathDefianceCount--;
		}
	}
}
