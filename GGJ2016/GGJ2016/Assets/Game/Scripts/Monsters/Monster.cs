using UnityEngine;
using System.Collections;
using Game;
using Common.Signal;

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

	void OnEnable()
	{
		Signal signal = GameSignals.ON_PLAY_SFX;
		signal.AddParameter(GameParams.AUDIO_ID, ESfx.Start);
		signal.Dispatch();
	}

	public void Show(Vector3 initialPosition)
	{
		HitPoints = BubblesToReachBottom;
		RendererT.localPosition = initialPosition;
		RendererGo.SetActive(true);

		Signal signal = GameSignals.ON_PLAY_SFX;
		signal.AddParameter(GameParams.AUDIO_ID, UnityEngine.Random.Range(0, 2) == 0 ? ESfx.Monster001 : ESfx.Monster002);
		signal.Dispatch();
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
		HitPoints++;
		Vector3 pos = RendererT.localPosition;
		pos.y += Speed;
		RendererT.localPosition = pos;
	}

	public void Lower()
	{
		HitPoints--;
		Vector3 pos = RendererT.localPosition;
		pos.y -= Speed;
		RendererT.localPosition = pos;
	}
}
