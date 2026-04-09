using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Piece;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CorruptedWisper : RelicLogic
    {
        public readonly CorruptedWisperCharge corruptedWisperCharge;
        private readonly List<PieceRank> possibleRank;
        private int currentLevel;

        public CorruptedWisper(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            CurrentCooldown = 0;
            currentLevel = 1;
            possibleRank = new List<PieceRank> { PieceRank.Swarm, PieceRank.Summoned, PieceRank.Common };

            corruptedWisperCharge = new CorruptedWisperCharge(0, config.Color);
            BoardUtils.AddEffectObserver(corruptedWisperCharge);
        }

        public void LevelUp()
        {
            currentLevel++;
            if (currentLevel == 2)
                possibleRank.Add(PieceRank.Elite);
            else if (currentLevel == 3) possibleRank.Add(PieceRank.Champion);
        }
        
        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }
    }
}