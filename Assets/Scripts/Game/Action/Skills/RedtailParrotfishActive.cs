using Game.Action.Internal.Pending;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RedtailParrotfishActive: PendingAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        public static int formationPos = -1;
        public static int moveTo = -1;
        RedtailParrotfish redtailParrotfish;
        public RedtailParrotfishActive(RedtailParrotfish maker, int target) : base(maker.Pos)
        {
            redtailParrotfish = maker;
            Target = (ushort)target;
        }
        // protected override void ModifyGameState()
        // {
        //     FormationManager.Ins.MoveFormation(formationPos, moveTo);
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        //     Reset();
        // }
        public override void CompleteAction()
        {
            if (formationPos == -1)
            {
                Debug.Log("[RedtailParrotfishActive] Choose destination!");
                formationPos = Target;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                for(int i = 0; i < BoardSize; ++i)
                {
                    if (!IsActive(i)) { return; }
                    Formation formation = FormationManager.Ins.GetFormation(i);
                    if(formation != null){ continue; }
                    if (IsOnBlackSide(Maker) == redtailParrotfish.Color)
                    {
                        BoardViewer.ListOf.Add(new RedtailParrotfishActive(redtailParrotfish, i));
                        TileManager.Ins.MarkAsMoveable(i);
                    }
                }
            }
            else
            {
                moveTo = Target;
                BoardViewer.Ins.ExecuteAction(this);
            }
        }
        public void Reset()
        {
            formationPos = -1;
            moveTo = -1;
        }
    }
}