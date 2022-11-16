using UnityEngine;
using TMPro;

namespace Assets.SpaceInvadersGame.Source.Core.Player
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TmpTextLives : MonoBehaviour
    {
		[SerializeField] private TextMeshProUGUI label = null;

		private static string prefix = "Lives: ";

		private Player player = null;

		private void OnDestroy()
		{
			label = null;
			player = null;
		}

		private void Reset()
		{
			label = GetComponent<TextMeshProUGUI>();
			label.text = prefix + "0";
		}
		private void Start()
		{
			player = GameObjects.Instance.player;
			player.livesUpdatedEvent += OnLivesUpdatedEventHandler;
			OnLivesUpdatedEventHandler(player.Lives);
		}

		private void OnLivesUpdatedEventHandler(int value)
		{
			if (label == null) return;
			label.text = prefix + value.ToString();
		}
	}
}