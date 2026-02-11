using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStoneExcute : Action, IRelicAction
    {
        private readonly bool isFirstOption;
        
        public LedgerStoneExcute(bool isFirstOption) : base(-1)
        {
            this.isFirstOption = isFirstOption;
        }

        protected override void ModifyGameState()
        {
            UnityEngine.Debug.Log("LedgerStoneExcute: " + isFirstOption);
            var startingSizeY = (MaxLength - MatchManager.Ins.StartingSize.y) / 2;
            var startingSizeX = (MaxLength - MatchManager.Ins.StartingSize.x) / 2;
            
            if (isFirstOption)
            {
                var isFillColumn = false;
                if(!IsColumnFull(startingSizeY)){
                    isFillColumn = true;
                }
                if(isFillColumn)
                {
                    for (var rank = startingSizeX; rank < startingSizeX + MatchManager.Ins.StartingSize.x; rank++)
                    {
                        var index = IndexOf(rank, startingSizeY);
                        if (TileManager.Ins.IsTileEmpty(index))
                        {
                            TileManager.Ins.ActivateTile(rank, startingSizeY);
                        }
                    }
                } else {
                    var leftColumnFile = startingSizeY - 1;
                    if (leftColumnFile >= 0)
                    {
                        for (var rank = startingSizeX; rank < startingSizeX + MatchManager.Ins.StartingSize.x; rank++)
                        {
                            TileManager.Ins.ActivateTile(rank, leftColumnFile);
                        }
                    }
                }

                isFillColumn = false;
                var RightColumnFile = (MaxLength - MatchManager.Ins.StartingSize.y) / 2 + MatchManager.Ins.StartingSize.x - 1;
                if(!IsColumnFull(RightColumnFile)){
                    isFillColumn = true;
                }
                if(isFillColumn)
                {
                    for (var rank = startingSizeX; rank < startingSizeX + MatchManager.Ins.StartingSize.x; rank++)
                    {
                        var index = IndexOf(rank, RightColumnFile);
                        if (TileManager.Ins.IsTileEmpty(index))
                        {
                            TileManager.Ins.ActivateTile(rank, RightColumnFile);
                        }
                    }
                } else {
                    RightColumnFile++;
                    if (RightColumnFile >= 0)
                    {
                        for (var rank = startingSizeX; rank < startingSizeX + MatchManager.Ins.StartingSize.x; rank++)
                        {
                            TileManager.Ins.ActivateTile(rank, RightColumnFile);
                        }
                    }
                }
            }
            else
            {
                var isFillRow = false;
                if(!IsRowFull(startingSizeX)){
                    isFillRow = true;
                }
                if(isFillRow)
                {
                    for (var file = startingSizeY; file < startingSizeY + MatchManager.Ins.StartingSize.y; file++)
                    {
                        var index = IndexOf(startingSizeX, file);
                        if (TileManager.Ins.IsTileEmpty(index))
                        {
                            TileManager.Ins.ActivateTile(startingSizeX, file);
                        }
                    }
                } else {
                    var topRowRank = startingSizeX - 1;
                    if (topRowRank >= 0)
                    {
                        for (var file = startingSizeY; file < startingSizeY + MatchManager.Ins.StartingSize.y; file++)
                        {
                            TileManager.Ins.ActivateTile(topRowRank, file);
                        }
                    }
                }

                isFillRow = false;
                var bottomRowRank = startingSizeX + MatchManager.Ins.StartingSize.x - 1;
                if(!IsRowFull(bottomRowRank)){
                    isFillRow = true;
                }
                if(isFillRow)
                {
                    for (var file = startingSizeY; file < startingSizeY + MatchManager.Ins.StartingSize.y; file++)
                    {
                        var index = IndexOf(bottomRowRank, file);
                        if (TileManager.Ins.IsTileEmpty(index))
                        {
                            TileManager.Ins.ActivateTile(bottomRowRank, file);
                        }
                    }
                } else {
                    var bottomRowRankPlus = bottomRowRank + 1;
                    if (bottomRowRankPlus < MaxLength)
                    {
                        for (var file = startingSizeY; file < startingSizeY + MatchManager.Ins.StartingSize.y; file++)
                        {
                            TileManager.Ins.ActivateTile(bottomRowRankPlus, file);
                        }
                    }
                }
            }
        }

        private bool IsColumnFull(int column)
        {
            var startingSizeX = (MaxLength - MatchManager.Ins.StartingSize.x) / 2;
            for (var i = startingSizeX; i < startingSizeX + MatchManager.Ins.StartingSize.x; i++)
            {
                UnityEngine.Debug.Log("IsColumnFull: " + i + ", " + column + ", " + MatchManager.Ins.StartingSize.x);
                if(TileManager.Ins.IsTileEmpty(IndexOf(i,column))) 
                {
                    UnityEngine.Debug.Log("IsNotColumnFull");
                    return false;
                }
            }
            return true;
        }
        
        private bool IsRowFull(int row)
        {
            var startingSizeY = (MaxLength - MatchManager.Ins.StartingSize.y) / 2;
            for (var file = startingSizeY; file < startingSizeY + MatchManager.Ins.StartingSize.y; file++)
            {
                if(TileManager.Ins.IsTileEmpty(IndexOf(row, file))) 
                {
                    return false;
                }
            }
            return true;
        }
    }
}
