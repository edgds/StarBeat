using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AudioSyncColor : AudioSyncer { //syncs the img to color according to the audio

	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = m_img.color;
		Color _initial = _curr;
		float _timer = 0;
		
		while (_curr != _target)
		{
			_curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			m_img.color = _curr;

			yield return null;
		}

		m_isBeat = false;
	}

	private Color RandomColor() //if no color, set color to white and change to random color
	{
		if (beatColors == null || beatColors.Length == 0) return Color.white;
		m_randomIndx = Random.Range(0, beatColors.Length);
		return beatColors[m_randomIndx];
	}

	public override void OnUpdate() //if on beat, change color (takes into account input in unity such as rest color to return to and time to change color)
	{
		base.OnUpdate();

		if (m_isBeat) return;

		m_img.color = Color.Lerp(m_img.color, restColor, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat() //shifts img to desired color
	{
		base.OnBeat();

		Color _c = RandomColor();

		StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", _c);
	}

	private void Start()
	{
		m_img = GetComponent<Image>();
	}
	//public variables to control color in unity

	public Color[] beatColors;
	public Color restColor;

	private int m_randomIndx;
	private Image m_img;
}
