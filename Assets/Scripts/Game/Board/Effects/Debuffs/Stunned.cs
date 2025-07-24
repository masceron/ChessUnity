using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Stunned: Effect
    {
        public Stunned(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectType.Stunned)
        {}

        public override string Description()
        {
            return string.Format(MatchManager.assetManager.EffectData[EffectName].description, Duration);
        }
    }
}