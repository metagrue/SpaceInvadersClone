using System.Collections;
using UnityEngine;

namespace Assets.SpaceInvadersGame.Source.Core.Player
{
	public class Bullet : MonoBehaviour
	{
		public BulletPool poolController;
		private static float lifetime = 6f;
		private float elapsed = 0f;


		private void Update()
		{			
			if (elapsed + Time.deltaTime >= lifetime)
			{
				Die();
				return;
			}
			elapsed += Time.deltaTime;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.tag == "Enemy")
			{
				Enemy.Enemy enemy = collider.gameObject.GetComponent<Enemy.Enemy>();
				enemy.Die();
			}
			Die();
		}

		[ContextMenu("Die")]
		public void Die()
		{
			elapsed = 0f;
			poolController.AddToPool(gameObject);
		}
	}
}