using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class SnappingStrike: Effect
    {
        public SnappingStrike(PieceLogic piece) : base(-1, -1, piece, Effects.EffectName.SnappingStrike)
        {}

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description; 
        }
    }
}