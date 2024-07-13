using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	public List<GameEventItemListener> Events = new List<GameEventItemListener>();

	public void OnEnable()
	{
		foreach (var item in Events)
		{
			if (item.Event) item.Event.RegisterListener(item);
		}
	}

	public void OnDisable()
	{
		foreach (var item in Events)
		{
			if (item.Event) item.Event.UnregisterListener(item);
		}
	}
}

[Serializable]
public class GameEventItemListener
{
	public GameEvent Event;
	
	public UnityEvent Response = new UnityEvent();
	public FloatEvent ResponseFloat = new FloatEvent();
	public IntEvent ResponseInt = new IntEvent();
	
	public virtual void OnEventInvoked()
	{
		Response.Invoke();
	}
	
	public virtual void OnEventInvoked(float value)
	{
		ResponseFloat.Invoke(value);
	}
	
	public virtual void OnEventInvoked(int value)
	{
		ResponseInt.Invoke(value);
	}
}

[Serializable] public class IntEvent : UnityEvent<int> { }
[Serializable] public class FloatEvent : UnityEvent<float> { }