using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class DirectionalSelector<TObject> : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] protected Button previousButton;
	[SerializeField] protected Button nextButton;
	[SerializeField] protected TextMeshProUGUI selectedText;

	[Header("Options"), Space]
	[SerializeField] protected TObject[] options;
	[SerializeField] protected int defaultOptionIndex;
	
	[Header("On Value Changed Event"), Space]
	public UnityEvent<int> onIndexChanged;
	public UnityEvent<TObject> onValueChanged;

	public int Index
	{
		get { return _currentIndex; }
		set { SetIndex(value, notify: true); }
	}

	public TObject Value
	{
		get { return _selected; }
		set { SetValue(value, notify: true); }
	}

	// Protected fields.
	protected TObject _selected;
	protected int _currentIndex;

	// Private fields.
	private TweenPool _tweenPool = new TweenPool();

	private void Start()
	{
		previousButton.onClick.AddListener(PreviousOption);
		nextButton.onClick.AddListener(NextOption);
	}

	public void SetIndexWithoutNotify(int index)
	{
		SetIndex(index, false);
	}
	
	public void SetValueWithoutNotify(TObject value)
	{
		SetValue(value, false);
	}

	private void PreviousOption()
	{
		int len = options.Length;
		_currentIndex = (--_currentIndex % len + len) % len;
		ReloadUI();	
	}

	private void NextOption()
	{
		_currentIndex = ++_currentIndex % options.Length;
		ReloadUI();
	}

	private void ReloadUI(bool notify = true)
	{
		_tweenPool.KillActiveTweens(false);

		_selected = options[_currentIndex];
		SetDisplayText();

		_tweenPool.Add(selectedText.transform.DOScale(1f, .2f)
							  				 .From(1.2f)
							  				 .SetEase(Ease.OutCubic));

		if (notify)
		{
			onIndexChanged?.Invoke(_currentIndex);
			onValueChanged?.Invoke(_selected);
		}
	}

	protected abstract void SetDisplayText();

	private void SetIndex(int index, bool notify)
	{
		try
		{
			_currentIndex = index;
			ReloadUI(notify);
		}
		catch (IndexOutOfRangeException)
		{
			_currentIndex = defaultOptionIndex;
			ReloadUI(notify);
		}
	}

	private void SetValue(TObject value, bool notify)
	{
		int index = Array.IndexOf(options, value);
		
		if (index != -1)
		{
			_currentIndex = index;
			ReloadUI(notify);
		}
		else
		{
			_currentIndex = defaultOptionIndex;
			ReloadUI(notify);
		}
	}
}