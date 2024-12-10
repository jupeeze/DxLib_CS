#pragma warning disable CS0436

using System;
using DxLibDLL;

internal class Game
{
	// タイマーの周期
	private const int TIMER_INTERVAL = 16;

	private Ground _ground;

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

		// タイマーの初期化
		_timer = DX.GetNowCount();
	}

	public void Load() {
		// 画像の読み込み
		_ground.Load();
	}

	private void Loop() {
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
