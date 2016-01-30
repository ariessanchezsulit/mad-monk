using UnityEngine;
using System.Collections;
using Common.Signal;

namespace Game
{
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

        void DisableRecognizers()
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
        void OnSwipeRecognized(TKSwipeRecognizer r)
        {
            Debug.Log("swipe recognized");

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

        void OnTapRecognized(TKTapRecognizer r)
        {
            Debug.Log("tap recognized");
            var worldPos = Camera.main.ScreenToWorldPoint(r.touchLocation());
            StartCoroutine(SpawnTapEffect(worldPos));
            GameSignals.INPUT_TAP.Dispatch();
        }

        void OnLongTapRecognized(TKLongPressRecognizer r)
        {
            Debug.Log("long press started");
        }

        void OnLongTapFinished(TKLongPressRecognizer r)
        {
            Debug.Log("long press finished");

            GameSignals.INPUT_LONG_PRESS.Dispatch();
        }

        void OnPinchRecognized(TKPinchRecognizer r)
        {
            Debug.Log("pinch recognized");

            GameSignals.INPUT_PINCH.Dispatch();
        }
        #endregion

        IEnumerator SpawnSwipeEffect(Vector3 start, Vector3 end, float duration, float speed)
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

        private IEnumerator SpawnTapEffect(Vector3 position)
        {
            var prefab = Instantiate(tapFXPrefab, position, Quaternion.identity) as GameObject;
            if (effectsRoot) prefab.transform.SetParent(effectsRoot);
            yield return null;
        }
    }
}