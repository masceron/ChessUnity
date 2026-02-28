using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class BrainCoralPassive : Effect, IStartTurnTrigger
    {
        public BrainCoralPassive(PieceLogic piece) : base(-1, 1, piece, "effect_brain_coral_passive")
        {
            SetStat(EffectStat.Target, 1);
            SetStat(EffectStat.Duration, 1);
            SetStat(EffectStat.Radius, 3);
        }
        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;
        public void OnCallStart(Action.Action lastMainAction)
        {
            int targetCount = GetStat(EffectStat.Target);
            var enemies = new List<PieceLogic>();
            
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), GetStat(EffectStat.Radius)))
            {
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color != Piece.Color)
                {
                    enemies.Add(pieceOn);
                }
            }
            
            if (enemies.Count == 0) return;
            
            // Chọn random targetCount quân địch
            int actualTarget = System.Math.Min(targetCount, enemies.Count);
            var chosen = new List<PieceLogic>();
            
            for (int i = 0; i < actualTarget; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, enemies.Count);
                chosen.Add(enemies[randomIndex]);
                enemies.RemoveAt(randomIndex);
            }
            
            foreach (var enemy in chosen)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Controlled(GetStat(EffectStat.Duration), enemy)));
            }
        }
    }
}