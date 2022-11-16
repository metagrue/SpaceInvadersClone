using UnityEngine;
using TMPro;

namespace Assets.SpaceInvadersGame.Source.Core.Score
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TmpTextScore : MonoBehaviour
    {
		[SerializeField] private TextMeshProUGUI label = null;

		private static string prefix = "Score: ";

		private Scoreboard scoreboard = null;

		private void OnDestroy()
		{
			label = null;
			scoreboard = null;
		}

		private void Reset()
		{
			label = GetComponent<TextMeshProUGUI>();
			label.text = prefix + "0";
		}
		private void Start()
		{
			scoreboard = GameObjects.Instance.scoreboard;
			scoreboard.scoreUpdatedEvent += OnScoreUpdatedEventHandler;
			OnScoreUpdatedEventHandler(scoreboard.GetScore());
		}

		private void OnScoreUpdatedEventHandler(int value)
		{
			if (label == null) return;
			label.text = prefix + value.ToString();
		}
	}
}