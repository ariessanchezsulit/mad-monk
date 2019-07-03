using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Common.Signal;

namespace Game {
	
	public abstract class GameSignals {
		
		public static readonly Signal ON_CLICKED_BUTTON = new Signal("OnClickedButton");
		public static readonly Signal ON_BUBBLE_POPPED = new Signal("OnBubblePopped");
		public static readonly Signal ON_BUBBLE_MISSED = new Signal("OnBubbleMissed");
		public static readonly Signal ON_MONSTER_SHOWN = new Signal("OnMonsterShown");
		public static readonly Signal ON_MONSTER_HIT = new Signal("OnMonsterHit");
		public static readonly Signal ON_MONSTER_SUMMONED = new Signal("OnMonsterSummoned");
		public static readonly Signal ON_MONSTER_DEAD = new Signal("OnMonsterDead");
		public static readonly Signal START_GAME = new Signal("StartGame");
		public static readonly Signal END_GAME = new Signal("EndGame");
        public static readonly Signal INPUT_TAP = new Signal("tap");
        public static readonly Signal INPUT_SWIPE = new Signal("swipe");
        public static readonly Signal INPUT_PINCH = new Signal("pinch");
        public static readonly Signal INPUT_LONG_PRESS = new Signal("longpress");
        public static readonly Signal INPUT_GENERIC = new Signal("genericinput");
        public static readonly Signal DEBUG_LOG = new Signal("debuglog");
        public static readonly Signal INPUT_NETWORK = new Signal("networkinput");
        public static readonly Signal INPUT_HOST_IP = new Signal("inputhostip");
        public static readonly Signal DISPLAY_HOST_IP = new Signal("displayhostip");
        public static readonly Signal NETWORK_STATUS_SIGNAL = new Signal("networkstatus");

        public static readonly Signal ON_BUBBLE_ADDED_TO_POOL = new Signal("OnBubbleAddedToPool");
		public static readonly Signal ON_BUBBLE_REMOVED_TO_POOL = new Signal("OnBubbleRemovedToPool");

		public static readonly Signal ON_PLAY_SFX = new Signal("OnPlaySFX");
		public static readonly Signal ON_PLAY_BGM = new Signal("OnPlayBGM");
	}

}