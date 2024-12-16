using DxLibDLL;

internal class Player
{
	private const int JUMP_FORCE = -20;

	private const int GRAVITY_INCREMENT = 1;

	// 画像の位置
	private int _posY = Game.GROUND_POS;

	// 重力
	private int _gravity = 0;

	// 画像のハンドル
	private int _playerImage;

	public void Load() {
		_playerImage = DX.LoadGraph(@"sprite/ground.png");
	}

	#region Update

	public void Update() {
		ApplyGravity();
		HandleInput();
		UpdatePosition();
	}

	private void ApplyGravity() {
		if (!IsGround()) {
			_gravity += GRAVITY_INCREMENT;
		}
		else {
			_posY = Game.GROUND_POS;
			_gravity = 0;
		}
	}

	private void HandleInput() {
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && IsGround()) {
			_gravity = JUMP_FORCE;
		}
	}

	private void UpdatePosition() {
		_posY += _gravity;
	}

	#endregion Update

	public void Draw() {
		DX.DrawGraph(Game.GROUND_SIZE, _posY - Game.GROUND_SIZE, _playerImage, DX.TRUE);
	}

	private bool IsGround() {
		// 地面についているかどうか
		return _posY >= Game.GROUND_POS;
	}
}
