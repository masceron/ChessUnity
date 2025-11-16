using System;
using System.Reflection;
using Game.Managers;
using PrimeTween;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece
{
    public enum PieceRank : byte
    {
        None,
        Construct,
        Summoned,
        Swarm,
        Common,
        Elite,
        Champion,
        Commander
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piece : MonoBehaviour
    {
        private int rank;
        private int file;
        private bool color;
        
        public void Spawn(int pos, bool c)
        {
            rank = RankOf(pos);
            file = FileOf(pos);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            transform.position = new Vector3(rank, YCoordinate, file);
            
            var angles = new Quaternion
            {
                eulerAngles = !c
                    ? new Vector3(-90, 0, 0)
                    : new Vector3(-90, 0, 180)
            };

            transform.rotation = angles;
        }
        
        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;
            
            Tween.Position(transform, new Vector3(rank, transform.position.y, file), 0.2f);
        }
    }

    public static class PieceMaker
    {
        private static readonly Assembly GameAssembly = Assembly.GetExecutingAssembly();

        public static PieceLogic.Commons.PieceLogic Get(PieceConfig config)
        {
            var classname = AssetManager.Ins.PieceData[config.Type];
            var pieceType = GameAssembly.GetType($"Game.Piece.PieceLogic.{classname.logicClassName}");
        
            if (pieceType == null)
            {
                Debug.LogError($"Could not find logic class with key {config.Type} and name: {classname.logicClassName}");
                return null;
            }

            try
            {
                var instance = Activator.CreateInstance(pieceType, args:config) as PieceLogic.Commons.PieceLogic;
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create instance of {classname.logicClassName}: {e.Message}");
                return null;
            }
        }
    }
}
