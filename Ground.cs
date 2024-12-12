using DxLibDLL;

internal class Ground
{
	// 画面サイズ
	private const int SCREEN_X = 640;

	// 画像のサイズ
	private const int TILE_SIZE = 32;

	// 画像の数
	private const int TILE_COUNT = SCREEN_X / TILE_SIZE + 1;

	// 画像の移動
	private int _diff = 0;

	// 画像のハンドル
	private int _groundImage;

	public void Load() {
		// PNG画像のメモリへの読みこみ
		_groundImage = DX.LoadGraph(@"sprite/ground.png");
	}

	public void Draw() {
		int y = 300;

		// 差分を計算
		_diff -= 2;
		if (_diff <= -TILE_SIZE) _diff = 0;

		for (int i = 0; i < TILE_COUNT; i++) {
			DX.DrawGraph(i * TILE_SIZE + _diff, y, _groundImage, DX.TRUE);
		}
	}
}
