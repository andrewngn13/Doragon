using UnityEngine;
using UnityEngine.EventSystems;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.Mapping
{
    public class InputPointTracker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform Canvas;
        public GameObject Vert, Horiz;
        private Vector3 curPoint = Vector3.down;
        private float gridWidth, gridHeight;
        private const int gridPoint = 100, pointRadius = 25;

        private void Start()
        {
            RectTransform rect = GetComponent<RectTransform>();
            gridWidth = rect.rect.width;
            gridHeight = rect.rect.height;
        }

        /// <summary>
        /// Draws a line between valid intersections
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private void SetDrawPoints(Vector3 point)
        {
            Vector3 normalized = (point - transform.position) / Canvas.localScale.x;
            Vector3 roundedPoint = new Vector3(((int)normalized.x + gridPoint / 2) / gridPoint * gridPoint,
                ((int)normalized.y - gridPoint / 2) / gridPoint * gridPoint);
            // return if off the grid
            if (normalized.x > gridWidth || -normalized.y > gridHeight) return;
            // return if point isnt on a gridpoint within pointRadius detection 
            if (normalized.x - roundedPoint.x > pointRadius || normalized.x - roundedPoint.x < -pointRadius ||
                normalized.y - roundedPoint.y > pointRadius || normalized.y - roundedPoint.y < -pointRadius)
                return;
            if (curPoint == Vector3.down)
            {
                DLogger.Log(ZString.Format("start point set: {0}", roundedPoint));
                curPoint = roundedPoint;
                return;
            }
            if (roundedPoint == curPoint) return;
            // TODO: Do not spawn if line exists
            // determine relative intersection and spawn line prefab
            bool horiz = roundedPoint.x != curPoint.x;
            // return if not adjacent points
            if (horiz && roundedPoint.y != curPoint.y)
            {
                curPoint = roundedPoint;
                return;
            }
            Transform newGridLine = Instantiate(horiz ? Horiz : Vert, transform, true).transform;
            newGridLine.localScale = Vector3.one;
            if (horiz)
                newGridLine.localPosition = roundedPoint + (roundedPoint.x > curPoint.x ? new Vector3(-50, 0) : new Vector3(50, 0));
            else
                newGridLine.localPosition = roundedPoint + (roundedPoint.y > curPoint.y ? new Vector3(0, -50) : new Vector3(0, 50));

            curPoint = roundedPoint;
            DLogger.Log("Line Spawned");
        }

        public void OnBeginDrag(PointerEventData data)
        {
            DLogger.Log(ZString.Format("OnBeginDrag: {0}", data.position));
            SetDrawPoints(data.position);
        }

        // Drag the selected item.
        public void OnDrag(PointerEventData data)
        {
            if (data.dragging)
                SetDrawPoints(data.position);
        }

        public void OnEndDrag(PointerEventData data)
        {
            DLogger.Log(ZString.Format("OnEndDrag: {0}", data.position));
            curPoint = Vector3.down;
        }
    }
}