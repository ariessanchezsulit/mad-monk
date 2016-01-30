using UnityEngine;
using System.Collections.Generic;
using Common.Signal;
using Game;

public class MonsterManager : MonoBehaviour
{
	[SerializeField]
	private Monster[] Monsters;

	private int CurrentMonsterIndex = -1;
	private Monster CurrentMonster;

	[SerializeField]
	private float MonsterMaxYPosition;
	[SerializeField]
	private float MonsterMinYPosition;

	void Awake()
	{
		// subscribe to signals
		GameSignals.START_GAME.AddListener (OnGameStart);
		GameSignals.ON_BUBBLE_POPPED.AddListener (OnBubblePopped);
		GameSignals.ON_BUBBLE_MISSED.AddListener (OnBubbleMissed);
	}

	void OnDestroy()
	{
		// subscribe to signals
		GameSignals.START_GAME.RemoveListener (OnGameStart);
		GameSignals.ON_BUBBLE_POPPED.RemoveListener (OnBubblePopped);
		GameSignals.ON_BUBBLE_MISSED.RemoveListener (OnBubbleMissed);
	}

	#region Event listener
	private void OnGameStart(ISignalParameters @params)
	{
		// reset monster index
		CurrentMonsterIndex = -1;

		// show next monster
		ShowNextMonster ();
	}

	private void OnBubbleMissed(ISignalParameters @params)
	{
		// raise monster
		CurrentMonster.Rise ();

		// check if monster has reached character
		bool isMonsterOnTop = CurrentMonster.YPosition >= MonsterMinYPosition;
		if (isMonsterOnTop) {
			// dispatch end game
			Signal signal = GameSignals.END_GAME;
			signal.Dispatch ();
		}
	}

	private void OnBubblePopped(ISignalParameters @params)
	{
		// lower monster
		CurrentMonster.Lower ();

		// check if monster has been defeated
		bool monsterDefeated = CurrentMonster.YPosition < MonsterMinYPosition;
		if (monsterDefeated) {
			// kill current monster
			KillCurrentMonster();

			// dispatch monster dead
			Signal signal = GameSignals.ON_MONSTER_DEAD;
			signal.Dispatch();

			// replace with new monster
			ShowNextMonster();
		}
	}
	#endregion

	private void ShowNextMonster()
	{
		// incremenet monster index
		CurrentMonsterIndex = CurrentMonsterIndex == Monsters.Length ? 0 : ++CurrentMonsterIndex;

		// update current monster
		CurrentMonster = Monsters[CurrentMonsterIndex];

		// show monster
		CurrentMonster.Show ();
	}

	private void KillCurrentMonster()
	{
		// hide monster
		CurrentMonster.Hide ();
	}
}
