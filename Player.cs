using DxLibDLL;

internal static class Player
{
	private static readonly int SIZE_X = 50, SIZE_Y = 37;
	private static readonly int JUMP_FORCE = -20;
	private static readonly int GRAVITY_INCREMENT = 1;

	private static int _posY2 = Game.GROUND_POS;

	private static int _gravity = 0;

	private static int _imageNum = 0;

	private static int[] _playerImages;

	public static readonly int PosX1 = SIZE_X;
	public static readonly int PosX2 = PosX1 + (2 * SIZE_Y);

	public static int PosY1 => PosY2 - (2 * SIZE_Y);
	public static int PosY2 => _posY2;

	public static readonly int HitboxPosX1 = ((PosX1 + PosX2) / 2) - 10;
	public static readonly int HitboxPosX2 = ((PosX1 + PosX2) / 2) + 10;
	public static readonly int HitboxCenterX = (HitboxPosX1 + HitboxPosX2) / 2;

	public static int HitboxPosY1 => ((PosY1 + PosY2) / 2) - 20;
	public static int HitboxPosY2 => PosY2;
	public static int HitboxCenterY => (HitboxPosY1 + HitboxPosY2) / 2;

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
			_posY2 = Game.GROUND_POS;
			_gravity = 0;
		}
	}

	private static void HandleInput() {
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && IsGround()) {
			_gravity = JUMP_FORCE;
		}
	}

	private static void UpdatePosition() {
		_posY2 += _gravity;
	}

	#endregion Update

	#region Draw

	public static void Draw() {
		DX.DrawExtendGraph(PosX1, PosY1, PosX2, PosY2, GetCurrentImage(), DX.TRUE);
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
		return _posY2 >= Game.GROUND_POS;
	}
}
