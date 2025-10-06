
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Effects;
using UnityEngine;

namespace Game.Tile{
    public enum FormationType{
        None,
        FogOfWar,
        BubbleVent,
    }
    public abstract class Formation {
        public int duration = -1;
        /// <summary>
        /// Trả về FormationType tương ứng với class
        /// </summary>
        public abstract FormationType GetFormationType();
        /// <summary>
        /// Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân đi vào vị trí
        /// </summary>
        public virtual void OnPieceEnter(PieceLogic piece){

        }
        /// <summary>
        /// Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân rời khỏi vị trí
        /// </summary>
        public virtual void OnPieceExit(PieceLogic piece){

        }
    }

}