using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	public int BubblesToReachBottom;
	public int BubblesToReachTop;

	private float Speed;

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
			Debug.Log (MyT.localPosition.y);
			return MyT.localPosition.y;
		}
	}

	public void Show(Vector3 initialPosition)
	{
		MyT.localPosition = initialPosition;
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
		Vector3 pos = MyT.localPosition;
		pos.y += Speed;
		MyT.localPosition = pos;
	}

	public void Lower()
	{
		Vector3 pos = MyT.localPosition;
		pos.y -= Speed;
		MyT.localPosition = pos;
	}
}
