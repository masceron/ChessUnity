using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Debuffs;

namespace Game.Tile
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
                var applyEffectAction = new ApplyEffect(frenziedEffect, GetFormationType());
                ActionManager.EnqueueAction(applyEffectAction);
                ActionManager.EnqueueAction(new ApplyEffectAndRemoveFormation(applyEffectAction, Pos));
            }
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -70;
        }
    }
}