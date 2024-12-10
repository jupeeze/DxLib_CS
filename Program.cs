#pragma warning disable CS0436

using System;
using DxLibDLL;

internal class Program
{
	// 画像のサイズ
	private static readonly int TILE_SIZE = 32;

	// 画像
	private static int[] _grounds = new int[5];

	private static void Init() {
		// ウィンドウモードで起動するように設定
		DX.ChangeWindowMode(DX.TRUE);
		// Dxlib の初期化
		DX.DxLib_Init();
		// 描画先を裏画面に設定
		DX.SetDrawScreen(DX.DX_SCREEN_BACK);
	}

	private static void Loop() {
		// メインループ
		while (DX.ProcessMessage() == 0) {
			// 画面をクリア
			DX.ClearDrawScreen();
			// 画像を描画
			Draw();
			// 文字を描画
			DX.DrawString(100, 100, "Hello World", DX.GetColor(255, 255, 255));
			// 裏画面の内容を表画面に反映
			DX.ScreenFlip();
		}
	}

	private static void Load() {
		// PNG画像のメモリへの読みこみ
		DX.LoadDivGraph(@"Sprite/Grounds.png", 5, 5, 1, 16, 16, _grounds);
	}

	private static void Draw() {
		int x = 100, y = 100;
		DrawEXGraph(x, y, _grounds[0]);
	}

	private static void DrawEXGraph(int x, int y, int GrHandle) {
		DX.DrawExtendGraph(x, y, x + TILE_SIZE, y + TILE_SIZE, GrHandle, DX.TRUE);
	}

	private static void Main(string[] args) {
		// 初期化処理
		Init();
		// 画像の読み込み
		Load();
		// メインループ
		Loop();

		// Dxlib の終了処理
		DX.DxLib_End();
	}
}
