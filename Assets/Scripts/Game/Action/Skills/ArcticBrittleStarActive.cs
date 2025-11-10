using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Tile;
using UnityEngine;

namespace Game.Action.Skills
{
    public class ArcticBrittleStarActive : Action, ISkills
    {
        private Tile.Tile hoveringTile;
        public ArcticBrittleStarActive(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Execute Arctic Brittle Star");
            Formation AnchorIce = new AnchorIce(PieceOn(Maker).Color);
            AnchorIce.SetDuration(3);
            FormationManager.Ins.SetFormation(Target, AnchorIce);
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}