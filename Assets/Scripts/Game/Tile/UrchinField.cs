using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Tile
{
    /// <summary>
    /// Urchin Field Tile
    /// </summary>
    ///       
    public class UrchinField : Formation
    {
        public UrchinField(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.UrchinField;
        }

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            //Theo Tân bảo thì bleed không cộng dồn, mà sẽ reset lại 4 turn
            var existingBleeding = piece.Effects.OfType<Bleeding>().ToList();

            foreach (var bleeding in existingBleeding)
            {
                ActionManager.EnqueueAction(new RemoveEffect(bleeding));
            }

            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, piece), FormationType.UrchinField));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -20;
        }
    }

}
