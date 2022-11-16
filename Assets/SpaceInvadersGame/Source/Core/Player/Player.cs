using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SpaceInvadersGame.Source.Core.Player
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private int startingLives = 3;
		private int lives = 0;
		public int Lives
		{
			get => lives;
			set
			{
				if (lives == value) return;
				lives = value;
				livesUpdatedEvent?.Invoke(lives);
			}
		}
		public UnityAction<int> livesUpdatedEvent;

		[SerializeField] private Transform firingPosition;
		[SerializeField] private float fireSpeed = 10f;
		[SerializeField] private float fireRate = 1f;

		private bool canFire = false;
		private bool firing = false;
		private float firingElapsed = 0f;
		private float firingCooldown = 0f;
		private BulletPool bulletPool;

		private GameManager gameManager;

		private void OnDestroy()
		{
			livesUpdatedEvent = null;
		}

		private void Start()
		{
			bulletPool = GameObjects.Instance.bulletPool;
			gameManager = GameObjects.Instance.gameManager;
		}

		public void Update()
		{
			if (!canFire) return;
			if (Input.GetMouseButtonDown(0) && !firing)
			{
				//Debug.Log($"Player.Update: start firing", gameObject);
				firing = true;
				firingElapsed = 0f;
				if (firingCooldown <= 0) Fire();
			}
			else if (Input.GetMouseButtonUp(0) && firing)
			{
				//Debug.Log($"Player.Update: stop firing", gameObject);
				firing = false;
				firingCooldown = fireRate;
			}

			if (firing)
			{
				ChaseMouse(transform);

				if (firingElapsed + Time.deltaTime >= fireRate)
				{
					if (firingCooldown > 0)
					{
						//Debug.Log($"Player.Update: still on cooldown...", gameObject);
						return;
					}

					//Debug.Log($"Player.Update: firing.", gameObject);
					Fire();
					firingElapsed = 0f;
					return;
				}
				//Debug.Log($"Player.Update: waiting to fire...", gameObject);
				firingElapsed += Time.deltaTime;
			}

			if (firingCooldown > 0f)
			{
				firingCooldown -= Time.deltaTime;
			}
		}

		private static void ChaseMouse(Transform transform)
		{
			Vector3 p = Input.mousePosition;
			p.z = 0;
			Vector3 pos = Camera.main.ScreenToWorldPoint(p);
			if (pos.x >= 5.2)
				pos.x = 5.2f;

			if (pos.x <= -5.2)
				pos.x = -5.2f;

			pos.y = transform.position.y;
			pos.z = 0f;
			transform.position = pos;
		}

		private void Fire()
		{
			GameObject spawn = bulletPool.InstantiatePooledObject(firingPosition.transform.position);
			if (spawn == null) return;
			Rigidbody2D rigidbody = spawn.GetComponent<Rigidbody2D>();
			rigidbody.velocity = Vector2.up * fireSpeed;
		}

		[ContextMenu("IncrementLives")]
		public void IncrementLives()
		{
			Lives++;
		}

		[ContextMenu("DecrementLives")]
		public void DecrementLives()
		{
			if (Lives == 1)
			{
				gameManager.LoseGame();
			}
			else
			{
				Lives--;
			}
		}

		public void StartGame()
		{
			Lives = startingLives;
			canFire = true;
			firingElapsed = 0f;
			firingCooldown = fireRate;
		}

		public void EndGame()
		{
			canFire = false;
			firing = false;
			bulletPool.ReturnAllToPool();
		}

	}
}
