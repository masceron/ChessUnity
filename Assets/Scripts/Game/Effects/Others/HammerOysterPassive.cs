using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Relics;
using static Game.Common.BoardUtils;
using static Config;

namespace Game.Effects.Others
{
    public class HammerOysterPassive : Effect, IEndTurnEffect
    {
        public HammerOysterPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.HammerOysterPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var piecePos = Piece.Pos;
            if (!IsAtPromotionRank(piecePos)) return;
            ActionManager.EnqueueAction(new KillPiece(piecePos));

            if (Piece.Color == true)
            {
                var blackRelic = GetRelicOf(true);
                if (blackRelic is not { Type: RelicType.CommonPearl }) return;
                MatchManager.Ins.GameState.BlackRelic =
                    GameState.GetRelicLogicByConfig(new RelicConfig(RelicType.BlackPearl, true, 4));
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
                if (whiteRelic is not { Type: RelicType.CommonPearl }) return;
                MatchManager.Ins.GameState.WhiteRelic =
                    GameState.GetRelicLogicByConfig(new RelicConfig(RelicType.BlackPearl, false, 4));
            }

            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}