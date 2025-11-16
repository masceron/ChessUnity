using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Others
{
    public class EpauletteSharkPurify : Effect, IEndTurnEffect
    {
        private bool yesterdayIsDay;
        public EpauletteSharkPurify(PieceLogic piece) : base(-1, 1, piece, EffectName.EpauletteSharkPurify)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!MatchManager.Ins.GameState.IsDay && yesterdayIsDay)
            {
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
            }
            yesterdayIsDay = MatchManager.Ins.GameState.IsDay;
        }
    }
}