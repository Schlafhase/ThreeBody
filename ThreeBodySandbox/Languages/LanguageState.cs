namespace ThreeBodySandbox.Languages;

public class LanguageState
{
	private Language _current = new German();

	public Language Current
	{
		get => _current;
		set
		{
			_current = value;
			OnChange?.Invoke();
			OnChangeAsync?.Invoke();
		}
	}

	public event Action? OnChange;
	public event Func<Task> OnChangeAsync;
}