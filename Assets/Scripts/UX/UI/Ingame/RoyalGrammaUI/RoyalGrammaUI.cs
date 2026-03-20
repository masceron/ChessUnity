using Game.Action.Internal.Pending.Piece;
using Game.Action.Relics;
using Game.Common;
using Game.Piece.PieceLogic;
using Game.Relics.Commons;

namespace UX.UI.Ingame.RoyalGrammaUI
{
    public class RoyalGrammaUI : Singleton<RoyalGrammaUI>
    {
        private RoyalGrammaPending royalGramma;
        public void Load(RoyalGrammaPending royalGramma)
        {
            this.royalGramma = royalGramma;
        }
        public void CommitResult(string chosenType)
        {
            royalGramma.CommitResult(chosenType);
        }
    }
}