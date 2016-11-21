using UnityEngine;
using System.Collections;

public class p2D : Object {

	private int X;
	private int Y;

	public p2D (int x, int y) {
		this.X = x;
		this.Y = y;
	}

	public int getX() { return this.X; }
	public int getY() { return this.Y; }

	public void setX(int x) { this.X = x; }
	public void setY(int y) { this.Y = y; }
	public void setP(int x, int y) {
		this.X = x;
		this.Y = y;
	}

	public static p2D translateDirectional2(p2D coord, int delta, int direction){
		p2D translated = new p2D(int.MaxValue,int.MaxValue);
		translated.setP(coord.X, coord.Y);

		switch (direction) {
		case 0:
			translated.setY(coord.Y + delta);
			break;
		case 1:
			translated.setX(coord.X + delta);
			break;
		case 2:
			translated.setY(coord.Y - delta);
			break;
		case 3:
			translated.setX(coord.X - delta);
			break;
		default:
			//Debug.Log(direction + " " + coord.X + ", " + coord.Y + " Not translated, max,max returned");
			break;
		}
		return translated;
	}

	public override string ToString(){
		return "p:[" + this.X + ", " + this.Y + "]";		
	}

	public override bool Equals(object other){
		var that = other as p2D;
		if(that != null){
			return that.X == this.X && that.Y == this.Y;
		}
		return false;
	}
}