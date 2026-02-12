using MemoryPack;
using Game.Tile;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BobtailSquidActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        public BobtailSquidActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        protected override void Animate()
        {
            // (int rank, int file) = RankFileOf(Maker);
            // PieceManager.Ins.Move(Maker, IndexOf(rank - 3, file));
        }
        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            var Color = PieceOn(Maker).Color;
            var direction = Color ? -1 : +1;
            var steps = 0;
            var (oldRank, oldFile) = RankFileOf(Maker);
            
            while(IsActive(IndexOf(oldRank + (steps+1)*direction, oldFile)) && steps < 3)
            {
                steps++;
            }
            var finalPos = IndexOf(oldRank + steps * direction, oldFile);
            MatchManager.Ins.GameState.Move(Maker, (ushort)finalPos);
            PieceManager.Ins.Move(Maker, (ushort)finalPos);
            for (var x = oldRank + (Color ? 0 : -1); x <= oldRank + (Color ? 1 : 0); ++x){
                for (var y = oldFile - 1; y <= oldFile + 1; ++y){  
                    if (IsActive(IndexOf(x, y)))
                    {
                        Formation FogOfWar = new FogOfWar(Color);
                        FogOfWar.SetDuration(3);
                        SetFormation(IndexOf(x, y), FogOfWar);
                    }
                    
                }
            }
            
        }
    }
} 