using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class BrineShrimpSummon : Effect, IStartTurnTrigger, ISkills
    {
        private Petrified petrified;

        public BrineShrimpSummon(PieceLogic piece) : base(-1, 1, piece, "effect_brine_shrimp_summon")
        {
            petrified = Piece.Effects.OfType<Petrified>().FirstOrDefault();
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Other;

        StartTurnEffectType IStartTurnTrigger.StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }

        int ISkills.AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        void IStartTurnTrigger.OnCallStart(Action.Action lastMainAction)
        {
            if (lastMainAction.Maker != Piece.Pos) return;
            if (Piece == null) return;
            if (petrified == null) return;

            if (petrified.Duration <= 0)
            {
                var availableIndex = MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), 1);
                var idx = UnityEngine.Random.Range(0, availableIndex.Count());
                var (rank, file) = availableIndex.ElementAt(idx);

                ActionManager.EnqueueAction(new SpawnPiece(new Game.Piece.PieceConfig("piece_juvenile_brine_shrimp", Piece.Color, IndexOf(rank, file))));
                
                petrified = null;
            }
        }
    }
}