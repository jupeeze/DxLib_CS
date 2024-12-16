using System;
using DxLibDLL;

internal class Game
{
	private Ground _ground;
	private Player _player;
	private Background _background;

	// 画面サイズ
	public static readonly int SCREEN_X = 640, SCREEN_Y = 480;

	// 1フレームの時間 (16ms)
	private const int TIMER_INTERVAL = 16;

	// 画面更新の計測用タイマー
	private int _timer;

	public Game() {
		_ground = new Ground();
		_player = new Player();
		_background = new Background();
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
		_background.Load();

		_ground.Load();
		_player.Load();
	}

	private void Loop() {
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

	private void Update() {
		_background.Update();

		_player.Update();
	}

	private void Draw() {
		// 画面をクリア
		DX.ClearDrawScreen();

		_background.Draw();

		_ground.Draw();
		_player.Draw();

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
