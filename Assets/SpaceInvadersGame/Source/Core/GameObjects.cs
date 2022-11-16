using System.Collections;
using UnityEngine;

namespace Assets.SpaceInvadersGame.Source.Core
{
	public class GameObjects : Singleton<GameObjects>
	{
		public Score.Scoreboard scoreboard;
		public GameManager gameManager;

		public Enemy.EnemyController enemyController;
		public Player.BulletPool bulletPool;
		public Player.Player player;
	}
}