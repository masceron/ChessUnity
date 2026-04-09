using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class HammerOysterPassive : Effect, IEndTurnTrigger
    {
        public HammerOysterPassive(PieceLogic piece) : base(-1, 1, piece, "effect_hammer_oyster_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var piecePos = Piece.Pos;
            if (!IsAtPromotionRank(piecePos)) return;
            ActionManager.EnqueueAction(new KillPiece(null, Piece));

            var relic = GetRelicOf(Piece.Color);
            if (relic is not { type: "relic_common_pearl" }) return;
            SetRelic(Piece.Color, RelicMaker.Get(new RelicConfig("relic_black_pearl", Piece.Color, 4)));

            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}