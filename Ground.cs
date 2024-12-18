using DxLibDLL;

internal static class Ground
{
	// 画像の数
	private static readonly int TILE_COUNT = Game.SCREEN_X / Game.GROUND_SIZE + 1;

	// 画像のハンドル
	private static int _groundImage;

	public static void Load() {
		// PNG画像のメモリへの読みこみ
		_groundImage = DX.LoadGraph(@"sprite/ground.png");
	}

	public static void Draw() {
		for (int i = 0; i < TILE_COUNT; i++) {
			DX.DrawGraph(i * Game.GROUND_SIZE + Game._diff, Game.GROUND_POS, _groundImage, DX.TRUE);
		}
	}
}
