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

	public static readonly int PosX1 = Game.SCREEN_X - SIZE_X;
	public static readonly int PosY1 = Game.GROUND_POS - (2 * SIZE_Y);

	public static readonly int PosX2 = PosX1 - (2 * SIZE_X);
	public static readonly int PosY2 = PosY1 + (2 * SIZE_Y);

	public static readonly int HitboxPosX1 = ((PosX1 + PosX2) / 2) - 10;
	public static readonly int HitboxPosX2 = ((PosX1 + PosX2) / 2) + 30;
	public static readonly int HitboxCenterX = (HitboxPosX1 + HitboxPosX2) / 2;

	public static readonly int HitboxPosY1 = ((PosY1 + PosY2) / 2) - 40;
	public static readonly int HitboxPosY2 = ((PosX1 + PosX2) / 2) - 20;
	public static readonly int HitboxCenterY = (HitboxPosY1 + HitboxPosY2) / 2;

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

		foreach (var summon in _summons)
			if (summon.IsMoving)
				summon.Move();
	}

	public static void Draw() {
		int imageIndex = _isAttacking ? 1 : 0;

		DX.DrawExtendGraph(PosX1, PosY1, PosX2, PosY2, _enemyImages[imageIndex][_imageNum], DX.TRUE);

		foreach (var summon in _summons)
			if (summon.IsActive)
				summon.Draw();

		uint Cr = DX.GetColor(0, 0, 255);
		DX.DrawBox(HitboxPosX1, HitboxPosY1, HitboxPosX2, HitboxPosY2, Cr, DX.FALSE);
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
