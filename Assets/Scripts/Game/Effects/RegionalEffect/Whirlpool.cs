using Game.Action;
using Game.Action.Quiets;
using Game.Managers;
using UnityEngine;

//Xuất hiện vào turn thứ 30 của ván nếu chưa kết thúc, tạo ra vòng xoáy 2x2 giữa bàn cờ, 
// mỗi turn hút tất cả các quân vào gần với xoáy nước 1 ô. 
//Quân nào bị vào xoáy nước auto mất
namespace Game.Effects.RegionalEffect
{
    public class Whirlpool : RegionalEffect{
        private int startTurn = 4;
        public Whirlpool() : base(RegionalEffectType.Whirpool){

        }
        protected override void ApplyEffect(int currentTurn)
        {
            if (currentTurn <= startTurn){
                Debug.Log("Whirlpool is not started yet!");
                return;
            }
            var board = MatchManager.Ins.GameState.PieceBoard;
            foreach(var piece in board)
            {
                if (piece == null) continue;
                const int targetPosition = 0;
                ActionManager.ExecuteImmediately(new NormalMove(piece.Pos, targetPosition));
            }

        }
    }
}