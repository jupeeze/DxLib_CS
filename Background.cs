using DxLibDLL;

internal static class Background
{
	private static readonly int SIZE_X = 576, SIZE_Y = 324;

	private static int[] _positionsX;

	private static int[] _imagesSky, _imagesCloud;

	public static void Load() {
		_imagesSky = new int[2];
		for (int i = 0; i < _imagesSky.Length; i++) {
			_imagesSky[i] = DX.LoadGraph(Game.ASSET_PATH + $@"Clouds\sky_{i + 1}.png");
		}

		_imagesCloud = new int[4];
		for (int i = 0; i < _imagesCloud.Length; i++) {
			_imagesCloud[i] = DX.LoadGraph(Game.ASSET_PATH + $@"Clouds\{i + 1}.png");
		}

		_positionsX = new int[_imagesCloud.Length];
	}

	public static void Update() {
		for (int i = 0; i < _imagesCloud.Length; i++) {
			_positionsX[i] -= i + 1;
			_positionsX[i] %= SIZE_X;
		}
	}

	public static void Draw() {
		for (int i = 0; i < _imagesSky.Length; i++) {
			DX.DrawExtendGraph(0, 0, Game.SCREEN_X, Game.GROUND_POS, _imagesSky[i], DX.TRUE);
		}

		for (int i = 0; i < _imagesCloud.Length; i++) {
			DrawLayer(_positionsX[i], i);
		}
	}

	private static void DrawLayer(int posX, int imageIndex) {
		for (int i = 0; i < 2 + (Game.SCREEN_X / SIZE_X); i++) {
			DX.DrawGraph(posX + (i * SIZE_X), Game.GROUND_POS - SIZE_Y, _imagesCloud[imageIndex], DX.TRUE);
		}
	}
}
