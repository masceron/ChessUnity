using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class BrokenRelocatorPassive : Effect, IAfterPieceActionEffect, IOnApply
    {
        private Effect Relentless;
        private const int Radius = 5;
        private readonly List<int> possiblePositions = new();
        public BrokenRelocatorPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_broken_relocator_passive")
        { }

        public void OnApply()
        {
            foreach (var effect in Piece.Effects)
            {
                if (effect.EffectName == "effect_relentless")
                {
                    if (effect.Strength > 0)
                    {
                        effect.Strength--;
                        Relentless = effect;
                        break;
                    }
                }
            }
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action == null || action is not ICaptures) return;
            
            if (action.Target != Piece.Pos) return;

            var (nRank, nFile) = BoardUtils.RankFileOf(action.Maker);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(nRank, nFile, Radius))
            {
                possiblePositions.Add(BoardUtils.IndexOf(r, f));
            }

            int idx;
            do
            {
                idx = UnityEngine.Random.Range(0, possiblePositions.Count);

            } while (BoardUtils.PieceOn(possiblePositions[idx]) != null && BoardUtils.PieceOn(possiblePositions[idx]).Color == Piece.Color);

            if (BoardUtils.PieceOn(possiblePositions[idx]) != null)
            {
                ActionManager.EnqueueAction(new KillPiece(possiblePositions[idx]));
            }
            
            MatchManager.Ins.GameState.Move(action.Target, possiblePositions[idx]);
        }
    }
}