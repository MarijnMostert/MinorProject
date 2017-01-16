using UnityEngine;
using System.Collections;

public class TrailRendererHelper : MonoBehaviour
{
	protected TrailRenderer mTrail;
	protected float mTime = 0;

	void Awake()
	{
		mTrail = gameObject.GetComponent<TrailRenderer>();
		if (null == mTrail)
		{
			Debug.Log("[TrailRendererHelper.Awake] invalid TrailRenderer.");
			return;
		}

		mTime = mTrail.time;
	}

	void OnEnable()
	{
		if (null == mTrail)
		{
			return;
		}

		StartCoroutine(ResetTrails());
	}

	IEnumerator ResetTrails()
	{
		mTrail.time = 0;

		yield return new WaitForEndOfFrame();

		mTrail.time = mTime;
	}
}