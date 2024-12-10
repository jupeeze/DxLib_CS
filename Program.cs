#pragma warning disable CS0436

using System;
using DxLibDLL;

internal class Program
{
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
			// 文字を描画
			DX.DrawString(100, 100, "Hello World", DX.GetColor(255, 255, 255));
			// 裏画面の内容を表画面に反映
			DX.ScreenFlip();
		}
	}

	private static void Main(string[] args) {
		// 初期化処理
		Init();
		// メインループ
		Loop();

		// Dxlib の終了処理
		DX.DxLib_End();
	}
}
