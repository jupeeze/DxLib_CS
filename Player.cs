using DxLibDLL;

internal class Player
{
	// 画像の位置
	private int _y = 300;

	// 重力
	private int _gravity = 0;

	// 画像のハンドル
	private int _playerImage;

	// 画像のサイズ
	private const int TILE_SIZE = 32;

	public void Load() {
		// PNG画像のメモリへの読みこみ
		_playerImage = DX.LoadGraph(@"sprite/ground.png");
	}

	public void Draw() {
		DX.DrawGraph(TILE_SIZE, _y - TILE_SIZE, _playerImage, DX.TRUE);
	}

	public void Update() {
		// 重力を加える
		if (!IsGround()) {
			_gravity += 1;
		}
		else {
			_y = 300;
			_gravity = 0;
		}

		// マウスの左クリックの入力を得る
		if (DX.GetMouseInput() == DX.MOUSE_INPUT_LEFT && IsGround()) {
			// ジャンプ
			_gravity = -20;
		}

		// 画像の位置を更新
		_y += _gravity;
	}

	private bool IsGround() {
		// 地面についているかどうか
		return _y >= 300;
	}
}
