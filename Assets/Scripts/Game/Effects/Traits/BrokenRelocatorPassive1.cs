using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using ZLinq;

namespace Game.Effects.Traits
{
    public class BrokenRelocatorPassive : Effect, IAfterPieceActionTrigger, IOnApplyTrigger
    {
        private const int Radius = 5;
        private readonly List<int> _possiblePositions = new();

        public BrokenRelocatorPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_broken_relocator_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            if (action.GetTarget() != Piece) return;

            var (nRank, nFile) = BoardUtils.RankFileOf(action.Maker);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(nRank, nFile, Radius))
                _possiblePositions.Add(BoardUtils.IndexOf(r, f));

            int idx;
            do
            {
                idx = Random.Range(0, _possiblePositions.Count);
            } while (BoardUtils.PieceOn(_possiblePositions[idx]) != null &&
                     BoardUtils.PieceOn(_possiblePositions[idx]).Color == Piece.Color);

            if (BoardUtils.PieceOn(_possiblePositions[idx]) != null)
                ActionManager.EnqueueAction(new KillPiece(_possiblePositions[idx]));

            MatchManager.Ins.GameState.Move(action.Target, _possiblePositions[idx]);
        }

        public void OnApply()
        {
            foreach (var effect in Piece.Effects.Where(effect => effect.EffectName == "effect_relentless")
                         .Where(effect => effect.Strength > 0))
            {
                effect.Strength--;
                break;
            }
        }
    }
}