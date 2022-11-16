using UnityEngine;
using UnityEngine.Events;

namespace Assets.SpaceInvadersGame.Source.Core.Score
{
    public class Scoreboard : MonoBehaviour
    {
		public UnityAction<int> scoreUpdatedEvent;
		public UnityAction<int> hiscoreUpdatedEvent;

		private int score = 0;
		private int hiscore = 0;

		private void OnDestroy()
		{
			hiscoreUpdatedEvent -= OnHiScoreUpdatedEventHandler;
			scoreUpdatedEvent = null;
			hiscoreUpdatedEvent = null;
		}

		protected void Start()
		{
			hiscoreUpdatedEvent += OnHiScoreUpdatedEventHandler;
			LoadHiScore();
		}

		public int GetScore()
		{
			return score;
		}

		public void SetScore(int value)
		{
			if (score == value) return;
			score = value;
			scoreUpdatedEvent?.Invoke(score);
		}
		public void IncrementScore(int amount)
		{
			SetScore(score + amount);
		}

		public int GetHiScore()
		{
			return hiscore;
		}

		public void SetHiScore(int value)
		{
			if (hiscore == value)
				return;
			hiscore = value;
			hiscoreUpdatedEvent?.Invoke(hiscore);
		}
		private void OnStartGame()
		{
			if (score != 0) SetScore(0);
		}
		private void OnEndGame()
		{
			if (score > hiscore)
				SetHiScore(score);
			SetScore(0);
		}

		private void OnHiScoreUpdatedEventHandler(int value)
		{
			PlayerPrefs.SetInt("hiscore", value);
		}

		private void LoadHiScore()
		{
			int temp = 0;
			if (PlayerPrefs.HasKey("hiscore"))
				temp = PlayerPrefs.GetInt("hiscore");
			SetHiScore(temp);
			hiscoreUpdatedEvent?.Invoke(hiscore);
		}

		#region unit tests
		[ContextMenu("StartGame")]
		public void StartGame()
		{
			OnStartGame();
		}

		[ContextMenu("IncrementScore")]
		public void IncrementScore()
		{
			SetScore(score+1);
		}

		[ContextMenu("DecrementScore")]
		public void DecrementScore()
		{
			SetScore(score - 1);
		}

		[ContextMenu("EndGame")]
		public void EndGame()
		{
			OnEndGame();
		}
		#endregion
	}
}