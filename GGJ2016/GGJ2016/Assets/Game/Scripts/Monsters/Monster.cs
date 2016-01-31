using UnityEngine;
using System.Collections;

public enum MonsterType
{
	Crab,
	Centipede,
	Venus
}

public class Monster : MonoBehaviour
{
	public MonsterType Type;

	public int HitPoints;
	public int MaxHitPoints { get { return BubblesToReachBottom + BubblesToReachTop; } }

	public int BubblesToReachBottom;
	public int BubblesToReachTop;

	public float MonkEatenY;
	public float TowerTopY;
	public float TowerBottomY;
	public float HiddenY;

	public float LerpSpeed = 10;

	private float Speed;

	[SerializeField]
	private Transform RendererT;

	private GameObject _RendererGo;
	private GameObject RendererGo
	{
		get
		{
			if (_RendererGo == null)
				_RendererGo = RendererT.gameObject;
			return _RendererGo;
		}
	}

	public float YPosition
	{
		get
		{
			return RendererT.localPosition.y;
		}
		set
		{
			Vector3 localPos = RendererT.localPosition;
			localPos.y = value;
			RendererT.localPosition = localPos;
		}
	}

	public void Show(Vector3 initialPosition)
	{
		StopAllCoroutines ();

		HitPoints = BubblesToReachBottom;
//		RendererT.localPosition = initialPosition;
		RendererGo.SetActive(true);

		Slide (initialPosition);
	}

	public void Hide()
	{
		// TODO play die
//		RendererGo.SetActive(false);

		Vector3 hiddenPos = RendererT.localPosition;
		hiddenPos.y = HiddenY;
		SlideThenHide (hiddenPos);
	}

	private void Slide(Vector3 targetPos)
	{
		StopAllCoroutines ();
		StartCoroutine(SlideCoroutine(targetPos));
	}

	private void SlideThenHide(Vector3 targetPos)
	{
		StopAllCoroutines ();
		StartCoroutine(SlideThenHideCoroutine (targetPos));
	}

	private IEnumerator SlideThenHideCoroutine(Vector3 targetPos)
	{
		yield return StartCoroutine (SlideCoroutine (targetPos));
		RendererGo.SetActive(false);
	}

	private IEnumerator SlideCoroutine(Vector3 targetPos)
	{
		while (RendererT.localPosition != targetPos) {
			Vector3 newPos = Vector3.Lerp (RendererT.localPosition, targetPos, Time.deltaTime * LerpSpeed);
			RendererT.localPosition = newPos;
			yield return null;
		}
	}

	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	public void Rise()
	{
		HitPoints++;
		Vector3 pos = RendererT.localPosition;
		pos.y += Speed;
		Slide (pos);
//		RendererT.localPosition = pos;
	}

	public void Lower()
	{
		HitPoints--;
		Vector3 pos = RendererT.localPosition;
		pos.y -= Speed;
		Slide (pos);
//		RendererT.localPosition = pos;
	}
}
