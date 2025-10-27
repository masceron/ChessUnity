using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic.Swarm
{
    public class PistolShrimp : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public PistolShrimp(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, SmallChargingMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);
                var color = Color;

                switch (color)
                {
                    case false :
                        foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, 1))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn == this) continue;
                            list.Add(new PistolShrimpActive(Pos, index));
                        }
                        break;
                    case true:
                        foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, 1))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn == this) continue;
                            list.Add(new PistolShrimpActive(Pos, index));
                        }
                        break;
                }

            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}