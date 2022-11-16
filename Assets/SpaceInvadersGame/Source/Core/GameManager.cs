using UnityEngine;
using UnityEngine.Events;

namespace Assets.SpaceInvadersGame.Source.Core
{
    public class GameManager : MonoBehaviour
    {
        public enum EState
        {
            NONE = 0,
            MENU = 1,
            PLAY = 2, 
            LOSS = 3,
            WON = 4
        }

        public UnityAction<EState> stateUpdatedEvent;

        private EState lastState = EState.NONE;
        private EState state = EState.NONE;
		[SerializeField] private EState startState = EState.MENU;

        private Score.Scoreboard scoreboard = null;
        private Enemy.EnemyController enemyController = null;
        private Player.Player player = null;


		public EState State
        {
            get => state;
            set
            {
                state = value;
                stateUpdatedEvent?.Invoke(state);
            }
        }

        private void OnDestroy()
        {
            stateUpdatedEvent -= OnStateUpdatedEventHandler;
            scoreboard = null;
            enemyController = null;
            player = null;
        }

        private void Start()
		{
            stateUpdatedEvent += OnStateUpdatedEventHandler;
            State = startState;

            scoreboard = GameObjects.Instance.scoreboard;
            enemyController = GameObjects.Instance.enemyController;
            player = GameObjects.Instance.player;
        }

        private void OnStateUpdatedEventHandler(EState value)
        {
            //Debug.Log($"GameManager.OnStateUpdatedEventHandler {value}", gameObject);
            lastState = state;
            state = value;
            OnStateUpdatedSwitch();

        }

        private void OnStateUpdatedSwitch()
        {
            switch (state)
            {
                case EState.MENU: OnStateMenu(); return;
                case EState.PLAY: OnStatePlay(); return;
                case EState.WON: OnStateWon(); return;
                case EState.LOSS: OnStateLoss(); return;
            }
            Debug.LogWarning("GameManager.OnStateUpdatedSwitch: state not handled", gameObject);
        }

        private void OnStateMenu()
        {
            //Debug.Log("GameManager.OnStateMenu", gameObject);
        }

        private void OnStatePlay()
        {
            //Debug.Log("GameManager.OnStatePlay", gameObject);
            scoreboard.StartGame();
            enemyController.StartGame();
            player.StartGame();
        }

        private void OnStateWon()
        {
            //Debug.Log("GameManager.OnStateWon", gameObject);
            scoreboard.EndGame();
            enemyController.EndGame();
            player.EndGame();
        }

        private void OnStateLoss()
        {
            //Debug.Log("GameManager.OnStateLoss", gameObject);
            scoreboard.EndGame();
            enemyController.EndGame();
            player.EndGame();
        }

        #region unit tests
        [ContextMenu("RestartMenu")]
        public void RestartMenu()
        {
            Debug.Log("GameManager.RestartMenu", gameObject);
            State = EState.MENU;
        }

        [ContextMenu("StartGame")]
        public void StartGame()
        {
            Debug.Log("GameManager.StartGame", gameObject);
            State = EState.PLAY;
        }

        [ContextMenu("WonGame")]
        public void WonGame()
        {
            Debug.Log("GameManager.WonGame", gameObject);
            State = EState.WON;
        }

        [ContextMenu("LoseGame")]
        public void LoseGame()
        {
            Debug.Log("GameManager.LoseGame", gameObject);
            State = EState.LOSS;
        }
		#endregion

	}
}
