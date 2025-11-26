using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class HammerOysterPassive : Effect, IEndTurnEffect
    {
        public HammerOysterPassive(PieceLogic piece) : base(-1, 1, piece, "effect_hammer_oyster_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var piecePos = Piece.Pos;
            if (!IsAtPromotionRank(piecePos)) return;
            ActionManager.EnqueueAction(new KillPiece(piecePos));

            if (Piece.Color)
            {
                var blackRelic = GetRelicOf(true);
                if (blackRelic is not { type: "relic_common_pearl" }) return;
                MatchManager.Ins.GameState.BlackRelic =
                    RelicMaker.Get(new RelicConfig("relic_black_pearl", true, 4));
                // var whiteRelic = GetRelicOf(false);
                // if (whiteRelic != null && whiteRelic.Type == RelicType.CommonPearl)
                // {
                //     relicWhiteConfig = new RelicConfig(RelicType.BlackPearl, false, 4);
                //     MatchManager.Ins.GameState.WhiteRelic = GameState.GetRelicLogicByConfig(relicWhiteConfig);
                //     MatchManager.Ins.InputProcessor.UpdateRelic();
                // }
            }
            else
            {
                var whiteRelic = GetRelicOf(false);
                if (whiteRelic is not { type: "relic_common_pearl" }) return;
                MatchManager.Ins.GameState.WhiteRelic =
                    RelicMaker.Get(new RelicConfig("relic_black_pearl", false, 4));
            }

            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}