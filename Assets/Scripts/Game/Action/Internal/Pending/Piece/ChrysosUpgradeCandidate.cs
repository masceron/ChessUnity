using System.Collections.Generic;
using Game.Action.Skills;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.ChrysosShop;
using ZLinq;
using Random = UnityEngine.Random;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosUpgradeCandidate : PendingAction, ISkills, IAIAction
    {
        private readonly List<PieceLogic> _allyPieces;
        private readonly Chrysos _chrysos;
        public readonly byte Cost;

        public readonly string CurrentPiece;
        public readonly PieceRank UpgradableTo;
        public readonly PieceRank UpgradeFrom;
        private PieceConfig _config;

        public ChrysosUpgradeCandidate(PieceLogic maker, PieceLogic to, int cost, Chrysos ch) : base(maker, to)
        {
            Cost = (byte)cost;
            _chrysos = ch;

            var cr = GetTarget() as PieceLogic;
            UpgradableTo = Chrysos.UpgradableTo(cr.PieceRank);
            UpgradeFrom = cr.PieceRank;
            CurrentPiece = cr.Type;

            _allyPieces = new List<PieceLogic>();
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
            var hasElite = false;
            var hasCommon = false;
            var hasSwarm = false;
            var hasChampion = false;

            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var p = BoardUtils.PieceOn(i);
                if (p == null || p.Color != ((PieceLogic)GetMaker()).Color) continue;
                _allyPieces.Add(p);
                switch (p.PieceRank)
                {
                    case PieceRank.Champion:
                        hasChampion = true;
                        break;
                    case PieceRank.Elite:
                        hasElite = true;
                        break;
                    case PieceRank.Common:
                        hasCommon = true;
                        break;
                    case PieceRank.Swarm:
                        hasSwarm = true;
                        break;
                    case PieceRank.None:
                    case PieceRank.Construct:
                    case PieceRank.Summoned:
                    case PieceRank.Commander:
                    default:
                        continue;
                }
            }

            if (_allyPieces.Count == 0) return;

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

            if (hasChampion && _chrysos.Coin >= 6) HandleUpgrade(6);
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void CompleteAction()
        {
            var shop = BoardViewer.Ins.GetOrInstantiateUI<ChrysosShop>(IngameSubmenus.ChrysosShop);
            shop.Load(_chrysos, this);
        }

        private void ActivateSkill(PieceLogic p, string type, byte cost)
        {
            CommitResult(new ChrysosUpgrade(GetMaker() as PieceLogic, p, new PieceConfig(type, p.Color, p.Pos), cost));
        }

        private void HandleUpgrade(byte cost)
        {
            _allyPieces.Sort((a, b) =>
                a.GetValueForAI().CompareTo(b.GetValueForAI()));

            var topValue = _allyPieces[0].GetValueForAI();
            var topGroup = _allyPieces.Where(p => p.GetValueForAI() == topValue).ToList();

            var upgradableTo = (from piece in AssetManager.Ins.PieceData.Values
                where piece.rank == UpgradableTo
                select piece.key).ToList();
            if (UpgradeFrom == PieceRank.Champion) upgradableTo.Remove(CurrentPiece);

            var idx = Random.Range(0, upgradableTo.Count);

            if (topGroup.Count == 1)
            {
                ActivateSkill(_allyPieces[0], upgradableTo[idx], cost);
            }
            else
            {
                var p = Random.Range(0, topGroup.Count);
                ActivateSkill(_allyPieces[p], upgradableTo[idx], cost);
            }
        }
    }
}