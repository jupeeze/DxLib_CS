﻿using DxLibDLL;
using System.Collections.Generic;

internal static class Enemy
{
	private static List<Summon> _summons = new List<Summon>();

	private static readonly int SIZE_X = 100, SIZE_Y = 100;
	private static readonly int DEFAULT_HP = 100;
	private static readonly int ATTACK_INTERVAL = 120;

	private static int _imageNum = 0;
	private static List<int[]> _enemyImages;

	private static int _attackTiming = 0;
	private static bool _isAttacking = false;

	public static bool IsActive = true;

	private static int _hp = DEFAULT_HP;

	public static readonly int PosX1 = Game.SCREEN_X - SIZE_X;
	public static readonly int PosY1 = Game.GROUND_POS - (3 * SIZE_Y);

	public static readonly int PosX2 = PosX1 - (2 * SIZE_X);
	public static readonly int PosY2 = PosY1 + (2 * SIZE_Y);

	public static readonly int HitboxPosX1 = ((PosX1 + PosX2) / 2) - 10;
	public static readonly int HitboxPosX2 = ((PosX1 + PosX2) / 2) + 30;
	public static readonly int HitboxCenterX = (HitboxPosX1 + HitboxPosX2) / 2;

	public static readonly int HitboxPosY1 = ((PosY1 + PosY2) / 2) - 10;
	public static readonly int HitboxPosY2 = ((PosY1 + PosY2) / 2) + 20;
	public static readonly int HitboxCenterY = (HitboxPosY1 + HitboxPosY2) / 2;

	public static void Load() {
		_enemyImages = new List<int[]>();
		if (_enemyImages.Count == 0) {
			int[] images;

			images = Program.LoadSprites(@"Undead executioner\idle2.png", 4, 2, SIZE_X, SIZE_Y);
			_enemyImages.Add(images);

			images = Program.LoadSprites(@"Undead executioner\summon.png", 4, 2, SIZE_X, SIZE_Y);
			_enemyImages.Add(images);
		}
	}

	public static void Update() {
		if (!IsActive) return;

		Attack();

		foreach (var summon in _summons)
			if (summon.IsMoving)
				summon.Move();
	}

	public static void Draw() {
		if (!IsActive) return;

		int imageIndex = _isAttacking ? 1 : 0;

		DX.DrawExtendGraph(PosX1, PosY1, PosX2, PosY2, _enemyImages[imageIndex][_imageNum], DX.TRUE);

		foreach (var summon in _summons)
			if (summon.IsActive)
				summon.Draw();

		uint Cr = DX.GetColor(0, 0, 255);
		DX.DrawBox(HitboxPosX1, HitboxPosY1, HitboxPosX2, HitboxPosY2, Cr, DX.FALSE);

		DX.DrawBox(50, 100, 50 + 400, 110, DX.GetColor(128, 128, 128), DX.TRUE);
		DX.DrawBox(50, 100, 50 + (_hp * 4), 110, DX.GetColor(255, 0, 0), DX.TRUE);

		AttackEffect.Draw();
	}

	public static void Animator() {
		_imageNum = 0;
	}

	public static void Animation() {
		_imageNum++;

		int maxFrame = _isAttacking ? 5 : 8;

		_imageNum %= maxFrame;

		if (_isAttacking && _imageNum == 0) {
			_isAttacking = false;

			if (Summonable()) {
				var summon = new Summon();
				summon.Load();
				_summons.Add(summon);
			}
		}

		foreach (var summon in _summons) {
			if (summon.IsActive) {
				summon.Animation();
			}
		}
	}

	public static void Attack() {
		_attackTiming++;
		if (_attackTiming % ATTACK_INTERVAL == 0) {
			_isAttacking = true;
			Animator();
		}
	}

	public static bool Summonable() {
		foreach (Summon summon in _summons) {
			if (summon.IsActive && !summon.IsMoving) {
				summon.Attack();
				return false;
			}
		}

		var inactiveSummon = _summons.Find(s => !s.IsActive);
		if (inactiveSummon != null) {
			inactiveSummon.Load();
			return false;
		}

		return true;
	}

	public static void Damage(int damage) {
		_hp -= damage;
		if (_hp <= 0) {
			_hp = 0;
			IsActive = false;
		}
	}
}
