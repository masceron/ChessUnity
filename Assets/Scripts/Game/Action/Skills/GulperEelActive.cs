using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class GulperEelActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private GulperEelActive()
        {
        }

        public GulperEelActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("GulperEelActive: " + Target);
            Debug.Log("GulperEelActive: " + RankFileOf(Target));
            TileManager.Ins.DestroyTile(Target);

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        // public void CompleteActionForAI()
        // {
        //     var listTiles = new List<int>();
        //     var (rank, file) = RankFileOf(Maker);
        //     for (var dr = -1; dr <= 1; dr++)
        //     {
        //         var trank = rank + dr;
        //         if (!VerifyBounds(trank)) continue;
        //         for (var df = -1; df <= 1; df++)
        //         {
        //             var fileOff = file + df;
        //             if (!VerifyBounds(fileOff)) continue;
        //             var tpos = IndexOf(trank, fileOff);
        //             var pieceAt = PieceOn(tpos);
        //             if (pieceAt != null) continue;
        //             if (TileManager.Ins.IsTileEmpty(tpos)) continue;
        //             listTiles.Add(tpos);
        //         }
        //     }
        //     if (listTiles.Count == 0) return;
        //     var random = new System.Random();
        //     var selectedTile = listTiles[random.Next(listTiles.Count)];

        //     TileManager.Ins.DestroyTile(selectedTile);
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        // }
    }
}