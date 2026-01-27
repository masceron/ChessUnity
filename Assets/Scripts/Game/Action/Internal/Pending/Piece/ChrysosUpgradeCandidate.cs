using System.Collections.Generic;
using System.Linq;
using Game.Action.Skills;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ChrysosShop;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosUpgradeCandidate: PendingAction, IInternal, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        private PieceConfig config;

        public readonly string CurrentPiece;
        public readonly PieceRank UpgradeFrom;
        public readonly PieceRank UpgradableTo;
        public readonly byte Cost;
        private Chrysos _chrysos;
        private List<PieceLogic> allyPieces;
        
        public ChrysosUpgradeCandidate(int maker, int to, int cost, Chrysos ch) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            Cost = (byte)cost;
            _chrysos = ch;
            
            var cr = BoardUtils.PieceOn(to);
            UpgradableTo = Chrysos.UpgradableTo(cr.PieceRank);
            UpgradeFrom = cr.PieceRank;
            CurrentPiece = cr.Type;

            allyPieces = new List<PieceLogic>();
        }

        public override void CompleteAction()
        {
            var shop = Object.FindAnyObjectByType<ChrysosShop>(FindObjectsInactive.Include);
            if (!shop)
            {
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                shop = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.ChrysosShop), canvas.transform).GetComponent<ChrysosShop>();
            }
            else shop.gameObject.SetActive(true);

            shop.Load((Chrysos)BoardUtils.PieceOn(Maker), this);
        }

        public void ActivateSkill(PieceLogic p, string type, byte cost)
        {
            MatchManager.Ins.InputProcessor.ExecuteAction(new ChrysosUpgrade(Maker, new PieceConfig(type, p.Color, p.Pos), cost));
        }
        public void CompleteActionForAI()
        {
            //Implement for AI automatically
            bool hasElite = false;
            bool hasCommon = false;
            bool hasSwarm = false;
            bool hasChampion = false;

            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var p = BoardUtils.PieceOn(i);
                if (p == null || p.Color != BoardUtils.PieceOn(Maker).Color) continue;
                allyPieces.Add(p);
                if (p.PieceRank == PieceRank.Champion) hasChampion = true;
                if (p.PieceRank == PieceRank.Elite) hasElite = true;
                if (p.PieceRank == PieceRank.Common) hasCommon = true;
                if (p.PieceRank == PieceRank.Swarm) hasSwarm = true;
            }

            if (allyPieces.Count == 0) return;
            
            if (hasElite && _chrysos.Coin >= 5)
            {
                HandleUpgrade(5);
                return;
            }

            if (hasCommon && _chrysos.Coin >= 3)
            {
                HandleUpgrade(3);
                return;
            }

            if (hasSwarm && _chrysos.Coin >= 1)
            {
                HandleUpgrade(1);
                return;
            }

            if (hasChampion && _chrysos.Coin >= 6)
            {
                HandleUpgrade(6);
            }
        }

        private void HandleUpgrade(byte cost)
        {
            allyPieces.Sort((a, b) => 
                a.GetValueForAI().CompareTo(b.GetValueForAI()));
                
            int topValue = allyPieces[0].GetValueForAI();
            var topGroup = allyPieces.Where(p => p.GetValueForAI() == topValue).ToList();

            var upgradableTo = (from piece in AssetManager.Ins.PieceData.Values 
                where piece.rank == UpgradableTo select piece.key).ToList();
            if (UpgradeFrom == PieceRank.Champion) upgradableTo.Remove(CurrentPiece);
                
            int idx = Random.Range(0, upgradableTo.Count);
                
            if (topGroup.Count == 1)
            {
                ActivateSkill(allyPieces[0], upgradableTo[idx], cost);
            }
            else
            {
                var p = UnityEngine.Random.Range(0, topGroup.Count);
                ActivateSkill(allyPieces[p], upgradableTo[idx], cost);
            }
        }
    }
}