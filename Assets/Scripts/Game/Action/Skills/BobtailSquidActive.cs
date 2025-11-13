using Game.Tile;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
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
            (int rank, int file) = RankFileOf(Maker);
            PieceManager.Ins.Move(Maker, IndexOf(rank - 3, file));
        }
        protected override void ModifyGameState()
        {
            Debug.Log("Execute BobtailSquidActive");
            (int rank, int file) = RankFileOf(Maker);
            for (int x = rank; x <= rank + 1; ++x){
                for (int y = file - 1; y <= file + 1; ++y){
                    Formation FogOfWar = new FogOfWar(PieceOn(Maker).Color);
                    FogOfWar.SetDuration(3);
                    FormationManager.Ins.SetFormation(IndexOf(x, y), FogOfWar);
                }
            }
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            MatchManager.Ins.GameState.Move(Maker, (ushort)IndexOf(rank - 3, file));
        }
    }
}