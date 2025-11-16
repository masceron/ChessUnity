

using Game.Piece.PieceLogic;
using Game.Managers;
using UnityEngine;
using Game.Common;
using UnityEngine.UI;
namespace Game.Tile
{
    public class FogOfWar : Formation
    {
        public FogOfWar(bool color) : base(color)
        {

        }
        public override void OnCreated(PieceLogic piece)
        {
            if (piece.Color != BoardUtils.OurSide())
            {
                ToggleVisibility(piece, false);
            }
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            if (piece.Color != Color)
            {
                FormationManager.Ins.RemoveFormation(piece.Pos);
            }
            else
            {
                // Do nothing
            }
            
        }
        public override void OnPieceExit(PieceLogic piece)
        {
            ToggleVisibility(piece, true);
        }
        void ToggleVisibility(PieceLogic piece, bool value)
        {
            piece.IsVisible = value;
            PieceManager.Ins.GetPieceGameObject(piece.Pos).GetComponent<MeshRenderer>().enabled = value;
        }
        public override FormationType GetFormationType()
        {
            return FormationType.FogOfWar;
        }
    }
}