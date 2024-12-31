using DxLibDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal static class Ground
{
	private static readonly int TILE_COUNT = Game.SCREEN_X / Game.GROUND_SIZE + 1;

	private static int[] _groundImages;

	public static List<int> _heights = new List<int>();
	public static List<int> _groundTiles = new List<int>();

	private static List<int[,]> _tilemaps = new List<int[,]>();

	private static int expandedImage;

	public static void Load() {
		int x = 21, y = 15;
		int spriteCount = x * y;
		_groundImages = new int[spriteCount];
		DX.LoadDivGraph(Game.ASSET_PATH + @"oak_woods_tileset.png", spriteCount, x, y, 24, 24, _groundImages);

		expandedImage = DX.MakeScreen(Game.GROUND_SIZE, Game.GROUND_SIZE); // 拡大描画
		DX.SetDrawScreen(expandedImage); // 描画先を変更
		DX.DrawExtendGraph(0, 0, Game.GROUND_SIZE, Game.GROUND_SIZE, _groundImages[1], DX.TRUE); // 画像を拡大描画
		DX.SetDrawScreen(DX.DX_SCREEN_BACK); // 描画先を元に戻す

		for (int i = 0; i < 3; i++) {
			_tilemaps.Add(LoadTilemap(Game.ASSET_PATH + $@"tilemap{i + 1}.txt"));
			SetTilemap(_tilemaps[i]);
		}

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < Math.Ceiling(TILE_COUNT * 2 / 3f) - 2; j++) {
				_heights.Add(i);
			}
			for (int j = 0; j < 2; j++) {
				_heights.Add(-10);
			}
		}
	}

	public static void Update() {
		int val = _heights[0];
		_heights.RemoveAt(0);
		_heights.Add(val);
	}

	public static void Draw() {
		//DX.DrawGraph(Game.GROUND_SIZE + Game.Diff, Game.GROUND_POS - (Game.GROUND_SIZE), _groundTiles[0], DX.TRUE);

		//DX.DrawGraph(Game.Diff, Game.GROUND_POS, _groundTiles[0], DX.TRUE);

		for (int i = 0; i < 3; i++) {
			DX.DrawGraph(i * 12 * Game.GROUND_SIZE + Game.Diff, Game.SCREEN_Y - (3 * Game.GROUND_SIZE), _groundTiles[i], DX.TRUE);
		}

		//for (int i = 0; i < TILE_COUNT; i++) {
		//	if (_heights[i] < 0) continue;
		//	DX.DrawGraph(i * Game.GROUND_SIZE + Game.Diff, Game.GROUND_POS - (_heights[i] * Game.GROUND_SIZE), expandedImage, DX.TRUE);
		//}
	}

	public static int GetHeight(int x) {
		int currentTile = (x - Game.Diff) / Game.GROUND_SIZE;
		int groundHeight = Game.GROUND_POS - (Ground._heights[currentTile] * Game.GROUND_SIZE);

		return groundHeight;
	}

	private static int[,] LoadTilemap(string filePath) {
		// ファイルの内容を全て読み込む
		string[] lines = File.ReadAllLines(filePath);

		// 行数と列数を計算
		int rowCount = lines.Length;
		int colCount = lines[0].Split(',').Length;

		// 二次元配列を初期化
		int[,] array = new int[rowCount, colCount];

		// ファイルの内容を二次元配列に格納
		for (int i = 0; i < rowCount; i++) {
			int[] values = lines[i]
				.Split(',')
				.Where(x => !string.IsNullOrEmpty(x))
				.Select(int.Parse)
				.ToArray();

			for (int j = 0; j < colCount; j++) {
				array[i, j] = values[j];
			}
		}

		return array;
	}

	private static void SetTilemap(int[,] tilemap) {
		int row = tilemap.GetLength(0), col = tilemap.GetLength(1);

		int groundTile = DX.MakeScreen(col * Game.GROUND_SIZE, row * Game.GROUND_SIZE);
		DX.SetDrawScreen(groundTile);

		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				if (tilemap[i, j] < 0) continue;

				int x = j * Game.GROUND_SIZE, y = i * Game.GROUND_SIZE;
				DX.DrawExtendGraph(x, y, x + Game.GROUND_SIZE, y + Game.GROUND_SIZE, _groundImages[tilemap[i, j]], DX.TRUE);
			}
		}

		DX.SetDrawScreen(DX.DX_SCREEN_BACK);

		_groundTiles.Add(groundTile);
	}
}
