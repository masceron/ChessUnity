using Game.Piece.PieceLogic;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.DeathDefianceUI;

namespace Game.Effects.Traits
{
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class DeathDefiance: Effect
	{
		public DeathDefiance(PieceLogic piece) : base(-1, 1, piece, EffectName.DeathDefiance)
		{
		}

		public override void OnCallPieceAction(Action.Action action)
		{
            Debug.Log("DeathDefiancsdfsdfsde: " + Piece.Pos);
			// if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed || action.Maker == action.Target) {
			// 	return;
			// }
            
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
		}
	}
}
