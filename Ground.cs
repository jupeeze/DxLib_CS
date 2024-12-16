using DxLibDLL;

internal class Ground
{
	// 画像の数
	private readonly int TILE_COUNT = Game.SCREEN_X / Game.GROUND_SIZE + 1;

	// 画像のハンドル
	private int _groundImage;

	public void Load() {
		// PNG画像のメモリへの読みこみ
		_groundImage = DX.LoadGraph(@"sprite/ground.png");
	}

	public void Draw() {
		for (int i = 0; i < TILE_COUNT; i++) {
			DX.DrawGraph(i * Game.GROUND_SIZE + Game.Diff, Game.GROUND_POS, _groundImage, DX.TRUE);
		}
	}
}
