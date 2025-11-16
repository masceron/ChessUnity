using Game.Tile;
using Game.Piece.PieceLogic;
using Game.Managers;
using static Game.Common.BoardUtils;
using UnityEngine;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BobtailSquidActive : Action, ISkills
    {
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
            bool Color = PieceOn(Maker).Color;
            int direction = Color ? -1 : +1;
            int steps = 0;
            (int oldRank, int oldFile) = RankFileOf(Maker);
            
            while(IsActive(IndexOf(oldRank + (steps+1)*direction, oldFile)) && steps < 3)
            {
                steps++;
            }
            int finalPos = IndexOf(oldRank + steps * direction, oldFile);
            MatchManager.Ins.GameState.Move(Maker, (ushort)finalPos);
            PieceManager.Ins.Move(Maker, (ushort)finalPos);
            // neu la den(true): offset 0, 1; neu la trang(false) : -1, 0
            for (int x = oldRank + (Color ? 0 : -1); x <= oldRank + (Color ? 1 : 0); ++x){ 
                for (int y = oldFile - 1; y <= oldFile + 1; ++y){  
                    if (IsActive(IndexOf(x, y)))
                    {
                        Formation FogOfWar = new FogOfWar(Color);
                        FogOfWar.SetDuration(3);
                        FormationManager.Ins.SetFormation(IndexOf(x, y), FogOfWar);
                    }
                    
                }
            }
            
        }
    }
} 