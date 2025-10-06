using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Tile;
using Game.Piece.PieceLogic;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BobtailSquidActive : Action, ISkills
    {
        public BobtailSquidActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }
        protected override void Animate()
        {
            (int rank, int file) = RankFileOf(Maker);
            PieceManager.Ins.Move(Maker, IndexOf(rank - 3, file));
        }
        protected override void ModifyGameState()
        {
            (int rank, int file) = RankFileOf(Maker);
            for (int x = rank - 1; x <= rank + 1; ++x){
                for (int y = file; y <= file + 1; ++y){
                    FormationManager.Ins.SetFormation(IndexOf(x, y), new FogOfWar(PieceOn(Maker).Color, 3));
                }
            }
            MatchManager.Ins.GameState.Move(Maker, (ushort)IndexOf(rank - 3, file));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}