using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, Effects.EffectName.Dominator)
        {}

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}