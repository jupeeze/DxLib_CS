using DxLibDLL;

internal static class Player
{
	// 画像のサイズ
	private static readonly int SIZE_X = 50, SIZE_Y = 37;

	private static readonly int JUMP_FORCE = -20;

	private static readonly int GRAVITY_INCREMENT = 1;

	// 画像の位置
	private static int _posY = Game.GROUND_POS;

	// 重力
	private static int _gravity = 0;

	// 画像の差し替え
	private static int _imageNum = 0;

	// 画像のハンドル
	private static int[] _playerImages;

	public static void Load() {
		int divX = 7, divY = 16;
		int spriteCount = divX * divY;
		_playerImages = new int[spriteCount];

		DX.LoadDivGraph("sprite/player-Sheet.png", spriteCount, divX, divY, SIZE_X, SIZE_Y, _playerImages);
	}

	#region Update

	public static void Update() {
		ApplyGravity();
		HandleInput();
		UpdatePosition();
	}

	private static void ApplyGravity() {
		if (!IsGround()) {
			_gravity += GRAVITY_INCREMENT;
		}
		else {
			_posY = Game.GROUND_POS;
			_gravity = 0;
		}
	}

	private static void HandleInput() {
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && IsGround()) {
			_gravity = JUMP_FORCE;
		}
	}

	private static void UpdatePosition() {
		_posY += _gravity;
	}

	#endregion Update

	#region Draw

	public static void Draw() {
		int x = 3 * SIZE_X, y = _posY;
		DX.DrawExtendGraph(x - (2 * SIZE_X), y - (2 * SIZE_Y), x, y, GetCurrentImage(), DX.TRUE);
	}

	private static int GetCurrentImage() {
		if (IsGround())
			return _playerImages[8 + _imageNum];

		return (_gravity <= 0) ? _playerImages[14 + _imageNum] : _playerImages[18 + _imageNum];
	}

	#endregion Draw

	public static void Animation() {
		_imageNum++;
		if (IsGround()) {
			_imageNum %= 6; // 地上アニメーション
		}
		else {
			_imageNum %= 4; // 空中アニメーション
		}
	}

	private static bool IsGround() {
		// 地面についているかどうか
		return _posY >= Game.GROUND_POS;
	}
}
