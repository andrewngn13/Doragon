using UnityEngine;
using UnityEngine.EventSystems;
using Doragon.Logging;

namespace Doragon.Mapping
{
    public class DragNDropIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 position;
        private float timeCount = 0.0f;
        public void OnBeginDrag(PointerEventData eventData)
        {
            position = transform.position;
            DLogger.Log("OnBeginDrag: " + position);
        }

        // Drag the selected item.
        public void OnDrag(PointerEventData data)
        {
            if (data.dragging)
            {
                // Object is being dragged.
                timeCount += Time.deltaTime;
                if (timeCount > 0.25f)
                {
                    DLogger.Log("Dragging:" + data.position);
                    timeCount = 0.0f;
                }
            }
            transform.position = data.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = position;
            DLogger.Log("OnEndDrag: " + position);
            // TODO: call snap to grid, or dispose if not
        }
    }
}