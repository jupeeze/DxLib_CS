#pragma warning disable CS0436

using System;
using DxLibDLL;

internal class Program
{
	// 画面サイズ
	private static readonly int SCREEN_X = 640, SCREEN_Y = 480;

	// 画像のサイズ
	private static readonly int TILE_SIZE = 32;

	// 画像の数
	private static readonly int TILE_COUNT = SCREEN_X / TILE_SIZE;

	// タイマーの周期
	private static readonly int TIMER_INTERVAL = 16;

	// 画像
	private static int[] _grounds = new int[5];

	// 画面更新の計測用タイマー
	private static int _timer;

	private static void Init() {
		// ウィンドウモードで起動するように設定
		DX.ChangeWindowMode(DX.TRUE);
		// Dxlib の初期化
		DX.DxLib_Init();
		// 描画先を裏画面に設定
		DX.SetDrawScreen(DX.DX_SCREEN_BACK);

		// タイマーの初期化
		_timer = DX.GetNowCount();
	}

	private static void Loop() {
		int diff = 0;

		// メインループ
		while (DX.ProcessMessage() == 0) {
			// 画面をクリア
			DX.ClearDrawScreen();
			// 差分を計算
			diff += 2;
			// 画像を描画
			Draw(diff);
			// 裏画面の内容を表画面に反映
			DX.ScreenFlip();

			// 周期 16ms で待機
			_timer += TIMER_INTERVAL;
			DX.WaitTimer(Math.Max(0, _timer - DX.GetNowCount()));
		}
	}

	private static void Load() {
		// PNG画像のメモリへの読みこみ
		DX.LoadDivGraph(@"Sprite/Grounds.png", 5, 5, 1, 16, 16, _grounds);
	}

	private static void Draw(int diff) {
		int x = 0, y = 100;
		for (int i = 0; i < TILE_COUNT; i++) {
			DrawEXGraph(x + i * TILE_SIZE - diff, y, _grounds[0]);
		}
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
