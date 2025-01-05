using DxLibDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal static class Ground
{
	private static readonly int TILE_COUNT = Game.SCREEN_X / Game.GROUND_SIZE + 1;

	private static int[] _groundImages;

	private static int _currentTile = (Player.HitboxPosX2 - Game.Diff) / Game.GROUND_SIZE;

	public static int currentHeight => _heights[_currentTile];

	public static List<int> _heights = new List<int>();
	public static List<int> _groundTiles = new List<int>();

	private static List<int[,]> _tilemaps = new List<int[,]>();

	private static int expandedImage;

	public static void Load() {
		int x = 21, y = 15;
		int spriteCount = x * y;
		_groundImages = new int[spriteCount];
		DX.LoadDivGraph(Game.ASSET_PATH + @"oak_woods_tileset.png", spriteCount, x, y, 24, 24, _groundImages);

		expandedImage = DX.MakeScreen(Game.GROUND_SIZE, Game.GROUND_SIZE);
		DX.SetDrawScreen(expandedImage);
		DX.DrawExtendGraph(0, 0, Game.GROUND_SIZE, Game.GROUND_SIZE, _groundImages[1], DX.TRUE);
		DX.SetDrawScreen(DX.DX_SCREEN_BACK);

		for (int i = 0; i < 3; i++) {
			var tilemap = LoadTilemap(Game.ASSET_PATH + $@"tilemap{i + 1}.txt");
			_tilemaps.Add(tilemap);
			SetTilemap(tilemap);  // この関数内で _heights を設定
		}
	}

	public static void Update() {
		for (int i = 0; i < 12; i++) {
			_heights.RemoveAt(0);
		}

		_groundTiles.RemoveAt(0);

		int randomNum = new Random().Next(0, 3);
		SetTilemap(_tilemaps[randomNum]);
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

		if (currentTile < 0 || currentTile >= _heights.Count || _heights[currentTile] < 0) {
			return Game.SCREEN_Y; // 空気の場合や範囲外の場合
		}

		return Game.GROUND_POS - (_heights[currentTile] * Game.GROUND_SIZE);
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

		int groundTile = DX.MakeScreen(col * Game.GROUND_SIZE, row * Game.GROUND_SIZE, DX.TRUE);
		DX.SetDrawScreen(groundTile);

		for (int j = 0; j < col; j++) {
			// タイル列の中で最初の地形タイルの行を取得
			int height = -1;
			for (int i = 0; i < row; i++) {
				if (tilemap[i, j] != -1) {
					height = row - i - 1;  // 画面座標系に合わせる
					break;
				}
			}
			_heights.Add(height);

			for (int i = 0; i < row; i++) {
				if (tilemap[i, j] < 0) continue;

				int x = j * Game.GROUND_SIZE, y = i * Game.GROUND_SIZE;
				DX.DrawExtendGraph(x, y, x + Game.GROUND_SIZE, y + Game.GROUND_SIZE, _groundImages[tilemap[i, j]], DX.TRUE);
			}
		}

		DX.SetDrawScreen(DX.DX_SCREEN_BACK);
		_groundTiles.Add(groundTile);
	}
}
