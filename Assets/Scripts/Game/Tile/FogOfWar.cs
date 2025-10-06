

using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Effects;
using UnityEngine;
namespace Game.Tile
{
    public class FogOfWar : Formation
    {
        public bool color;
        public FogOfWar(bool color, int duration)
        {
            this.color = color;
            this.duration = duration;
        }
        public override void OnPieceEnter(PieceLogic piece)
        {
            if (piece.Color != color)
            {
                FormationManager.Ins.RemoveEnviroment(piece.Pos);
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