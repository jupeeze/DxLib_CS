using System;
using DxLibDLL;

internal static class Game
{
	private static int _diff = 0;

	public static int Diff => _diff;

	public static readonly int GROUND_POS = 400;

	public static readonly int GROUND_SIZE = 32;

	// 画面サイズ
	public static readonly int SCREEN_X = 640, SCREEN_Y = 480;

	// 1フレームの時間 (16ms)
	private static readonly int TIMER_INTERVAL = 16;

	// 画面更新の計測用タイマー
	private static int _timer;

	public static void Run() {
		// 初期化処理
		Init();
		// 画像の読み込み
		Load();
		// メインループ
		Loop();

		// Dxlib の終了処理
		DX.DxLib_End();
	}

	private static void Init() {
		// ウィンドウモードで起動するように設定
		DX.ChangeWindowMode(DX.TRUE);
		// Dxlib の初期化
		DX.DxLib_Init();
		// 描画先を裏画面に設定
		DX.SetDrawScreen(DX.DX_SCREEN_BACK);
	}

	private static void Load() {
		Background.Load();

		Ground.Load();
		Player.Load();
		Enemy.Load();
	}

	private static void Loop() {
		// 現在の時間を取得
		_timer = DX.GetNowCount();

		// メインループ
		while (DX.ProcessMessage() == 0) {
			// 更新処理
			Update();
			// 画像を描画
			Draw();

			// 周期 16ms で待機
			_timer += TIMER_INTERVAL;
			DX.WaitTimer(Math.Max(0, _timer - DX.GetNowCount()));
		}
	}

	private static void Update() {
		// 差分を計算
		_diff -= 5;
		if (_diff <= -GROUND_SIZE) {
			_diff %= -GROUND_SIZE;

			Animation();
		}

		Background.Update();

		Player.Update();
	}

	private static void Animation() {
		Player.Animation();
		Enemy.Animation();
	}

	private static void Draw() {
		// 画面をクリア
		DX.ClearDrawScreen();

		Background.Draw();

		Ground.Draw();
		Player.Draw();
		Enemy.Draw();

		// 裏画面の内容を表画面に反映
		DX.ScreenFlip();
	}
}

internal class Program
{
	private static void Main(string[] args) {
		Game.Run();
	}
}
