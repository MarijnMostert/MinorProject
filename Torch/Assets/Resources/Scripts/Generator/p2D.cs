using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public override bool Equals(System.Object other) {
		if (other == null) {
			return false;
		}
		p2D that = (p2D)other;
		if (that == null) {
			return false;
		}
		return (that.X == this.X && that.Y == this.Y);
	}

	public bool Equals(p2D other) {
		if ((object)other == null) {
			return false;
		}
		return (other.getX() == this.X && other.getY() == this.Y);
	}

	public override int GetHashCode () {
		int hash = 23;
		hash = hash * 31 + this.X;
		hash = hash * 31 + this.Y;
		return hash;
	}

	public static bool myContains(List<p2D> list, p2D point) {
		return myIndexOf (list, point) != -1;
	}

	public static int myIndexOf(List<p2D> list, p2D point) {
		int i = 0;
		foreach (p2D item in list) {
			if (point.Equals (item)) {
				return i;
			}
			i++;
		}
		return -1;
	}

	public static void myRemove (List<p2D> list, List<p2D> marked) {
		foreach (p2D item in marked) {
			int index = myIndexOf (list, item);
			list.RemoveAt (index);
		}
	}

}