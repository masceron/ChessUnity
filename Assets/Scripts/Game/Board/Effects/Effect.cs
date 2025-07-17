using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects
{

    public enum EffectType
    {
        Carapace,
        Evasion,
        Surpass,
        Slow,
        Blinded,
        Ambush,
        VelkarisMarked,
        SirenDebuffer,
        VelkarisMarker,
        Demolisher,
        Vengeful,
        Stunned,
        Shield,
        HardenedShield,
        Piercing
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: Observer
    {
        public sbyte Duration;
        protected sbyte Strength;
        public readonly PieceLogic Piece;

        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, EffectType type) : base(type)
        {
            Duration = duration;
            Strength = strength;
            Piece = piece;
        }

        public virtual void OnApply()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }
    }
}