#if UNITY_EDITOR
using System.Collections.Generic;
using Third_Party.Autotiles3D.Scripts.Core;
using UnityEditor;
using UnityEngine;
using ZLinq;

namespace Third_Party.Autotiles3D.Scripts.Utility
{
    [InitializeOnLoad]
    public static class Autotiles3DUtility
    {
        private static readonly Dictionary<int, Autotiles3D_Tile> _cache = new();
        private static readonly Dictionary<string, Autotiles3D_Tile> _tagCache = new();
        private static readonly Dictionary<string, Autotiles3D_TileGroup> _groupCache = new();

        private static Autotiles3D_TileGroup[] _gCache;

        static Autotiles3DUtility()
        {
            UpdateAllTileGroupNames();
            if (Autotiles3DSettings.EditorInstance.UseUndoAPI)
                ClearUndoStack();
        }

        private static Autotiles3D_TileGroup[] Groups
        {
            get
            {
                _gCache ??= Resources.LoadAll<Autotiles3D_TileGroup>("");
                return _gCache;
            }
        }

        public static GUIStyle RichStyle
        {
            get
            {
                var style = new GUIStyle(GUI.skin.label)
                {
                    wordWrap = true,
                    richText = true,
                    normal =
                    {
                        textColor = Color.white
                    },
                    alignment = TextAnchor.MiddleLeft
                };
                return style;
            }
        }

        public static Autotiles3D_Tile GetTile(int tileID)
        {
            //try id cache
            if (_cache.TryGetValue(tileID, out var tile))
            {
                if (tile != null)
                    return tile;
                _cache.Remove(tileID);
            }

            foreach (var g in Groups)
            {
                tile = g.GetTile(tileID);
                if (tile != null)
                {
                    _cache.Add(tileID, tile);
                    return tile;
                }
            }

            return null;
        }

        public static Autotiles3D_Tile GetTile(int tileID, string name, string group)
        {
            //try id cache
            if (_cache.TryGetValue(tileID, out var tile))
            {
                if (tile != null)
                    return tile;
                _cache.Remove(tileID);
            }

            var tag = group + name;
            //try tag cache
            if (_tagCache.TryGetValue(tag, out tile))
            {
                if (tile != null)
                    return tile;
                _tagCache.Remove(tag);
            }

            //get group
            var tileGroup = GetGroup(group);

            if (tileGroup != null)
            {
                //try id first
                tile = tileGroup.GetTile(tileID);
                if (tile != null)
                {
                    _cache.Add(tileID, tile);
                    return tile;
                }

                //try tag next
                tile = tileGroup.GetTile(name);
                if (tile != null)
                {
                    _tagCache.Add(tag, tile);
                    return tile;
                }
            }

            //try looking just via ID one more time
            if (tileID != -1) return GetTile(tileID);

            return null;
        }

        private static Autotiles3D_TileGroup GetGroup(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (_groupCache.TryGetValue(name, out var group))
            {
                if (group != null)
                    return group;
                _groupCache.Remove(name);
            }

            foreach (var g in Groups)
            {
                if (g == null)
                    continue;
                if (g.name == name)
                {
                    _groupCache.Add(g.name, g);
                    return g;
                }
            }

            return null;
        }

        public static void ClearCache(int tileID)
        {
            if (_cache.Remove(tileID, out var tile)) _tagCache.Remove(tile.Tag);
            //clear group cache for good measure
            _gCache = null;
        }

        private static void UpdateAllTileGroupNames()
        {
            var groups = Resources.LoadAll<Autotiles3D_TileGroup>("");
            foreach (var group in groups) group.UpdateTilesWithGroupName();
        }

        private static void ClearUndoStack()
        {
            Undo.ClearAll();
        }

        public static List<Autotiles3D_TileGroup> LoadTileGroups()
        {
            EnsureFolders();
            return Resources.LoadAll<Autotiles3D_TileGroup>("").ToList();
        }

        public static Autotiles3D_TileGroup GetTileGroup(string groupName)
        {
            //todo
            return null;
        }

        public static void RepairBlocks(Autotiles3D_BlockBehaviour block, List<Autotiles3D_BlockBehaviour> siblings,
            string tilegroup, string tilename, int tileID = -1)
        {
            if (!block || !block.Anchor)
                return;


            var tile = GetTile(tileID, tilename, tilegroup);
            if (tile != null)
            {
                var missingTileId = block.TileID;

                //group name might be not set on the tile, so make sure to set again!
                tile.SetGroupName(tilegroup);
                //find all blocks with same old tile info with missing links in the scene and update them as well
                var brokenBlocks = siblings.Where(b => b.TileID == missingTileId && b.GetTile() == null).ToList();

                int i;
                var successes = 0;
                for (i = 0; i < brokenBlocks.Count(); i++)
                {
                    brokenBlocks[i].UpdateTileInfo(tile);
                    var node = brokenBlocks[i].GetInternalNode();
                    if (node != null)
                    {
                        node.UpdateTileInfo(tile.TileID, tile.Name, tile.Group);
                        node.ResetInstance(); //this will also update the block witht the new tile info
                        successes++;
                    }
                    else
                    {
                        Debug.LogError("Missing internal node");
                    }
                }

                Debug.Log($"Autotiles3D: Successfully linked {successes} block(s) with tile {tilename} again");
            }
            else
            {
                Debug.Log($"No tile with name {tilename} on group {tilegroup} existing!");
            }
        }

        public static void EnsureFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Third Party/Autotiles3D"))
                AssetDatabase.CreateFolder("Assets/Third Party", "Autotiles3D");

            if (!AssetDatabase.IsValidFolder("Assets/Third Party/Autotiles3D/Resources"))
                AssetDatabase.CreateFolder("Assets/Third Party/Autotiles3D", "Resources");
        }

        public static bool DoesTileExist(int tileId, string name, string group)
        {
            return GetTile(tileId, name, group) != null;
        }
    }
}


#endif