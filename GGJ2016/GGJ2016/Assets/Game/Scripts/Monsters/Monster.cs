﻿using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{

	[SerializeField]
	private float Speed;
	[SerializeField]
	private Vector3 InitialPosition;

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
			return MyT.localPosition.y;
		}
	}

	public void Show()
	{
		MyT.localPosition = InitialPosition;
		MyGo.SetActive(true);
	}

	public void Hide()
	{
		// TODO play die
		MyGo.SetActive(false);
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
