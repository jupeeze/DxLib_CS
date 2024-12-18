using DxLibDLL;
using System.Collections.Generic;

internal static class Enemy
{
	private static List<Summon> _summons = new List<Summon>();

	private static readonly int SIZE_X = 100, SIZE_Y = 100;
	private static readonly int ATTACK_INTERVAL = 120;

	// 画像の差し替え
	private static int _imageNum = 0;

	// 攻撃のタイミング
	private static int _attackTiming = 0;

	// 画像のハンドル
	private static List<int[]> _enemyImages;

	// 今攻撃しているか
	private static bool _isAttacking = false;

	public static void Load() {
		_enemyImages = new List<int[]>();

		if (_enemyImages.Count == 0) {
			LoadSprite("sprite/Undead executioner/idle2.png", 4, 2);
			LoadSprite("sprite/Undead executioner/summon.png", 4, 2);
		}
	}

	private static void LoadSprite(string filePath, int divX, int divY) {
		int spriteCount = divX * divY;
		int[] sprites = new int[spriteCount];
		DX.LoadDivGraph(filePath, spriteCount, divX, divY, SIZE_X, SIZE_Y, sprites);
		_enemyImages.Add(sprites);
	}

	public static void Update() {
		Attack();
	}

	public static void Draw() {
		int imageIndex = _isAttacking ? 1 : 0;

		DX.DrawExtendGraph(Game.SCREEN_X - SIZE_X, Game.GROUND_POS - (2 * SIZE_Y), Game.SCREEN_X - (3 * SIZE_X), Game.GROUND_POS, _enemyImages[imageIndex][_imageNum], DX.TRUE);

		foreach (var summon in _summons)
			if (summon.IsActive)
				summon.Draw();

		uint _black = DX.GetColor(0, 0, 0);
		uint _white = DX.GetColor(255, 255, 255);

		if (_summons.Count > 0) {
			for (int i = 0; i < _summons.Count; i++) {
				DX.DrawString(50, 100 + (i * 50),
						$"{_summons[i].IsActive}", _black, _white);
				DX.DrawString(100, 100 + (i * 50),
						$"{_summons[i].IsMoving}", _black, _white);
			}
		}
	}

	public static void Animator() {
		_imageNum = 0;
	}

	public static void Animation() {
		_imageNum++;

		int maxFrame = _isAttacking ? 5 : 8;

		_imageNum %= maxFrame;

		if (_isAttacking && _imageNum == 0) {
			_isAttacking = false;
		}

		foreach (var summon in _summons) {
			if (summon.IsActive) {
				summon.Animation();
			}
		}
	}

	public static void Attack() {
		_attackTiming++;
		if (_attackTiming % ATTACK_INTERVAL == 0) {
			_isAttacking = true;
			Animator();

			if (Summonable()) {
				var summon = new Summon();
				summon.Load();
				_summons.Add(summon);
			}
		}
	}

	public static bool Summonable() {
		foreach (Summon summon in _summons) {
			if (summon.IsActive && !summon.IsMoving) {
				summon.Attack();
				return false;
			}
		}

		var inactiveSummon = _summons.Find(s => !s.IsActive);
		if (inactiveSummon != null) {
			inactiveSummon.Load();
			return false;
		}

		return true;
	}
}
