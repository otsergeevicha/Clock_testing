using UnityEngine;
using UnityEngine.EventSystems;
using View;

namespace SetTime
{
    public enum TypeArrow
    {
        Minute,
        Hour
    }
    public class DragHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private ClockView _clockView;
        [SerializeField] private TypeArrow _indexArrow;
        [SerializeField] private ButtonEditor _buttonEditor;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_buttonEditor.IsEdit) 
                _clockView.TimeModule.Stop();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_buttonEditor.IsEdit) 
                return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_clockView.transform,
                eventData.position, _camera, out Vector2 localPoint);
        
            _clockView.TimeModule.OnDragArrow(localPoint, _indexArrow);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_buttonEditor.IsEdit) 
                _clockView.TimeModule.Launch();
        }
    }
}