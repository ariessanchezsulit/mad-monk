using UnityEngine;
using System.Collections;
using Common.Signal;

namespace Game
{
    public enum GestureType
    {
        TAP,
        SWIPE,
        PINCH,
        LONG_PRESS,
        //NETWORK_TAP,
        //NETWORK_SWIPE
    }

    public class SwipePayload
    {
        public TKSwipeDirection direction;
        public Vector2 startScreenPos;
        public Vector2 endScreenPos;
        public float velocity;
    }
    
    public class GestureManager : MonoBehaviour
    {
        [SerializeField]
        public Transform effectsRoot;

        // prefabs for gesture effects
        [SerializeField]
        private GameObject swipeFXPrefab;
        [SerializeField]
        private GameObject tapFXPrefab;

        [SerializeField]
        private AnimationCurve curve;

        // initial gesture recognizers. can add more later
        private TKSwipeRecognizer swipeRecognizer;
        private TKTapRecognizer tapRecognizer;
        private TKPinchRecognizer pinchRecognizer;
        private TKLongPressRecognizer longPressRecognizer;

        [SerializeField]
        private bool _useNetwork = false;

        public bool UseNetwork
        {
            get { return _useNetwork; }
            set { _useNetwork = value; }
        }

        #region listeners setup
        private void Awake()
        {
            GameSignals.START_GAME.AddListener(OnGameStarted);
            GameSignals.END_GAME.AddListener(OnGameEnded);
        }

        private void OnDestroy()
        {
            GameSignals.START_GAME.RemoveListener(OnGameStarted);
            GameSignals.END_GAME.RemoveListener(OnGameEnded);
        }

        private void Start()
        {
            // for testing
            OnGameStarted(null);
        }
        #endregion

        void InitializeRecognizers()
        {
            swipeRecognizer = new TKSwipeRecognizer();
            swipeRecognizer.gestureRecognizedEvent += OnSwipeRecognized;
            TouchKit.addGestureRecognizer(swipeRecognizer);

            tapRecognizer = new TKTapRecognizer();
            tapRecognizer.gestureRecognizedEvent += OnTapRecognized;
            TouchKit.addGestureRecognizer(tapRecognizer);
            
            pinchRecognizer = new TKPinchRecognizer();
            pinchRecognizer.gestureRecognizedEvent += OnPinchRecognized;
            TouchKit.addGestureRecognizer(pinchRecognizer);

            longPressRecognizer = new TKLongPressRecognizer();
            longPressRecognizer.gestureRecognizedEvent += OnLongTapRecognized;
            longPressRecognizer.gestureCompleteEvent += OnLongTapFinished;
            TouchKit.addGestureRecognizer(longPressRecognizer);

            Debug.Log("recognizers successfully initialized");
        }

        public void DisableRecognizers()
        {
            TouchKit.removeAllGestureRecognizers();
        }

        #region listeners
        void OnGameStarted(ISignalParameters parameters)
        {
            InitializeRecognizers();
        }

        void OnGameEnded(ISignalParameters parameters)
        {
            DisableRecognizers();
        }
        #endregion

        #region dispatchers and input processing
        public void OnSwipeRecognized(TKSwipeRecognizer r)
        {
            Debug.Log("swipe recognized");

            if (!_useNetwork)
            {
                var start = Camera.main.ScreenToWorldPoint(r.startPoint);
                var end = Camera.main.ScreenToWorldPoint(r.endPoint);

                start = new Vector3(start.x, start.y, 0);
                end = new Vector3(end.x, end.y, 0);

                StartCoroutine(SpawnSwipeEffect(start, end, 0.5f, r.swipeVelocity));

                var signal = GameSignals.INPUT_SWIPE;
                signal.AddParameter(GameParams.INPUT_SWIPE_DIR, r.completedSwipeDirection);
                signal.Dispatch();
                signal.ClearParameters();
            }
            else
            {
                DispatchGenericSwipe(new SwipePayload()
                {
                    velocity = r.swipeVelocity,
                    startScreenPos = r.startPoint,
                    endScreenPos = r.endPoint,
                    direction = r.completedSwipeDirection
                });
            }
        }

        // don't use in gesture client controller
        public void DispatchGenericSwipe(SwipePayload payload)
        {
            //Debug.Log("Network Swipe detected");

            var genericSignal = GameSignals.INPUT_GENERIC;
            genericSignal.AddParameter(GameParams.INPUT_TYPE, GestureType.SWIPE);
            genericSignal.AddParameter(GameParams.INPUT_SWIPE_PAYLOAD, payload);
            genericSignal.Dispatch();
            genericSignal.ClearParameters();
        }

        public void OnTapRecognized(TKTapRecognizer r)
        {
            Debug.Log("tap recognized");

            if (!_useNetwork)
            {
                var worldPos = Camera.main.ScreenToWorldPoint(r.touchLocation());
                StartCoroutine(SpawnTapEffect(worldPos));
                GameSignals.INPUT_TAP.Dispatch();
            }
            else
            {
                DispatchGenericTap(r.touchLocation());
            }
        }

        // don't use in gesture client controller
        public void DispatchGenericTap(Vector2 position)
        {
            //Debug.Log("Network Tap detected");

            var signal = GameSignals.INPUT_GENERIC;
            signal.AddParameter(GameParams.INPUT_TYPE, GestureType.TAP);
            signal.AddParameter(GameParams.INPUT_TAP_POS, position);
            signal.Dispatch();
            signal.ClearParameters();
        }

        public void OnLongTapRecognized(TKLongPressRecognizer r)
        {
            Debug.Log("long press started");
        }

        public void OnLongTapFinished(TKLongPressRecognizer r)
        {
            Debug.Log("long press finished");

            if (!_useNetwork)
            {
                GameSignals.INPUT_LONG_PRESS.Dispatch();
            }
            else
            {
                var signal = GameSignals.INPUT_GENERIC;
                signal.AddParameter(GameParams.INPUT_TYPE, GestureType.LONG_PRESS);
                signal.Dispatch();
                signal.ClearParameters();
            }
        }

        public void OnPinchRecognized(TKPinchRecognizer r)
        {
            Debug.Log("pinch recognized");

            if (!_useNetwork)
            {
                GameSignals.INPUT_PINCH.Dispatch();
            }
            else
            { 
                var signal = GameSignals.INPUT_GENERIC;
                signal.AddParameter(GameParams.INPUT_TYPE, GestureType.PINCH);
                signal.Dispatch();
                signal.ClearParameters();
            }
        }
        #endregion

        #region spawners
        public IEnumerator SpawnSwipeEffect(Vector3 start, Vector3 end, float duration, float speed)
        {
            var direction = start - end;
            var prefab = Instantiate(swipeFXPrefab, start, Quaternion.LookRotation(direction)) as GameObject;
            if (effectsRoot) prefab.transform.SetParent(effectsRoot);
            float elapsedTime = 0f;

            while(elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fracTime = elapsedTime / duration;
                prefab.transform.position = direction * curve.Evaluate(fracTime);//Vector3.Lerp(start, end, Mathf.Sqrt(fracTime));

                yield return null;
            }

            DestroyObject(prefab);
        }

        public IEnumerator SpawnTapEffect(Vector3 position)
        {
            var prefab = Instantiate(tapFXPrefab, position, Quaternion.identity) as GameObject;
            if (effectsRoot) prefab.transform.SetParent(effectsRoot);
            yield return null;
        }
        #endregion
    }
}