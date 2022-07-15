using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create Attack")]
public class Attack : ScriptableObject{
	[SerializeField]
	private int length;
	[SerializeField]
	private int width;
	[SerializeField]
	private int damage;

	public Attack(int _l, int _w, int _d){
		length = _l;
		width = _w;
		damage = _d;
	}

	public int GetDamage(){ return damage;}
	public int[] GetRange(){ return new int[2]{length, width};}
}
