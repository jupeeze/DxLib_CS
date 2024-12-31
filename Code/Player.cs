using DxLibDLL;
using System;

using static DxLibDLL.DX;

internal static class Player
{
	private static readonly int SIZE_X = 50, SIZE_Y = 37;
	private static readonly int JUMP_FORCE = -15;
	private static readonly int GRAVITY_INCREMENT = 1;

	private static int _imagePosY2 = Game.GROUND_POS;

	private static int _gravity = 0;

	private static int _imageNum = 0;

	private static int[] _playerImages;

	private static int _availableJumps = 2;

	private static int _currentTile;

	public static readonly int ImagePosX1 = SIZE_X;
	public static readonly int ImagePosX2 = ImagePosX1 + (2 * SIZE_Y);

	public static readonly int HitboxPosX1 = ((ImagePosX1 + ImagePosX2) / 2) - 10;
	public static readonly int HitboxPosX2 = ((ImagePosX1 + ImagePosX2) / 2) + 10;
	public static readonly int HitboxCenterX = (HitboxPosX1 + HitboxPosX2) / 2;

	public static int ImagePosY1 => ImagePosY2 - (2 * SIZE_Y);
	public static int ImagePosY2 => _imagePosY2;

	public static int HitboxPosY1 => ((ImagePosY1 + ImagePosY2) / 2) - 20;
	public static int HitboxPosY2 => ImagePosY2;
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
				_imagePosY2 = Game.GROUND_POS - (Ground._heights[_currentTile] * Game.GROUND_SIZE);

				_availableJumps = 2;
			}
		}
	}

	public static void Load() {
		int divX = 7, divY = 16;
		int spriteCount = divX * divY;
		_playerImages = new int[spriteCount];

		DX.LoadDivGraph(Game.ASSET_PATH + @"adventurer-v1.5-Sheet.png", spriteCount, divX, divY, SIZE_X, SIZE_Y, _playerImages);
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

	private static bool _isJumpKeyPressed = false;

	private static void HandleInput() {
		// 左クリックが押された瞬間を検知
		bool isMouseLeftClicked = DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT;

		if (isMouseLeftClicked && !_isJumpKeyPressed) {
			// ジャンプ処理
			if (_availableJumps > 0) {
				State = PlayerState.Jumping;
				_gravity = JUMP_FORCE;
				_availableJumps--;
			}
		}

		// 現在のボタン状態を更新
		_isJumpKeyPressed = isMouseLeftClicked;
	}

	private static void UpdatePosition() {
		_imagePosY2 += _gravity;
	}

	private static void Attack() {
		if (Enemy.HitboxPosY1 <= HitboxCenterY && HitboxCenterY <= Enemy.HitboxPosY2) {
			if (State != PlayerState.Attacking) {
				State = PlayerState.Attacking;
			}

			Enemy.Damage(1);
		}
	}

	#endregion Update

	#region Draw

	public static void Draw() {
		DX.DrawExtendGraph(ImagePosX1, ImagePosY1, ImagePosX2, ImagePosY2, GetCurrentImage(), DX.TRUE);

		uint Cr = DX.GetColor(0, 0, 255);

		DX.DrawBox(HitboxPosX1, HitboxPosY1, HitboxPosX2, HitboxPosY2, Cr, DX.FALSE);

		int x = _currentTile * Game.GROUND_SIZE + Game.Diff, y = Game.GROUND_POS - (Ground._heights[_currentTile] * Game.GROUND_SIZE);
		DX.DrawBox(x, y, x + Game.GROUND_SIZE, y + Game.GROUND_SIZE, Cr, DX.FALSE);
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

	public static bool IsGround() {
		// プレイヤーの下端が地面の高さと一致しているか判定
		_currentTile = (HitboxPosX2 - Game.Diff) / Game.GROUND_SIZE; // プレイヤーが位置する地面タイル

		// 地面の高さを取得
		int groundHeight = Game.GROUND_POS - (Ground._heights[_currentTile] * Game.GROUND_SIZE);

		if (HitboxPosY1 >= Game.SCREEN_Y) {
			Die();
			return false;
		}

		// プレイヤーの下端と地面の高さを比較
		return HitboxPosY2 >= groundHeight;
	}

	public static void Die() {
		DX.DxLib_End();
	}
}
