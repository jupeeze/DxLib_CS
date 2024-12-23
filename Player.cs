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

	public static readonly int HitboxPosX1 = ((PosX1 + PosX2) / 2) - 10;
	public static readonly int HitboxPosX2 = ((PosX1 + PosX2) / 2) + 10;
	public static readonly int HitboxCenterX = (HitboxPosX1 + HitboxPosX2) / 2;

	public static int PosY1 => PosY2 - (2 * SIZE_Y);
	public static int PosY2 => _posY2;

	public static int HitboxPosY1 => ((PosY1 + PosY2) / 2) - 20;
	public static int HitboxPosY2 => PosY2;
	public static int HitboxCenterY => (HitboxPosY1 + HitboxPosY2) / 2;

	private enum PlayerState
	{ Running, Jumping, Attacking }

	private static PlayerState _state = PlayerState.Running;

	private static PlayerState State {
		get => _state;
		set {
			_state = value;
			_imageNum = 0;

			if (_state == PlayerState.Running) {
				_gravity = 0;
				_posY2 = Game.GROUND_POS;
			}
		}
	}

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
		Attack();
	}

	private static void ApplyGravity() {
		if (!IsGround())
			_gravity += GRAVITY_INCREMENT;
		else if (State != PlayerState.Running)
			State = PlayerState.Running;
	}

	private static void HandleInput() {
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && State == PlayerState.Running) {
			State = PlayerState.Jumping;
			_gravity = JUMP_FORCE;
		}
	}

	private static void UpdatePosition() {
		_posY2 += _gravity;
	}

	private static void Attack() {
		if (Enemy.HitboxPosY1 <= HitboxCenterY && HitboxCenterY <= Enemy.HitboxPosY2) {
			if (State != PlayerState.Attacking) {
				State = PlayerState.Attacking;
			}
		}
	}

	#endregion Update

	#region Draw

	public static void Draw() {
		DX.DrawExtendGraph(PosX1, PosY1, PosX2, PosY2, GetCurrentImage(), DX.TRUE);

		uint Cr = DX.GetColor(0, 0, 255);
		DX.DrawBox(HitboxPosX1, HitboxPosY1, HitboxPosX2, HitboxPosY2, Cr, DX.FALSE);
	}

	private static int GetCurrentImage() {
		switch (State) {
			case PlayerState.Running:
				return _playerImages[8 + _imageNum];

			case PlayerState.Jumping:
				return (_gravity <= 0) ? _playerImages[16 + _imageNum] : _playerImages[22 + _imageNum];

			case PlayerState.Attacking:
				return _playerImages[97 + _imageNum];

			default: throw new InvalidOperationException();
		}
	}

	#endregion Draw

	public static void Animation() {
		_imageNum++;
		switch (State) {
			case PlayerState.Running:
				_imageNum %= 6; // 地上アニメーション
				break;

			case PlayerState.Jumping:
				_imageNum %= 2; // 空中アニメーション
				break;

			case PlayerState.Attacking:
				_imageNum %= 5; // 攻撃アニメーション
				if (_imageNum == 0) {
					State = IsGround() ? PlayerState.Running : PlayerState.Jumping;
				}
				break;

			default: throw new InvalidOperationException();
		}
	}

	private static bool IsGround() {
		// 地面についているかどうか
		return PosY2 >= Game.GROUND_POS;
	}

	public static void Die() {
		DX.DxLib_End();
	}
}
