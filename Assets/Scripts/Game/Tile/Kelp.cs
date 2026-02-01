using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Kelp : Formation
    {
        private bool pieceHaveCamouflage;
        public Kelp(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.Kelp;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            if (piece.Effects.Any(effect => effect.EffectName == "effect_camouflage"))
            {
                pieceHaveCamouflage = true;
            } else {
                ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(piece), FormationType.Kelp));
            }
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            if (!pieceHaveCamouflage && piece.Effects.Any(effect => effect.EffectName == "effect_camouflage"))
            {
                ActionManager.EnqueueAction(new RemoveEffect(piece.Effects.Find(effect => effect.EffectName == "effect_camouflage")));
            }
            
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -10;
        }
    }
}

