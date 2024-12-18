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

	private int HitboxPosX1 => _posX - SIZE_X - 15;
	private int HitboxPosY1 => _posY + SIZE_Y - 15;
	private int HitboxPosX2 => HitboxPosX1 + 30;
	private int HitboxPosY2 => HitboxPosY1 + 40;
	private int HitboxCenterX => (HitboxPosX1 + HitboxPosX2) / 2;
	private int HitboxCenterY => (HitboxPosY1 + HitboxPosY2) / 2;

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

		if (OnCollision(Player.HitboxPosX1, Player.HitboxPosY1, Player.HitboxPosX2, Player.HitboxPosY2)) {
			_state = SummonState.Inactive;
			Player.Die();
		}
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
		_targetRad = Math.Atan2(Player.HitboxCenterY - HitboxCenterY, Player.HitboxCenterX - HitboxCenterX);
		_state = SummonState.Moving;
	}

	public void Move() {
		_posX += (int)(MOVE_SPEED * Math.Cos(_targetRad));
		_posY += (int)(MOVE_SPEED * Math.Sin(_targetRad));

		if (_posX <= -SIZE_X) {
			_state = SummonState.Inactive;
		}
	}

	private bool OnCollision(int x1, int y1, int x2, int y2) {
		if ((HitboxPosX1 <= x1 && x1 <= HitboxPosX2) || (HitboxPosX1 <= x2 && x2 <= HitboxPosX2))
			if ((HitboxPosY1 <= y1 && y1 <= HitboxPosY2) || (HitboxPosY1 <= y2 && y2 <= HitboxPosY2))
				return true;

		return false;
	}
}
