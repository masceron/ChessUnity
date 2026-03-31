using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunrayZoanthidDiurnal : Diurnal, IAfterPieceActionTrigger, IStartTurnTrigger
    {
		private readonly HashSet<PieceLogic> preBlinded = new();
		private readonly HashSet<PieceLogic> auraBlinded = new();
		private bool wasActive;
		public SunrayZoanthidDiurnal(int radius, int probability, PieceLogic piece)
            : base(-1, 1, piece, "effect_sunray_zoanthid_diurnal")
        {
            SetStat(EffectStat.Radius, radius);
            SetStat(EffectStat.Strength, probability);
            wasActive = IsActive;
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

            var movedPiece = PieceOn(action.Maker);
            if (movedPiece == null) return;

            var radius = GetStat(EffectStat.Radius);

			if (movedPiece != Piece && movedPiece.Color != Piece.Color)
            {
				int prevPos = movedPiece.PreviousMoves.Count > 0
						? movedPiece.PreviousMoves[movedPiece.PreviousMoves.Count - 1]
						: -1;

				var nowIn = Distance(movedPiece.Pos, Piece.Pos) <= radius;
				var wasIn = prevPos != -1 && Distance(prevPos, Piece.Pos) <= radius;

				if (nowIn && !wasIn) OnEnterRange(movedPiece);
				else if (!nowIn && wasIn) OnLeaveRange(movedPiece);

				return;
            }

            if (Piece.PreviousMoves.Count <= 0) return;

            int oldPos = Piece.PreviousMoves[Piece.PreviousMoves.Count - 1];
            for (int i = 0; i < BoardSize; i++)
            {
                if (!IsActive(i)) continue;

				var enemy = PieceOn(i);
				if (enemy == null || enemy == Piece) continue;
				if (enemy.Color == Piece.Color) continue;

                int distToNewPos = Distance(i, Piece.Pos);
                int distToOldPos = Distance(i, oldPos);

				if (distToNewPos <= radius && distToOldPos > radius) OnEnterRange(enemy);
				else if (distToNewPos > radius && distToOldPos <= radius) OnLeaveRange(enemy);
            }
        }

        public void OnCallStart(Action.Action lastMainAction)
		{
			if (wasActive && !IsActive) RemoveAuraBlindInCurrentRadius();
			if (!wasActive && IsActive) ApplyBlindInCurrentRadius();
			wasActive = IsActive;
		}

		private void OnEnterRange(PieceLogic target)
		{
			if (target == null) return;
			if (HasBlinded(target))
			{
				preBlinded.Add(target);
				return;
			}
			if (auraBlinded.Contains(target)) return;

			var probability = GetStat(EffectStat.Strength);
			UnityEngine.Debug.Log("SunrayZoanthidDiurnal: OnEnterRange: ApplyBlind: ");
			ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, probability, target), Piece));
			auraBlinded.Add(target);
		}

		private void OnLeaveRange(PieceLogic target)
		{
			if (target == null) return;
			if (!auraBlinded.Contains(target)) return;

			for (int i = 0; i < target.Effects.Count; i++)
			{
				var effect = target.Effects[i];
				if (effect is Blinded)
				{
					ActionManager.EnqueueAction(new RemoveEffect(effect));
					break;
				}
			}
			auraBlinded.Remove(target);
		}

		private void ApplyBlindInCurrentRadius()
		{
			var radius = GetStat(EffectStat.Radius);
			var probability = GetStat(EffectStat.Strength);

			for (int i = 0; i < BoardSize; i++)
			{
				if (!IsActive(i)) continue;
				var enemy = PieceOn(i);
				if (enemy == null || enemy.Color == Piece.Color) continue;
				if (Distance(enemy.Pos, Piece.Pos) > radius) continue;

				if (HasBlinded(enemy))
				{
					preBlinded.Add(enemy);
					continue;
				}
				if (auraBlinded.Contains(enemy) || preBlinded.Contains(enemy)) continue;

				ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, probability, enemy), Piece));
				auraBlinded.Add(enemy);
			}
		}

		private void RemoveAuraBlindInCurrentRadius()
		{
			var radius = GetStat(EffectStat.Radius);
			for (int i = 0; i < BoardSize; i++)
			{
				if (!IsActive(i)) continue;
				var enemy = PieceOn(i);
				if (enemy == null || enemy.Color == Piece.Color) continue;
				if (Distance(enemy.Pos, Piece.Pos) > radius) continue;
				if (!auraBlinded.Contains(enemy)) continue;

				for (int j = 0; j < enemy.Effects.Count; j++)
				{
					var effect = enemy.Effects[j];
					if (effect is Blinded)
					{
						ActionManager.EnqueueAction(new RemoveEffect(effect));
						break;
					}
				}
				auraBlinded.Remove(enemy);
			}
		}

		private static bool HasBlinded(PieceLogic target)
		{
			if (target == null) return false;
			for (int i = 0; i < target.Effects.Count; i++)
			{
				if (target.Effects[i] is Blinded) return true;
			}
			return false;
		}

    }
}
