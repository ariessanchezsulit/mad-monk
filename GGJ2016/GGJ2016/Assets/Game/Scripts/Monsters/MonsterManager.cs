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

	private bool IsActive = false;

	void Awake()
	{
		// subscribe to signals
		GameSignals.START_GAME.AddListener (OnStartGame);
		GameSignals.ON_BUBBLE_POPPED.AddListener (OnBubblePopped);
		GameSignals.ON_BUBBLE_MISSED.AddListener (OnBubbleMissed);
	}

	void OnDestroy()
	{
		// subscribe to signals
		GameSignals.START_GAME.RemoveListener (OnStartGame);
		GameSignals.ON_BUBBLE_POPPED.RemoveListener (OnBubblePopped);
		GameSignals.ON_BUBBLE_MISSED.RemoveListener (OnBubbleMissed);
	}

	// TEMP FOR TEST
	void Update()
	{
		if (Input.GetKeyUp (KeyCode.W)) {
			GameSignals.ON_BUBBLE_MISSED.Dispatch ();
		}

		if (Input.GetKeyUp (KeyCode.S)) {
			GameSignals.ON_BUBBLE_POPPED.Dispatch ();
		}

		if (Input.GetKeyUp (KeyCode.I)) {
			GameSignals.START_GAME.Dispatch ();
		}
	}

	#region Event listener
	private void OnStartGame(ISignalParameters @params)
	{
		IsActive = true;

		// hide current monster if applicable
		HideCurrentMonster ();

		// reset monster index
		CurrentMonsterIndex = -1;

		// show next monster
		ShowNextMonster ();
	}

	private void OnBubbleMissed(ISignalParameters @params)
	{
		if (!IsActive)
			return;

		// raise monster
		CurrentMonster.Rise ();

		Signal monsterSummonedSignal = GameSignals.ON_MONSTER_SUMMONED;
		monsterSummonedSignal.ClearParameters ();
		monsterSummonedSignal.AddParameter (GameParams.MONSTER_HP, CurrentMonster.HitPoints);
		monsterSummonedSignal.AddParameter (GameParams.MONSTER_MAX_HP, CurrentMonster.MaxHitPoints);
		monsterSummonedSignal.Dispatch ();

		// check if monster has reached character
		bool isMonsterOnTop = CurrentMonster.YPosition >= CurrentMonster.TowerTopY;
		if (isMonsterOnTop) {
			IsActive = false;

			CurrentMonster.YPosition = CurrentMonster.MonkEatenY;

			// dispatch end game
			GameSignals.END_GAME.Dispatch ();
		}
	}

	private void OnBubblePopped(ISignalParameters @params)
	{
		if (!IsActive)
			return;
		
		// lower monster
		CurrentMonster.Lower ();

		Signal monsterHitSignal = GameSignals.ON_MONSTER_HIT;
		monsterHitSignal.ClearParameters ();
		monsterHitSignal.AddParameter (GameParams.MONSTER_HP, CurrentMonster.HitPoints);
		monsterHitSignal.AddParameter (GameParams.MONSTER_MAX_HP, CurrentMonster.MaxHitPoints);
		monsterHitSignal.Dispatch ();

		// check if monster has been defeated
		bool monsterDefeated = CurrentMonster.YPosition < CurrentMonster.TowerBottomY;
		Debug.Log ("monsterDefeated: " + monsterDefeated);
		if (monsterDefeated) {
			// kill current monster
			HideCurrentMonster();

			// dispatch monster dead
			GameSignals.ON_MONSTER_DEAD.Dispatch();

			// replace with new monster
			ShowNextMonster();
		}
	}
	#endregion

	private void ShowNextMonster()
	{
		// incremenet monster index
		CurrentMonsterIndex = (CurrentMonsterIndex == Monsters.Length - 1) ? 0 : ++CurrentMonsterIndex;

		// update current monster
		CurrentMonster = Monsters[CurrentMonsterIndex];

		// set monster speed
		float towerHeight = CurrentMonster.TowerTopY - CurrentMonster.TowerBottomY;
		int heightInBubbles = CurrentMonster.BubblesToReachTop + CurrentMonster.BubblesToReachBottom;
		float monsterSpeed = towerHeight / ((float)heightInBubbles);
		CurrentMonster.SetSpeed (monsterSpeed);

		// show monster
		Vector3 initialPosition = Vector3.zero;
		initialPosition.y = CurrentMonster.TowerBottomY + (monsterSpeed * CurrentMonster.BubblesToReachBottom);
		CurrentMonster.Show (initialPosition);

		Signal monsterShowSignal = GameSignals.ON_MONSTER_SHOWN;
		monsterShowSignal.ClearParameters ();
		monsterShowSignal.AddParameter (GameParams.MONSTER_TYPE, CurrentMonster.Type);
		monsterShowSignal.Dispatch ();
	}

	private void HideCurrentMonster()
	{
		if (CurrentMonster == null)
			return;
		
		// hide monster
		CurrentMonster.Hide ();
	}
}
