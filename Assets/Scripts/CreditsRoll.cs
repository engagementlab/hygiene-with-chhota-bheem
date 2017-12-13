using System.Net.Configuration;
using System.Timers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

//	public RectTransform Content;

	private ScrollRect _scroll;
	private float scrollDelay = 4;
	private float delayElapsed;
	private bool _isDragging;
	private bool _reverse;
	
	// Use this for initialization
	void Start ()
	{
		
		_scroll = GetComponent<ScrollRect>();
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		// Reset delay if dragged
		if(_isDragging)
		{
			scrollDelay = 2;
			delayElapsed = 0;
			return;
		}

		// If at bottom, move to top in 2s
		if(_scroll.verticalNormalizedPosition <= 0)
			Invoke("MoveToTop", 2);

		// Scroll down
		if(delayElapsed >= scrollDelay && _scroll.verticalNormalizedPosition > 0)
			_scroll.verticalNormalizedPosition -= Time.deltaTime * .07f;
		
		// Scroll back up
		else if(_reverse)
		{
			if(_scroll.verticalNormalizedPosition < 1)
				_scroll.verticalNormalizedPosition += Time.deltaTime;
			else
				_reverse = false;
		}
		else
			delayElapsed += Time.deltaTime;

	}

	void MoveToTop()
	{
			scrollDelay = 4;
			delayElapsed = 0;
			_reverse = true;

	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_isDragging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_isDragging = false;
	}
}
