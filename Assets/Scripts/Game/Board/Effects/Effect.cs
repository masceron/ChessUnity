using Game.Board.General;

namespace Game.Board.Effects
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: Observer
    {
        public sbyte Duration;
        protected sbyte Strength;
        public readonly PieceLogic.PieceLogic Piece;

        protected Effect(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece, ObserverType type, byte priority) : base(type, priority)
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