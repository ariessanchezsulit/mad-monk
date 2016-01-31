using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	public int BubblesToReachBottom;
	public int BubblesToReachTop;

	private float Speed;

	[SerializeField]
	private Transform RendererT;

	private GameObject _MyGo;
	private GameObject MyGo
	{
		get
		{
			if (_MyGo == null)
				_MyGo = gameObject;
			return _MyGo;
		}
	}

	private Transform _MyT;
	private Transform MyT
	{
		get
		{
			if (_MyT == null)
				_MyT = transform;
			return _MyT;
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
		MyGo.SetActive(true);
	}

	public void Hide()
	{
		// TODO play die
		MyGo.SetActive(false);
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
