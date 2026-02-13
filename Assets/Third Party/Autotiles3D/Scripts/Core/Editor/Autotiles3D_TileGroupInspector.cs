using UnityEditor;
using UnityEngine;

namespace Third_Party.Autotiles3D.Scripts.Core.Editor
{
    [CustomEditor(typeof(Autotiles3D_TileGroup))]
    public class Autotiles3D_TileGroupInspector : UnityEditor.Editor
    {
        public static Autotiles3D_Tile CopyBuffer;
        private Autotiles3D_TileGroup _group;

        private void OnEnable()
        {
            _group = (Autotiles3D_TileGroup)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var upArrow = '\u25B2';
            var downArrow = '\u25BC';

            EditorGUI.BeginChangeCheck();

            foreach (var tile in _group.Tiles.ToArray())
            {
                tile.SetGroupName(_group.name);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                tile.RenderTileGUI(out var dirty, _group);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(upArrow.ToString(), GUILayout.Width(24)))
                {
                    var index = _group.Tiles.IndexOf(tile);
                    if (index > 0)
                    {
                        Undo.RegisterCompleteObjectUndo(_group, "Tile Move");
                        _group.Tiles.Remove(tile);
                        _group.Tiles.Insert(index - 1, tile);
                    }
                }

                if (GUILayout.Button(downArrow.ToString(), GUILayout.Width(24)))
                {
                    var index = _group.Tiles.IndexOf(tile);
                    if (index < _group.Tiles.Count - 1)
                    {
                        Undo.RegisterCompleteObjectUndo(_group, "Tile Move");
                        _group.Tiles.Remove(tile);
                        _group.Tiles.Insert(index + 1, tile);
                    }
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Copy"))
                {
                    Debug.Log($"Copying tile {tile.Name}");
                    CopyBuffer = tile;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add new tile"))
            {
                Undo.RegisterCompleteObjectUndo(_group, "Tile Add");
                _group.Tiles.Add(new Autotiles3D_Tile("New Tile"));
            }

            if (GUILayout.Button("Remove last tile"))
                if (_group.Tiles.Count > 0)
                {
                    Undo.RegisterCompleteObjectUndo(_group, "Tile Remove");
                    _group.Tiles.RemoveAt(_group.Tiles.Count - 1);
                }

            if (CopyBuffer != null)
                if (GUILayout.Button($"Paste copied tile ({CopyBuffer.Name})"))
                {
                    Debug.Log("Pasting copied tile!");
                    _group.Tiles.Add(new Autotiles3D_Tile(CopyBuffer));
                }

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck() || GUI.changed)
            {
                //fix: mapbuilding in deserialization callback is not always called when inspecting.
                _group.ConstructMapping();
                EditorUtility.SetDirty(_group);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}