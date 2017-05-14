using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
	class MiniBossMovementRight : MovementPattern
	{
		public override void UpdateDirection(GameObject enemy)
		{
			enemy.Speed = 0.0025f * Settings.GlobalDisplayWidth;
			enemy.YDir = 1;

			if (enemy.YPos > Settings.GlobalDisplayHeight / 4)
			{
				enemy.Speed = 0.0005f * Settings.GlobalDisplayWidth;
				enemy.YDir = 0.1f;
				enemy.XDir = 1f;
			}

			if (enemy.YPos < 0 - enemy.Texture.Height)
				enemy.Destroy();
		}
	}
}
