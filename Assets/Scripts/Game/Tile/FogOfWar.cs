

using Game.Piece.PieceLogic;
using Game.Managers;
using UnityEngine;
namespace Game.Tile
{
    public class FogOfWar : Formation
    {
        public FogOfWar(bool color) : base(color){
            
        }
    public override void OnPieceEnter(PieceLogic piece)
        {
            if (piece.Color != GetColor())
            {
                FormationManager.Ins.RemoveFormation(piece.Pos);
            }
            ToggleVisibility(piece, false);
        }
        public override void OnPieceExit(PieceLogic piece)
        {
            ToggleVisibility(piece, true);
        }
        void ToggleVisibility(PieceLogic piece, bool value)
        {
            piece.isClickable = value;
            PieceManager.Ins.GetPieceGameObject(piece.Pos).GetComponent<MeshRenderer>().enabled = value;
        }
        public override FormationType GetFormationType()
        {
            return FormationType.FogOfWar;
        }
    }
}