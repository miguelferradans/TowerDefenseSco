using System;
using System.Collections.Generic;
using TD.Data.Entities;
using TD.Data.Levels;
using UnityEditor;
using UnityEngine;

namespace TD.Editor
{
    public class LevelEditor : EditorWindow
    {
        [MenuItem("TD/Level Editor")]
        public static void OpenWindow()
        {
            Type dockType = Type.GetType("UnityEditor.SceneView, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            LevelEditor window = GetWindow<LevelEditor>("Level Editor", true, dockType);
            window.titleContent = new GUIContent("Level Editor", EditorGUIUtility.IconContent("Animation.Record").image);
            window.Show();
        }

        private void OnEnable()
        {
            SetData();
        }

        private void OnSelectionChange()
        {
            SetData();
        }

        private void SetData()
        {
            if (Selection.activeObject is Level data)
            {
                m_data = data;
                m_selectedSlots = new List<Slot>() { m_data.Slots[Vector2Int.zero] };
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (EarlyExitIfNoData())
            {
                return;
            }

            using (var scope = new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
            using (var scroll = new EditorGUILayout.ScrollViewScope(m_scrollPosition))
            {
                m_scrollPosition = scroll.scrollPosition;

                ResetData();

                DrawSize();

                EditorGUILayout.LabelField("Level Grid", EditorStyles.centeredGreyMiniLabel);

                DrawGrid();

                AdvanceAutomaticLayout();

                AddEnemyPath();

                DrawSelectedSlotProperties();
            }

        }

        private void ResetData()
        {
            if (GUILayout.Button("Reset"))
            {
                if (EditorUtility.DisplayDialog("Reset All Data", "This will be erase your current config. Are you sure?", "Go Ahead!"))
                {
                    m_data.Reset();
                    ApplyChanges();
                }
            }
        }

        private void AddEnemyPath()
        {
            using (new EditorGUI.DisabledGroupScope(!IsValidEnemyPath()))
            {
                if (GUILayout.Button("Create Enemy Path"))
                {
                    List<Vector2Int> path = new List<Vector2Int>();

                    foreach (var slot in m_selectedSlots)
                    {
                        path.Add(slot.Coords);
                    }

                    m_data.EnemyPaths.Add(new EnemyPathList(path));

                    ApplyChanges();
                }
            }
        }

        private void DrawSelectedSlotProperties()
        {
            if (m_selectedSlots.Count > 0)
            {
                EditorGUILayout.LabelField("Slot Properties", EditorStyles.centeredGreyMiniLabel);
                EditorGUILayout.LabelField("Coordinates: " + m_selectedSlots[0].Coords);

                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    m_selectedSlots[0].CanConstruct = EditorGUILayout.Toggle("Can Construct", m_selectedSlots[0].CanConstruct);
                    if (change.changed)
                    {
                        for (int i = 1; i < m_selectedSlots.Count; i++)
                        {
                            m_selectedSlots[i].CanConstruct = m_selectedSlots[0].CanConstruct;
                        }

                        ApplyChanges();
                    }
                }

                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    m_selectedSlots[0].CanGenerateEnemies = EditorGUILayout.Toggle("Can Generate Enemies", m_selectedSlots[0].CanGenerateEnemies);

                    if (change.changed)
                    {
                        for (int i = 1; i < m_selectedSlots.Count; i++)
                        {
                            m_selectedSlots[i].CanGenerateEnemies = m_selectedSlots[0].CanGenerateEnemies;
                        }

                        ApplyChanges();
                    }
                }

                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    m_selectedSlots[0].Base = EditorGUILayout.ObjectField("Base: ", m_selectedSlots[0].Base, typeof(Base), false) as Base;

                    if (change.changed)
                    {
                        for (int i = 1; i < m_selectedSlots.Count; i++)
                        {
                            m_selectedSlots[i].Base = m_selectedSlots[0].Base;
                        }

                        ApplyChanges();
                    }
                }
            }
        }

        private void DrawGrid()
        {
            var rect = EditorGUILayout.GetControlRect();

            foreach (var slot in m_data.Slots)
            {
                var coords = slot.Coords;

                Vector2 position = GetCellPosition(rect, coords.x, coords.y);
                Rect cellRect = new Rect(position.x, position.y, m_cellSize, m_cellSize);

                Color color = (m_selectedSlots.Contains(slot)) ? m_selectedColor : m_unselectedColor;

                EditorGUI.DrawRect(cellRect, color);

                if (slot.CanConstruct)
                {
                    GUI.Label(new Rect(cellRect.x + cellRect.width * 0.5f - 2, cellRect.y - 2, 12, 12), "C");
                }

                if (slot.CanGenerateEnemies)
                {
                    GUI.Label(new Rect(cellRect.x + cellRect.width * 0.5f - 2, cellRect.y - 2, 12, 12), "G");
                }

                if (null != slot.Base)
                {
                    GUI.Label(new Rect(cellRect.x + cellRect.width * 0.5f - 2, cellRect.y - 2, 12, 12), "B");
                }

                if (BelongsToEnemyPath(slot.Coords))
                {
                    GUI.Label(new Rect(cellRect.x + cellRect.width * 0.5f - 2, cellRect.y + 8, 12, 12), "P");
                }

                if (Event.current.type == EventType.MouseUp && cellRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.control)
                    {
                        m_selectedSlots.Add(slot);
                    }
                    else
                    {
                        SelectSingleSlot(slot);
                    }
                    Repaint();
                }
            }
        }

        private void AdvanceAutomaticLayout()
        {
            // Fooling Unity into thinking we are using the automatic layout so it handles the scroll for us
            float gridHeight = GetGridHeight(m_data.Slots.Height);
            EditorGUILayout.GetControlRect(false, gridHeight + m_cellMargin);
        }

        private bool EarlyExitIfNoData()
        {
            if (null == m_data)
            {
                EditorGUILayout.LabelField("Select a Level to start", EditorStyles.centeredGreyMiniLabel);
                return true;
            }

            return false;
        }

        private void DrawSize()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                var newWidth = EditorGUILayout.IntField("Width", (int)m_data.Slots.Width);
                var newHeight = EditorGUILayout.IntField("Height", (int)m_data.Slots.Height);

                if (change.changed && newWidth > 0 && newHeight > 0)
                {
                    m_data.ResizeGrid(newWidth, newHeight);
                    ApplyChanges();
                }
            }
        }

        private void ApplyChanges()
        {
            EditorUtility.SetDirty(m_data);
            Repaint();
        }

        private Vector2 GetCellPosition(Rect rect, int x, int y)
        {
            uint width = m_data.Slots.Width;
            uint height = m_data.Slots.Height;

            float gridWidth = GetGridWidth(width);
            float gridHeight = GetGridHeight(height);

            Vector2 position;
            position.x = x * (m_cellSize + m_cellMargin) + Mathf.Max(m_padding, rect.width / 2 - gridWidth / 2);

            float center_y = rect.height / 2 - gridHeight / 2;

            position.y = rect.y + (-m_cellSize + (height - y) * (m_cellSize + m_cellMargin) + Mathf.Max(m_padding, center_y));

            return position;
        }

        private float GetGridWidth(uint width)
        {
            return width * (m_cellSize + m_cellMargin);
        }

        private float GetGridHeight(uint height)
        {
            return height * (m_cellSize + m_cellMargin);
        }

        private void SelectSingleSlot(Slot slot)
        {
            m_selectedSlots.Clear();
            m_selectedSlots.Add(slot);
        }

        private bool IsValidEnemyPath()
        {
            bool result = m_selectedSlots.Count > 1 && m_selectedSlots[0].CanGenerateEnemies;

            if (result)
            {
                for (int i = 1; i < m_selectedSlots.Count; i++)
                {
                    var currentSlot = m_selectedSlots[i];
                    var previousSlot = m_selectedSlots[i - 1];

                    if (Distance(currentSlot, previousSlot) > 1)
                    {
                        result = false;
                        break;
                    }
                }

                result &= null != m_selectedSlots[m_selectedSlots.Count - 1].Base;
            }

            return result;
        }

        private int Distance(Slot slot1, Slot slot2)
        {
            return Mathf.Abs((slot1.Coords.x - slot2.Coords.x)) +
                       Mathf.Abs((slot1.Coords.y - slot2.Coords.y));
        }

        private bool BelongsToEnemyPath(Vector2Int coords)
        {
            foreach (var path in m_data.EnemyPaths)
            {
                foreach (var slot in path.Path)
                {
                    if (coords == slot)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Level m_data;
        private List<Slot> m_selectedSlots;

        private Vector2 m_scrollPosition;

        private const float m_cellSize = 20f;
        private const float m_cellMargin = 5f;
        private const float m_padding = 5f;

        private Color m_unselectedColor = new Color(0.4f, 0.4f, 0.4f);
        private Color m_selectedColor = new Color(0.4f, 1f, 0.4f);
    }
}