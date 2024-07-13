using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    [TextArea] [SerializeField] string _eventDescription = "Invoke in:\nListeners are:";
    
    List<GameEventItemListener> _listeners = new List<GameEventItemListener>();

    public virtual void Invoke()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventInvoked();
        }
    }
    
    public virtual void Invoke(int value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventInvoked(value);
    }
    
    public virtual void Invoke(float value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventInvoked(value);
    }

    public virtual void Invoke(Vector2 value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventInvoked(value);
    }

    public void RegisterListener(GameEventItemListener gameEventListener)
    {
        if(!_listeners.Contains(gameEventListener)) _listeners.Add(gameEventListener);
    }

    public void UnregisterListener(GameEventItemListener gameEventListener)
    {
        if(_listeners.Contains(gameEventListener)) _listeners.Remove(gameEventListener);
    }
}