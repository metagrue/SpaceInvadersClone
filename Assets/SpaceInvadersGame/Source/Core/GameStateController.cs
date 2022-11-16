using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SpaceInvadersGame.Source.Core
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPageObject = null;
        [SerializeField] private GameObject playPageObject = null;
        [SerializeField] private GameObject wonPageObject = null;
        [SerializeField] private GameObject lossPageObject = null;

        private GameManager gameManager;

        private void OnDestroy()
        {
            gameManager.stateUpdatedEvent -= OnStateUpdatedEventHandler;
            menuPageObject = null;
            playPageObject = null;
            wonPageObject = null;
            lossPageObject = null;
        }

        private void Start()
        {
            gameManager = GameObjects.Instance.gameManager;
            gameManager.stateUpdatedEvent += OnStateUpdatedEventHandler;
        }


        private void OnStateUpdatedEventHandler(GameManager.EState value)
        {
            //Debug.Log($"GameStateController.OnStateUpdatedEventHandler {value}", gameObject);
            OnStateUpdatedSwitch(value);

        }

        private void OnStateUpdatedSwitch(GameManager.EState value)
        {
            switch (value)
            {
                case GameManager.EState.MENU: OnStateMenu(); return;
                case GameManager.EState.PLAY: OnStatePlay(); return;
                case GameManager.EState.WON: OnStateWon(); return;
                case GameManager.EState.LOSS: OnStateLoss(); return;
            }
            Debug.LogWarning("GameStateController.OnStateUpdatedSwitch: state not handled", gameObject);
        }
        private void OnStateMenu()
        {
            //Debug.Log("GameStateController.OnStateMenu", gameObject);
            menuPageObject?.SetActive(true);
            playPageObject?.SetActive(false);
            wonPageObject?.SetActive(false);
            lossPageObject?.SetActive(false);
        }

        private void OnStatePlay()
        {
            //Debug.Log("GameStateController.OnStatePlay", gameObject);
            menuPageObject?.SetActive(false);
            playPageObject?.SetActive(true);
            wonPageObject?.SetActive(false);
            lossPageObject?.SetActive(false);
        }

        private void OnStateWon()
        {
            //Debug.Log("GameStateController.OnStateWon", gameObject);
            menuPageObject?.SetActive(false);
            playPageObject?.SetActive(false);
            wonPageObject?.SetActive(true);
            lossPageObject?.SetActive(false);
        }

        private void OnStateLoss()
        {
            //Debug.Log("GameStateController.OnStateLoss", gameObject);
            menuPageObject?.SetActive(false);
            playPageObject?.SetActive(false);
            wonPageObject?.SetActive(false);
            lossPageObject?.SetActive(true);
        }

    }
}