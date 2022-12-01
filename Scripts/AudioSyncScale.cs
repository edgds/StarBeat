using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer { //syncs the img to scale according to the audio

	private IEnumerator MoveToScale(Vector3 _target) //adjusts x, y, and z to scale
	{
		Vector3 _curr = transform.localScale;
		Vector3 _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Vector3.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			transform.localScale = _curr;

			yield return null;
		}

		m_isBeat = false;
	}

	public override void OnUpdate() //checks if on beat, if so adjust scale
	{
		base.OnUpdate();

		if (m_isBeat) return;

		transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat() //adjust to scale:
	{
		base.OnBeat();

		StopCoroutine("MoveToScale");
		StartCoroutine("MoveToScale", beatScale);
	}

	//public variables to control scale when a beat is hit vs when it is at rest:

	public Vector3 beatScale;
	public Vector3 restScale;
}
