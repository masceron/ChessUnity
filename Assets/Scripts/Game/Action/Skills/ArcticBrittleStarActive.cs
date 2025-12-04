using static Game.Common.BoardUtils;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;
using Game.AI;

namespace Game.Action.Skills
{
    public class ArcticBrittleStarActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue => PieceOn(Target).Color != PieceOn(Maker).Color ? -5 : 0;

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

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}