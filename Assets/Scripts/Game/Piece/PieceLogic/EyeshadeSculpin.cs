using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class EyeshadeSculpin : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public EyeshadeSculpin(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, AmbushPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Infected(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 4))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this || pOn.Color == Color) continue;
                        list.Add(new EyeshadeSculpinActive(Pos, index));
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        var listPieces = new List<Commons.PieceLogic>(); 
                        foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 4))
                        {
                            var idx = IndexOf(rank, file);
                            var pOn = PieceOn(idx);
                            if (pOn != null && pOn.Color != Color)
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
                            list.Add(new EyeshadeSculpinActive(Pos, listPieces[0].Pos));
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
                    
                        var eyeshadeSculpinActiveAI = new EyeshadeSculpinActiveAI(Pos, selectedPieces[0], selectedPieces[1]);
                        list.Add(eyeshadeSculpinActiveAI);
                    }
                    else
                    {
                        foreach (var (rankOff1, fileOff1) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 4))
                        {
                            var index1 = IndexOf(rankOff1, fileOff1);
                            var pOn1 = PieceOn(index1);
                            if (pOn1 == null || pOn1 == this || pOn1.Color == Color) continue;
                            foreach (var (rankOff2, fileOff2) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 4))
                            {
                                var index2 = IndexOf(rankOff2, fileOff2);
                                var pOn2 = PieceOn(index2);
                                if (pOn2 == null || pOn2 == this || pOn2.Color == Color || pOn2.Pos == pOn1.Pos) continue;
                                list.Add(new EyeshadeSculpinActiveAI(Pos, pOn1, pOn2));
                            }
                        }
                    }
                    
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}