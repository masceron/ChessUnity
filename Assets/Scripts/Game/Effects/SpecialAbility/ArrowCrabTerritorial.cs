using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.SpecialAbility
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ArrowCrabTerritorial : Territorial
	{
		private Piercing _piercing;

		public ArrowCrabTerritorial(PieceLogic piece) : base(4, piece, "effect_arrow_crab_territorial")
		{
		}

		protected override void OnTerritorialActivated()
		{
			if (_piercing != null) return;

			_piercing = new Piercing(-1, Piece);
			ActionManager.EnqueueAction(new ApplyEffect(_piercing));
		}

		protected override void OnTerritorialDeactivated()
		{
			if (_piercing == null) return;

			ActionManager.EnqueueAction(new RemoveEffect(_piercing));
			_piercing = null;
		}
	}
}
