using System;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
// using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class BlueChromisPassive : Effect, IDeadTrigger
    {
        public BlueChromisPassive(PieceLogic piece) : base(-1, 1, piece, "effect_blue_chromis_passive")
        {

        }
        public void OnCallDead(PieceLogic pieceToDie)
        {
            int pos = pieceToDie.Pos;
            var dict = AssetManager.Ins.FormationData;
            var rand = new Random();
            int index = rand.Next(dict.Count);
            var pair = dict.ElementAt(index);
            Formation formation = FormationFactory.CreateInstance(pair.Key, this.Color, -1);
            FormationManager.Ins.SetFormation(pos, formation);
        }
    }
}