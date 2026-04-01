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
			UnityEngine.Debug.Log("SunrayZoanthidDiurnal: OnCallAfterPieceAction");
			if (!IsActive) return;

            if (action is not IQuiets) return;

            var movedPiece = action.GetMakerAsPiece();
            if (movedPiece == null) return;

            var radius = GetStat(EffectStat.Radius);

			if (movedPiece != Piece && movedPiece.Color != Piece.Color)
            {
				var prevPos = movedPiece.PreviousMoves.Count > 0
						? movedPiece.PreviousMoves[^1]
						: -1;

				var nowIn = Distance(movedPiece.Pos, Piece.Pos) <= radius;
				var wasIn = prevPos != -1 && Distance(prevPos, Piece.Pos) <= radius;

				if (nowIn && !wasIn) OnEnterRange(movedPiece);
				else if (!nowIn && wasIn) OnLeaveRange(movedPiece);

				return;
            }

            if (Piece.PreviousMoves.Count <= 0) return;

            var oldPos = Piece.PreviousMoves[^1];
            for (var i = 0; i < BoardSize; i++)
            {
                if (!IsActive(i)) continue;

				var enemy = PieceOn(i);
				if (enemy == null || enemy == Piece) continue;
				if (enemy.Color == Piece.Color) continue;

                var distToNewPos = Distance(i, Piece.Pos);
                var distToOldPos = Distance(i, oldPos);

				if (distToNewPos <= radius && distToOldPos > radius) OnEnterRange(enemy);
				else if (distToNewPos > radius && distToOldPos <= radius) OnLeaveRange(enemy);
            }
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
			return target != null && Enumerable.Any(Enumerable.OfType<Blinded>(target.Effects));
		}

    }
}
