using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;    
using Game.Action.Skills;
using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Internal.Pending;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Humilitas : Commons.PieceLogic, IPieceWithSkill
    {
        private int deathDefianceCount;
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            deathDefianceCount = 4;
            // ActionManager.ExecuteImmediately(new ApplyEffect(new PureMinded(this)));
            // ActionManager.ExecuteImmediately(new ApplyEffect(new Relentless(this, deathDefianceCount)));
            // ActionManager.ExecuteImmediately(new ApplyEffect(new DeathDefiance(this, deathDefianceCount)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != Color)
                        {
                            list.Add(new HumilitasPending(Pos, idx));
                        }
                    }
                }
                else
                {
                    if (!excludeEmptyTile)
                    {
                        foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                        {
                            var idx = IndexOf(rank, file);
                            var pOn = PieceOn(idx);
                            if (pOn != null && pOn.Color != Color)
                            {
                                list.Add(new HumilitasPending(Pos, idx));
                            }
                        }
                        return;
                    }

                    var listPieces = new List<Commons.PieceLogic>();

                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != PieceOn(Pos).Color)
                        {
                            if (pOn.Effects != null && pOn.Effects.Any(e => e.EffectName == "effect_extremophile")) continue;
                            listPieces.Add(pOn);
                        }
                    }
                    // neu khong co quan nao
                    if (listPieces.Count == 0) return;
                    // neu co dung mot quan
                    if (listPieces.Count == 1)
                    {
                        return;
                    }
                    // neu co nhieu quan           
                    listPieces.Sort((a, b) =>
                        b.GetValueForAI()
                            .CompareTo(a.GetValueForAI()));

                    var selectedPieces = new List<Commons.PieceLogic>();

                    int topValue = listPieces[0].GetValueForAI();
                    var topGroup = listPieces.Where(p =>
                        p.GetValueForAI() == topValue).ToList();

                    if (topGroup.Count >= 2)
                    {
                        int idx1 = UnityEngine.Random.Range(0, topGroup.Count);
                        int idx2;
                        do { idx2 = UnityEngine.Random.Range(0, topGroup.Count); }
                        while (idx2 == idx1);

                        selectedPieces.Add(topGroup[idx1]);
                        selectedPieces.Add(topGroup[idx2]);
                    }
                    else
                    {
                        selectedPieces.Add(listPieces[0]);

                        if (listPieces.Count > 1)
                        {
                            int secondValue = listPieces[1].GetValueForAI();
                            var secondGroup = listPieces.Where(p =>
                                p.GetValueForAI() == secondValue).ToList();
                            if (secondGroup.Count == 0) return;
                            int idx = UnityEngine.Random.Range(0, secondGroup.Count);
                            selectedPieces.Add(secondGroup[idx]);
                        }
                    }
                    if (selectedPieces.Count < 2) return;

                    var action = new HumilitasPending(Pos, selectedPieces[0].Pos);
                    HumilitasPending.SecondTarget = selectedPieces[1].Pos;
                    list.Add(action);
                }
            };
        }


        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}