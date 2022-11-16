using UnityEngine;
using TMPro;

namespace Assets.SpaceInvadersGame.Source.Core.Score
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TmpTextHiScore : MonoBehaviour
    {
		[SerializeField] private TextMeshProUGUI label = null;
		private static string prefix = "Hiscore: ";

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
			scoreboard.hiscoreUpdatedEvent += OnHiScoreUpdatedEventHandler;
			OnHiScoreUpdatedEventHandler(scoreboard.GetHiScore());
		}

		private void OnHiScoreUpdatedEventHandler(int value)
		{
			if (label == null) return;
			label.text = prefix + value.ToString();
		}
	}
}