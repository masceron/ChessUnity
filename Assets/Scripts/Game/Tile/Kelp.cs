using Game.Action;
using Game.Piece.PieceLogic;
using Game.Action.Internal;
using Game.Effects.Buffs;
using System.Linq;
using Game.Effects;

namespace Game.Tile
{


    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Kelp : Formation
    {
        public Kelp() : base(false)
        {
        }

        public override FormationType GetFormationType()
        {
            return FormationType.Kelp;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            if (!piece.Effects.Any(effect => effect.EffectName == EffectName.Camouflage))
            {
                ApplyEffect(piece, new Camouflage(piece));
            }
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            if (piece.Effects.Any(effect => effect.EffectName == EffectName.Camouflage))
            {
                ActionManager.ExecuteImmediately(new RemoveEffect(piece.Effects.Find(effect => effect.EffectName == EffectName.Camouflage)));
            }
            
            base.OnPieceExit(piece);
        }

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }
    }
}

