using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	[SerializeField]
	private GameObject RendererGroup;

	[SerializeField]
	private float Speed;
	[SerializeField]
	private Vector3 InitialPosition;

	private Transform _MyT;
	private Transform MyT
	{
		get
		{
			if (_MyT == null)
				_MyT = null;
			return _MyT;
		}
	}

	public float YPosition
	{
		get
		{
			return MyT.localPosition.y;
		}
	}

	public void Show()
	{
		MyT.localPosition = InitialPosition;
		RendererGroup.SetActive(false);
	}

	public void Hide()
	{
		// TODO play die
		RendererGroup.SetActive(true);
	}

	public void Rise()
	{
		Vector3 pos = MyT.localPosition;
		pos.y -= Speed;
		MyT.localPosition = pos;
	}

	public void Lower()
	{
		Vector3 pos = MyT.localPosition;
		pos.y += Speed;
		MyT.localPosition = pos;
	}
}
