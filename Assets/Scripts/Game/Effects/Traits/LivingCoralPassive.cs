using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LivingCoralPassive : Effect, IStartTurnEffect, IOnApply, IAfterPieceActionEffect
    {
        private const int Interval = 3;
        private const int EvasionValue = 25;

        private int turnCounter;
        
        private readonly HashSet<PieceLogic> alliesInRange = new();

        public StartTurnEffectType StartTurnEffectType { get; }

        public LivingCoralPassive(PieceLogic piece)
            : base(-1, 1, piece, "effect_living_coral_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public void OnApply()
        {
            UpdateAura();
        }
        
        public void OnCallAfterPieceAction(Action.Action action)
        {
            UpdateAura();
        }

        public void OnCallStart(Action.Action lastMainAction)
        {
            turnCounter++;
            if (turnCounter % Interval == 0)
            {
                SummonClownFish();
            }
        }

        private void UpdateAura()
        {
            var newInRange = new HashSet<PieceLogic>();
            var (rank, file) = RankFileOf(Piece.Pos);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                var p = PieceOn(IndexOf(r, f));
                if (p == null || p == Piece) continue;
                if (p.Color != Piece.Color) continue;

                newInRange.Add(p);
            }
            
            foreach (var entered in newInRange.Except(alliesInRange))
            {
                ApplyEvasion(entered);
            }
            
            foreach (var exited in alliesInRange.Except(newInRange))
            {
                RemoveOrReduceEvasion(exited);
            }

            alliesInRange.Clear();
            foreach (var p in newInRange)
                alliesInRange.Add(p);
        }

        private void ApplyEvasion(PieceLogic piece)
        {
            if (piece.Effects.FirstOrDefault(e => e is Evasion) is Evasion evasion)
            {
                evasion.Probability += EvasionValue;
            }
            else
            {
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Evasion(-1, EvasionValue, piece))
                );
            }
        }

        private void RemoveOrReduceEvasion(PieceLogic piece)
        {
            if (piece.Effects.FirstOrDefault(e => e is Evasion) is not Evasion evasion) return;

            if (evasion.Probability <= EvasionValue)
            {
                ActionManager.EnqueueAction(new RemoveEffect(evasion));
            }
            else
            {
                evasion.Probability -= EvasionValue;
            }
        }

        private void SummonClownFish()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            List<int> emptyTiles = new();

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                int index = IndexOf(r, f);
                if (PieceOn(index) == null)
                {
                    emptyTiles.Add(index);
                }
            }

            if (emptyTiles.Count == 0) return;

            int rand = UnityEngine.Random.Range(0, emptyTiles.Count);
            ActionManager.EnqueueAction(
                new SpawnPiece(new PieceConfig(
                    "piece_clown_fish",
                    Piece.Color,
                    (ushort)emptyTiles[rand]))
            );
        }
        
    }
}
