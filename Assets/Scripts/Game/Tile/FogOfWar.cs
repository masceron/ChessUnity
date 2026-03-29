using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Tile
{
    /// <summary>
    /// Che khuất tầm nhìn của quân địch bên trong fog. Quân địch đi lên fog mình, fog sẽ biến mất
    /// </summary>
    public class FogOfWar : Formation
    {
        public FogOfWar(bool color) : base(color)
        {
            
        }

        public override void OnCreated(PieceLogic piece)
        {
            if (piece.Color != BoardUtils.OurSide())
            {
                // ToggleVisibility(piece, false);
            }
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            if (piece.Color != Color) BoardUtils.RemoveFormation(piece.Pos);
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            // ToggleVisibility(piece, true);
        }

        private void ToggleVisibility(PieceLogic piece, bool value)
        {
            piece.IsVisible = value;
            PieceManager.Ins.GetPieceGameObject(piece.Pos).GetComponent<MeshRenderer>().enabled = value;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.FogOfWar;
        }

        public override int GetValueForAI()
        {
            return 20;
        }
    }
}