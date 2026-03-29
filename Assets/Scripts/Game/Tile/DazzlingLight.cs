using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    /// <summary>
    ///     Dazzling Light Tile
    ///     Gây hiệu ứng Blinded lên quân cờ trong 1 turn
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DazzlingLight : Formation
    {
        public DazzlingLight(bool color) : base(color)
        {

        }

        public override FormationType GetFormationType()
        {
            return FormationType.DazzlingLight;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(1, 100, piece)));
        }

        public override int GetValueForAI()
        {
            return -40;
        }
    }
}