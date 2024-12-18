using DxLibDLL;

internal static class Enemy
{
	private static readonly int SIZE_X = 100, SIZE_Y = 100;

	private static int _imageNum = 0;

	private static int[] _enemyImages;

	public static void Load() {
		LoadSprite("sprite/Undead executioner/idle2.png", 4, 2);
	}

	private static void LoadSprite(string filePath, int divX, int divY) {
		int spriteCount = divX * divY;
		_enemyImages = new int[spriteCount];
		DX.LoadDivGraph(filePath, spriteCount, divX, divY, SIZE_X, SIZE_Y, _enemyImages);
	}

	public static void Draw() {
		DX.DrawExtendGraph(Game.SCREEN_X - SIZE_X, Game.GROUND_POS - (2 * SIZE_Y), Game.SCREEN_X - (3 * SIZE_X), Game.GROUND_POS, _enemyImages[_imageNum], DX.TRUE);
	}

	public static void Animation() {
		_imageNum++;
		_imageNum %= 8;
	}
}
