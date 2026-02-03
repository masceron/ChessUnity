using System.Collections.Generic;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CorruptedWisper : RelicLogic
    {
        public readonly CorruptedWisperCharge corruptedWisperCharge;
        private int currentLevel;
        private List<PieceRank> possibleRank;
        public CorruptedWisper(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            CurrentCooldown = 0;
            currentLevel = 1;
            possibleRank = new List<PieceRank> { PieceRank.Swarm, PieceRank.Summoned, PieceRank.Common };
            
            corruptedWisperCharge = new CorruptedWisperCharge(3, config.Color);
            BoardUtils.AddEffectObserver(corruptedWisperCharge);
        }

        public void LevelUp()
        {
            currentLevel++;
            if (currentLevel == 2)
            {
                possibleRank.Add(PieceRank.Elite);
            }
            else if (currentLevel == 3)
            {
                possibleRank.Add(PieceRank.Champion);
            }
        }
        public override void Activate()
        {
            
            if (corruptedWisperCharge.Strength > 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Color == Color) continue;
                    if (possibleRank.Contains(piece.PieceRank) == false) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new CorruptedWisperPending(piece.Pos, this);
                    BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;

                corruptedWisperCharge.Strength--;
            }
           
           
        }

        
    }
}
