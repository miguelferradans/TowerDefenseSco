using UnityEngine;

namespace TD.InputHandling
{
    public class InputHandler
    {
        public delegate void InputBegan(Vector3 position);
        public event InputBegan OnInputBegan;

        public delegate void InputEnd(Vector3 position, bool wasDragging);
        public event InputEnd OnInputEnd;

        public delegate void InputDragged(Vector3 startPosition, Vector3 endPosition);
        public event InputDragged OnInputDragged;

        public delegate void InputScrolled(float delta);
        public event InputScrolled OnInputScrolled;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_lastPosition = Input.mousePosition;
                OnInputBeganEvent(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && !Mathf.Approximately(0f, (m_lastPosition - Input.mousePosition).magnitude))
            {
                OnInputDraggedEvent(m_lastPosition, Input.mousePosition);
                m_lastPosition = Input.mousePosition;
                m_isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnInputEndEvent(Input.mousePosition, m_isDragging);
                m_isDragging = false;
            }

            if (!Mathf.Approximately(0f, Input.mouseScrollDelta.y))
            {
                OnInputScrolledEvent(Input.mouseScrollDelta.y);
            }
        }

        private void OnInputBeganEvent(Vector3 position)
        {
            OnInputBegan?.Invoke(position);
        }

        private void OnInputEndEvent(Vector3 position, bool wasDragging)
        {
            OnInputEnd?.Invoke(position, wasDragging);
        }

        private void OnInputDraggedEvent(Vector3 startPosition, Vector3 endPosition)
        {
            OnInputDragged?.Invoke(startPosition, endPosition);
        }

        private void OnInputScrolledEvent(float delta)
        {
            OnInputScrolled?.Invoke(delta);
        }

        private Vector3 m_lastPosition = default;
        private bool m_isDragging = false;
    }
}