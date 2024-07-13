using UnityEngine;
using UnityEngine.EventSystems;

public class DragInputPanel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    bool _interactable;

    [SerializeField] GameEvent OnInputPositionChanged;
    [SerializeField] GameEvent OnShootingBall;

    void Start()
    {
        OnInputPositionChanged?.Invoke(Vector2.zero);
    }

    public void OnPointerDown (PointerEventData data)
    {
        _interactable = true;

        Vector2 pointerPos = Camera.main.ScreenToWorldPoint(data.position);
        OnInputPositionChanged?.Invoke(pointerPos);
    }

    public void OnDrag (PointerEventData data) 
    {
        if(!_interactable)
            return;
        
        Vector2 pointerPos = Camera.main.ScreenToWorldPoint(data.position);
        OnInputPositionChanged?.Invoke(pointerPos);
    }

    public void OnPointerUp (PointerEventData data) 
    {
        if(!_interactable)
            return;

        _interactable = false;
        OnShootingBall?.Invoke();
    }
}
