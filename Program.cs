using System;
using DxLibDLL;

internal class Game
{
	private Ground _ground;

	// 1フレームの時間 (16ms)
	private const int TIMER_INTERVAL = 16;

	// 画面更新の計測用タイマー
	private int _timer;

	public Game() {
		_ground = new Ground();
	}

	public void Run() {
		// 初期化処理
		Init();
		// 画像の読み込み
		Load();
		// メインループ
		Loop();

		// Dxlib の終了処理
		DX.DxLib_End();
	}

	private void Init() {
		// ウィンドウモードで起動するように設定
		DX.ChangeWindowMode(DX.TRUE);
		// Dxlib の初期化
		DX.DxLib_Init();
		// 描画先を裏画面に設定
		DX.SetDrawScreen(DX.DX_SCREEN_BACK);
	}

	private void Load() {
		// 画像の読み込み
		_ground.Load();
	}

	private void Loop() {
		// 現在の時間を取得
		_timer = DX.GetNowCount();

		// メインループ
		while (DX.ProcessMessage() == 0) {
			// 画像を描画
			Draw();

			// 周期 16ms で待機
			_timer += TIMER_INTERVAL;
			DX.WaitTimer(Math.Max(0, _timer - DX.GetNowCount()));
		}
	}

	private void Draw() {
		// 画面をクリア
		DX.ClearDrawScreen();

		// 地面を描画
		_ground.Draw();

		// 裏画面の内容を表画面に反映
		DX.ScreenFlip();
	}
}

internal class Program
{
	private static void Main(string[] args) {
		Game game = new Game();
		game.Run();
	}
}
