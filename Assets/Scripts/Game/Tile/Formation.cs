
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Effects;
using UnityEngine;

namespace Game.Tile{
    public enum FormationType{
        None,
        FogOfWar,
        BubbleVent,
        DazzlingLight,
    }
    // public interface IHaveDuration{
    //     /// <summary>
    //     /// Sử dụng cho những Formation tồn tại trong 1 khoảng thời gian
    //     /// Logic trừ duration được thực hiện ở FormationManager, bạn chỉ cần gán duration khi khởi tạo
    //     /// </summary>
    //     public int duration{ get; set; }
    // }
    public abstract class Formation{
        private bool color = false;
        public bool haveDuration{ get; protected set; }
        public int duration{ get; protected set; }
        public Formation(bool color){
            this.color = color;
        }
        public bool GetColor(){
            return color;
        }
        public void SetDuration(int duration){
            this.duration = duration;
            haveDuration = true;
        }
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

        public virtual void OnFirstTurn(PieceLogic piece){

        }
    }

}