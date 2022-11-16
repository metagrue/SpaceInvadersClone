using System.Collections;
using UnityEngine;
using static Assets.SpaceInvadersGame.Source.Core.Enemy.EnemyController;

namespace Assets.SpaceInvadersGame.Source.Core.Enemy
{
	public class Enemy : MonoBehaviour
	{

		public int points = 10;
		public EnemyController controller;

		public Player.Player player;
		public Score.Scoreboard scoreboard;

		[SerializeField] private float moveSpeed = 0.5f;

		Rigidbody2D rigbody = null;
		private void Start()
		{
			player = GameObjects.Instance.player;
			scoreboard = GameObjects.Instance.scoreboard;
			rigbody = GetComponent<Rigidbody2D>();

			controller.moveDirectionUpdated += OnMoveDirectionUpdated;
		}

		private void OnDestroy()
		{
			controller.moveDirectionUpdated -= OnMoveDirectionUpdated;
			rigbody = null;
		}

		private void OnMoveDirectionUpdated(EMoveDirection moveDirection)
		{
			Vector2 dir = Vector2.down;
			switch (moveDirection)
			{
				case EMoveDirection.RIGHT: dir = Vector3.right; break;
				case EMoveDirection.LEFT: dir = Vector3.left; break;
			}
			rigbody.velocity = dir * moveSpeed;
		}


		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.tag == "Goal")
			{
				player.DecrementLives();
				Die(false);
			}
		}

		[ContextMenu("Die")]
		public void Die(bool worth = true)
		{
			controller.AddToPool(gameObject);
			if (worth) scoreboard.IncrementScore(points);
		}
	}
}