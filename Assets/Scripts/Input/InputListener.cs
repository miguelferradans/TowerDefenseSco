using TD.Logic;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.InputHandling
{
    public class InputListener
    {
        public InputListener(LevelComponents levelComponents)
        {
            m_levelComponents = levelComponents;
            m_inputHandler = new InputHandler();

            m_inputHandler.OnInputBegan += OnInputTap;
            m_inputHandler.OnInputDragged += OnInputDragged;
            m_inputHandler.OnInputEnd += OnInputRelease;
            m_inputHandler.OnInputScrolled += OnInputScrolled;
        }

        public void Update()
        {
            m_inputHandler.Update();
        }

        private void OnInputTap(Vector3 position)
        {

        }

        private void OnInputDragged(Vector3 startPosition, Vector3 endPosition)
        {
            var delta2D = (endPosition - startPosition) * Time.deltaTime * 1.5f;

            Camera.main.transform.localPosition -= delta2D;
        }

        private void OnInputRelease(Vector3 position, bool wasDragging)
        {
            if (!wasDragging)
            {
                RuntimeSlot selectedSlot = m_levelComponents.GetSlotInScreenPosition(position);

                if (null != selectedSlot)
                {
                    m_levelComponents.State.SelectedSlot = selectedSlot;
                    m_levelComponents.Events.Slot.OnSelectedSlot(selectedSlot);
                }
                else
                {
                    m_levelComponents.State.SelectedSlot = null;
                }
            }
        }

        private void OnInputScrolled(float delta)
        {
            Camera.main.orthographicSize -= delta * 0.2f;

            if (Camera.main.orthographicSize < 4.5f)
            {
                Camera.main.orthographicSize = 4.5f;
            }
            else if (Camera.main.orthographicSize > 8f)
            {
                Camera.main.orthographicSize = 8f;
            }
        }

        private LevelComponents m_levelComponents = default;
        private InputHandler m_inputHandler = default;
    }
}