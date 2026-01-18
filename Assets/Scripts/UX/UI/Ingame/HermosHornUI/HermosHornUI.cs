using Game.Managers;
using Game.Action.Skills;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using PrimeTween;
using TMPro;
using UnityEngine;
using Game.Action;
using Game.Action.Internal.Pending.Relic;
using Game.Relics.Commons;
using Game.Common;

namespace UX.UI.Ingame.HermosHorn
{
    public class HermosHornUI : Singleton<HermosHornUI>
    {
        private RelicLogic relic; 
        public void Load(RelicLogic relicLogic)
        {
            this.relic = relicLogic;
        }
        public void FirstOption()
        {
            BoardViewer.Ins.ExecuteAction(new HermosHornActive(relic, true));
        }
        public void SecondOption()
        {
            BoardViewer.Ins.ExecuteAction(new HermosHornActive(relic, false));
        }
    }
}