using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    /// <summary>
    /// Gây hiệu ứng Frenzied 4 turn lên quân địch đi vào. Sau khi gây hiệu ứng, tự phá hủy formation này.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HopkinsRose : Formation
    {
        public HopkinsRose(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.HopkinsRose;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            if (piece != null && piece.Color != Color)
            {
                var frenziedEffect = new Frenzied(piece, 4);
                var applyEffectAction = new ApplyEffect(frenziedEffect);
                ActionManager.EnqueueAction(applyEffectAction);
                ActionManager.EnqueueAction(new ApplyEffectAndRemoveFormation(applyEffectAction, this));
            }
        }

        public override int GetValueForAI()
        {
            return -70;
        }
    }
}