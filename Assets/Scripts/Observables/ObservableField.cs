using System;

public class ObservableField<T>
{
	private T _value;

	public T Value
	{
		get => _value;
		set
		{
			if (!Equals(_value, value))
			{
				_value = value;
				OnValueChanged?.Invoke(_value);
			}
		}
	}

	public event Action<T> OnValueChanged;

	public ObservableField(T initialValue = default(T))
	{
		_value = initialValue;
	}
}