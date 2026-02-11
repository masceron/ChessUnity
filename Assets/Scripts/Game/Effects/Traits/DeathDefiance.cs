using UX.UI.Ingame;
using UX.UI.Ingame.DeathDefianceUI;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Captures;
namespace Game.Effects.Traits
{
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class DeathDefiance: Effect, IAfterPieceActionEffect
	{
		private int _deathDefianceCount;
		public DeathDefiance(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_death_defiance")
		{
		this._deathDefianceCount = deathDefianceCount;
		}

		public void OnCallAfterPieceAction(Action.Action action)
		{
			 if (action is not ICaptures)
			 {
				 return;
			 }
			 //còn né nữa chưa tính
			 if(!BoardUtils.IsAlive(Piece)) return;
			 if (Piece.Effects.Any(e => e.EffectName == "effect_shield") 
			     || Piece.Effects.Any(e => e.EffectName == "effect_carapace") 
			     || Piece.Effects.Any(e => e.EffectName == "effect_hardened_shield")) return;
			 if (action.Target != Piece.Pos || action.Maker == action.Target) {
				 return;
			 }
			 if (_deathDefianceCount <= 1) return;
			 var ui = BoardViewer.Ins.GetOrInstantiateUI<DeathDefianceUI>(IngameSubmenus.DeathDefianceUI);

			 ui.Load(Piece.Pos);
			 _deathDefianceCount--;
		}

		public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
	}
}
