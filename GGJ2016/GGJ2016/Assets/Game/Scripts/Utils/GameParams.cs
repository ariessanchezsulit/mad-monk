using System;

namespace Game {
	
	public abstract class GameParams {

		// buttons
		public const string BUTTON_TYPE = "ButtonType";
		public const string BUTTON_DATA = "ButtonData";

		// bubbles
		public const string BUBBLE = "Bubble";

        // gestures
        public const string INPUT_SWIPE_DIR = "SwipeDirection";
        public const string INPUT_TYPE = "GestureInputType";
        public const string INPUT_SWIPE_PAYLOAD = "SwipePayload";
        public const string INPUT_TAP_POS = "TapPosition";

		// monsters
		public const string MONSTER_TYPE = "MonsterType";
		public const string MONSTER_HP = "MonsterHP";
		public const string MONSTER_MAX_HP = "MonsterMaxHP";

        // debug
        public const string DEBUG_TEXT = "DebugText";

        // network
        public const string NETWORK_CLIENT_ID = "NetworkClientId";

    }

}

