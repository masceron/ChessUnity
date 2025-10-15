using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using UnityEngine;
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
            this.haveDuration = haveDuration;
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
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, piece)));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

    }

}
