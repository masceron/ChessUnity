using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunrayZoanthidDiurnal : Diurnal, IAfterPieceActionTrigger, IStartTurnTrigger
    {
		private readonly HashSet<PieceLogic> _preBlinded = new();
		private readonly HashSet<PieceLogic> _auraBlinded = new();
		private bool _wasActive;
		public SunrayZoanthidDiurnal(int radius, int probability, PieceLogic piece)
            : base(-1, 1, piece, "effect_sunray_zoanthid_diurnal")
        {
            SetStat(EffectStat.Radius, radius);
            SetStat(EffectStat.Strength, probability);
            _wasActive = IsActive;
            StartTurnEffectType = StartTurnEffectType.StartOfAnyTurn;
        }
        public AfterActionPriority Priority => AfterActionPriority.Debuff;
        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Debuff;
        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallAfterPieceAction(Action.Action action)
        {
			//Làm lại
        }

        public void OnCallStart(Action.Action lastMainAction)
		{
			if (_wasActive && !IsActive) RemoveAuraBlindInCurrentRadius();
			if (!_wasActive && IsActive) ApplyBlindInCurrentRadius();
			_wasActive = IsActive;
		}

		private void OnEnterRange(PieceLogic target)
		{
			if (target == null) return;
			if (HasBlinded(target))
			{
				_preBlinded.Add(target);
				return;
			}
			if (_auraBlinded.Contains(target)) return;

			var probability = GetStat(EffectStat.Strength);
			UnityEngine.Debug.Log("SunrayZoanthidDiurnal: OnEnterRange: ApplyBlind: ");
			ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, probability, target), Piece));
			_auraBlinded.Add(target);
		}

		private void OnLeaveRange(PieceLogic target)
		{
			if (target == null) return;
			if (!_auraBlinded.Contains(target)) return;

			foreach (var effect in target.Effects.Where(effect => effect is Blinded))
			{
				ActionManager.EnqueueAction(new RemoveEffect(effect));
				break;
			}
			_auraBlinded.Remove(target);
		}

		private void ApplyBlindInCurrentRadius()
		{
			var radius = GetStat(EffectStat.Radius);
			var probability = GetStat(EffectStat.Strength);

			for (var i = 0; i < BoardSize; i++)
			{
				if (!IsActive(i)) continue;
				var enemy = PieceOn(i);
				if (enemy == null || enemy.Color == Piece.Color) continue;
				if (Distance(enemy.Pos, Piece.Pos) > radius) continue;

				if (HasBlinded(enemy))
				{
					_preBlinded.Add(enemy);
					continue;
				}
				if (_auraBlinded.Contains(enemy) || _preBlinded.Contains(enemy)) continue;

				ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, probability, enemy), Piece));
				_auraBlinded.Add(enemy);
			}
		}

		private void RemoveAuraBlindInCurrentRadius()
		{
			var radius = GetStat(EffectStat.Radius);
			for (var i = 0; i < BoardSize; i++)
			{
				if (!IsActive(i)) continue;
				var enemy = PieceOn(i);
				if (enemy == null || enemy.Color == Piece.Color) continue;
				if (Distance(enemy.Pos, Piece.Pos) > radius) continue;
				if (!_auraBlinded.Contains(enemy)) continue;

				foreach (var effect in enemy.Effects.Where(effect => effect is Blinded))
				{
					ActionManager.EnqueueAction(new RemoveEffect(effect));
					break;
				}
				_auraBlinded.Remove(enemy);
			}
		}

		private static bool HasBlinded(PieceLogic target)
		{
			return target != null && target.Effects.Any(e => e is Blinded);
		}

    }
}
