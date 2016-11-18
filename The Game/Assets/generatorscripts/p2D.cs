using UnityEngine;
using System.Collections;

public class p2D : MonoBehaviour {

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

	public void translateDirecional(int delta, int direction){
		switch (direction) {
		case 0:
			this.Y += delta;
			break;
		case 1:
			this.X += delta;
			break;
		case 2:
			this.Y -= delta;
			break;
		case 3:
			this.X -= delta;
			break;
		}
	}
}