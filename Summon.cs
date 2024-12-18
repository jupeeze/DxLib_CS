using System;
using System.Collections.Generic;
using DxLibDLL;

internal class Summon
{
	private const int SIZE_X = 50, SIZE_Y = 50;
	private const int MOVE_SPEED = 10;
	private const int APPEAR_FRAMES = 6;
	private const int ACTIVE_FRAMES = 4;

	private int _posX, _posY;
	private int _currentFrame;
	private double _targetRad;
	private List<int[]> _enemyImages = new List<int[]>();

	private enum SummonState
	{ Appearing, Idle, Moving, Inactive }

	private SummonState _state = SummonState.Appearing;

	public bool IsActive => _state != SummonState.Inactive;
	public bool IsMoving => _state == SummonState.Moving;

	public void Load() {
		_state = SummonState.Appearing;
		_currentFrame = 0;

		_posX = Game.SCREEN_X - SIZE_X;
		_posY = Game.GROUND_POS - (2 * SIZE_Y);

		if (_enemyImages.Count == 0) {
			LoadSprite("sprite/Undead executioner/summonAppear.png", 3, 2);
			LoadSprite("sprite/Undead executioner/summonIdle.png", 4, 1);
		}
	}

	private void LoadSprite(string filePath, int divX, int divY) {
		int spriteCount = divX * divY;
		var sprites = new int[spriteCount];
		DX.LoadDivGraph(filePath, spriteCount, divX, divY, SIZE_X, SIZE_Y, sprites);
		_enemyImages.Add(sprites);
	}

	public void Draw() {
		int imageIndex = (_state == SummonState.Appearing) ? 0 : 1;
		DX.DrawExtendGraph(_posX, _posY, _posX - (2 * SIZE_X), _posY + (2 * SIZE_Y), _enemyImages[imageIndex][_currentFrame], DX.TRUE);
	}

	public void Animation() {
		_currentFrame++;
		switch (_state) {
			case SummonState.Appearing:
				if ((_currentFrame %= APPEAR_FRAMES) == 0) {
					_state = SummonState.Idle;
				}
				break;

			case SummonState.Idle:
			case SummonState.Moving:
				_currentFrame %= ACTIVE_FRAMES;
				break;
		}
	}

	public void Attack() {
		_targetRad = Math.Atan2(-(Player.PosY - Game.GROUND_POS), Game.SCREEN_X - SIZE_X);
		_state = SummonState.Moving;
	}

	public void Move() {
		_posX -= (int)(MOVE_SPEED * Math.Cos(_targetRad));
		_posY -= (int)(MOVE_SPEED * Math.Sin(_targetRad));

		if (_posX <= -SIZE_X) {
			_state = SummonState.Inactive;
		}
	}
}
