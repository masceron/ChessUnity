using Game.Action.Internal;
using Game.Piece.PieceLogic;
using UnityEngine;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Tile;


namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GrenadiersActive: Action, ISkills
    {
        public GrenadiersActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.SetFormation(Target, new Kelp());
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}