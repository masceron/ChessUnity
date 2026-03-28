using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Effects.SpecialAbility
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ArrowCrabNocturnal : Nocturnal, IStartTurnTrigger
	{
		private bool _wasActive = true;

		public ArrowCrabNocturnal(PieceLogic piece) : base(-1, 1, piece, "effect_arrow_crab_nocturnal")
		{

		}
		public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAnyTurn;
		public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

		public void OnCallStart(Action.Action lastMainAction)
		{
			if (IsActive == _wasActive) return;

			if (!IsActive)
			{
				var camouflage = Piece.Effects.OfType<Camouflage>().FirstOrDefault();
				if (camouflage != null) ActionManager.EnqueueAction(new RemoveEffect(camouflage));
			}
			else if (!Piece.Effects.OfType<Camouflage>().Any())
			{
				ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(Piece)));
			}

			_wasActive = IsActive;
		}
	}
}
