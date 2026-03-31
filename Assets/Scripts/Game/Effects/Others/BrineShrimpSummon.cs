using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using System.Collections.Generic;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class BrineShrimpSummon : Effect, IEndTurnTrigger, ISkills
    {
        private int _duration;
        private static readonly System.Random Rng = new();

        public BrineShrimpSummon(PieceLogic piece, int duration) : base(-1, 1, piece, "effect_brine_shrimp_summon")
        {
            _duration = duration;
            Debug.Log($"Duration: {_duration}");
        }

        EndTurnTriggerPriority IEndTurnTrigger.Priority => EndTurnTriggerPriority.Other;

        EndTurnEffectType IEndTurnTrigger.EndTurnEffectType => EndTurnEffectType.EndOfEnemyTurn;

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }

        int ISkills.AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        void IEndTurnTrigger.OnCallEnd(Action.Action lastMainAction)
        {
            Debug.Log("Petrified Duration: " + _duration);

            if (_duration == 0)
            {
                SpawnJuvenileBrineShrimp();
                ActionManager.EnqueueAction(new RemoveEffect(this));

            }

            _duration--;
        }

        void SpawnJuvenileBrineShrimp()
        {
            var availablePieces = new List<int>();
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), 1))
            {
                var piece = PieceOn(IndexOf(rank, file));
                if (piece == null)
                {
                    availablePieces.Add(IndexOf(rank, file));
                }
            }

            Shuffle(availablePieces);

            for (int i = 0; i < Piece.GetStat(SkillStat.Target); ++i)
            {
                if (i >= availablePieces.Count)
                {
                    Debug.Log("No more available spaces to spawn Brine Shrimp.");
                    return;
                }

                ActionManager.EnqueueAction(new SpawnPiece(new Game.Piece.PieceConfig("piece_juvenile_brine_shrimp", Piece.Color, availablePieces[i])));

            }
        }

        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}