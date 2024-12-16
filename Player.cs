using DxLibDLL;

internal class Player
{
	// 画像の位置
	private int _posY = Game.GROUND_POS;

	// 重力
	private int _gravity = 0;

	// 画像のハンドル
	private int _playerImage;

	public void Load() {
		// PNG画像のメモリへの読みこみ
		_playerImage = DX.LoadGraph(@"sprite/ground.png");
	}

	public void Draw() {
		DX.DrawGraph(Game.GROUND_SIZE, _posY - Game.GROUND_SIZE, _playerImage, DX.TRUE);
	}

	public void Update() {
		// 重力を加える
		if (!IsGround()) {
			_gravity += 1;
		}
		else {
			_posY = Game.GROUND_POS;
			_gravity = 0;
		}

		// マウスの左クリックの入力を得る
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && IsGround()) {
			// ジャンプ
			_gravity = -20;
		}

		// 画像の位置を更新
		_posY += _gravity;
	}

	private bool IsGround() {
		// 地面についているかどうか
		return _posY >= Game.GROUND_POS;
	}
}
