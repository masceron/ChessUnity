using System.Collections.Generic;
using System.Linq;
using Game.Piece;
using Game.ScriptableObjects;
using Game.Managers;
using UX.UI.Army.DesignArmy;
using ZLinq;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FPArmySearcher: ArmySearcher
    {
        public new static FPArmySearcher Ins;
        protected override void Awake()
        {
            base.Awake();
            Ins = this;
        }
        public override void Load()
        {
            gameObject.SetActive(true);
            data = AssetManager.Ins.PieceData;
            FPArmyDesign.Ins.board.OnAddTroop += (_) => FilterByCondition();
            FPArmyDesign.Ins.board.OnRemoveTroop += (_) => FilterByCondition();
            lastSearchResult = data.Values.ToList();
            FilterByCondition();
            SearchByKeyword("");
        }

        // Dựa trên một vài luật mà game đưa ra, một số Piece có thể sẽ không còn đặt được, bị greyout sau đó
        private new void FilterByCondition(){
            greyOutPieces.Clear();
            // Lọc theo một số condition 
            Dictionary<PieceInfo, int> counts = new();
            
            foreach (var pieceInfo in FPArmyDesign.Ins.board.Troops.Select(tr => AssetManager.Ins.PieceData[tr.PieceType]))
            {
                counts.TryAdd(pieceInfo, 0);
                counts[pieceInfo]++;
                //Chỉ có một Commander ở mỗi side, nên khi add vào thì phải grey out toàn bộ commander còn lại
                if (pieceInfo.rank == PieceRank.Commander)
                {
                    foreach (var t in AssetManager.Ins.PieceData.Values)
                    {
                        if (t.rank == PieceRank.Commander)
                        {
                            greyOutPieces.Add(t);
                        }
                    }
                }
                // Champion: Tối đa 2 quân giống nhau / biến thể của nhau 
                if (pieceInfo.rank == PieceRank.Champion && counts[pieceInfo] == 2)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                //Elite: Tối đa 4 quân giống nhau / biến thể của nha
                if (pieceInfo.rank == PieceRank.Elite && counts[pieceInfo] == 4)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                //Common: tối đa 10 quân giống nhau / biến thể của nhau
                if (pieceInfo.rank == PieceRank.Common && counts[pieceInfo] == 10)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                // Construct: 1 quân mỗi bên, giới hạn ở nửa bàn cờ bên mình
                if (pieceInfo.rank == PieceRank.Construct)
                {
                    foreach (var t in FPArmyDesign.Ins.board.Troops)
                    {
                        if (t.GetPieceInfo().rank == PieceRank.Construct)
                        {
                            greyOutPieces.Add(t.GetPieceInfo());
                        }
                    }
                }
            }
            DisplaySearchResult();
        }
    }
}