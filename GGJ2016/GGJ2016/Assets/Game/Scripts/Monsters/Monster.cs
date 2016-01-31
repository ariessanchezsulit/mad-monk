using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	public int BubblesToReachBottom;
	public int BubblesToReachTop;

	public float MonkEatenY;
	public float TowerTopY;
	public float TowerBottomY;

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
		RendererT.localPosition = initialPosition;
		RendererGo.SetActive(true);
	}

	public void Hide()
	{
		// TODO play die
		RendererGo.SetActive(false);
	}

	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	public void Rise()
	{
		Vector3 pos = RendererT.localPosition;
		pos.y += Speed;
		RendererT.localPosition = pos;
	}

	public void Lower()
	{
		Vector3 pos = RendererT.localPosition;
		pos.y -= Speed;
		RendererT.localPosition = pos;
	}
}
