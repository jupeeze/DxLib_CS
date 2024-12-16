using DxLibDLL;

internal class Background
{
	private const int SIZE_X = 576, SIZE_Y = 324;

	private int[] _positionsX;

	private int[] _imagesSky, _imagesCloud;

	public void Load() {
		_imagesSky = new int[2];
		for (int i = 0; i < _imagesSky.Length; i++) {
			_imagesSky[i] = DX.LoadGraph($@"sprite/Clouds/sky_{i + 1}.png");
		}

		_imagesCloud = new int[4];
		for (int i = 0; i < _imagesCloud.Length; i++) {
			_imagesCloud[i] = DX.LoadGraph($@"sprite/Clouds/{i + 1}.png");
		}

		_positionsX = new int[_imagesCloud.Length];
	}

	public void Update() {
		for (int i = 0; i < _imagesCloud.Length; i++) {
			_positionsX[i] -= i + 1;
			_positionsX[i] %= SIZE_X;
		}
	}

	public void Draw() {
		for (int i = 0; i < _imagesSky.Length; i++) {
			DX.DrawExtendGraph(0, 0, Game.SCREEN_X, Game.SCREEN_Y, _imagesSky[i], DX.TRUE);
		}

		for (int i = 0; i < _imagesCloud.Length; i++) {
			DrawLayer(_positionsX[i], i);
		}
	}

	private void DrawLayer(int posX, int imageIndex) {
		for (int i = 0; i < 3; i++) {
			DX.DrawGraph(posX + (i * SIZE_X), Game.SCREEN_Y - SIZE_Y, _imagesCloud[imageIndex], DX.TRUE);
		}
	}
}
