using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Poison: Effect, IEndTurnEffect
    {
        private byte timeLeft = 3;

        public Poison(sbyte strength, PieceLogic piece) : base(-1, strength, piece, EffectName.Poison)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (Strength >= 5) timeLeft--;
            if (timeLeft <= 0)
            {
                ActionManager.EnqueueAction(new DestroyPiece(Piece.Pos));
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, timeLeft);
        }
    }
}