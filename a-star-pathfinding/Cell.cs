using System;
using System.Drawing;

namespace a_star_pathfinding
{

	public class Vec2{
		public int x;
		public int y;
	    public Vec2(int _x, int _y){
	        this.x=_x;
	        this.y=_y;
	    }
	}
	
	public class Cell : IComparable{
	
		public State Open;
		public bool walkable;
		public int i;
		public int k;
		private Vec2 pos;
		private int w;
		public int gScore;
		public Cell parent;
		public int hScore;
		public int fScore;
	
	    public Cell(int i, int k, int w, bool walkable, Vec2 target){
	        this.Open=State.UNDEFINED;
	        this.walkable=walkable;
	        this.i=i;
	        this.k=k;
	        this.pos= new Vec2(i*w,k*w);
	        this.w=w;
	        this.gScore=100000000;
	        this.parent=null;
	        if(Math.Abs(this.k-target.x)<Math.Abs(this.i-target.y)){
	        	this.hScore=Math.Abs(this.k-target.x)*14+(Math.Abs(this.i-target.y)-Math.Abs(this.k-target.x))*10;
	        }else{
	            this.hScore=Math.Abs(this.i-target.y)*14+(Math.Abs(this.k-target.x)-Math.Abs(this.i-target.y))*10;
	        }
	        this.fScore=100000000;
	    }
	
	    public void show(Graphics g){
			if(!walkable){
				g.FillRectangle(Brushes.DarkGray,this.pos.x,this.pos.y,this.w,this.w);
				return;
			}
			
			switch (Open) {
				case State.UNDEFINED:
					g.FillRectangle(Brushes.LightGray,this.pos.x,this.pos.y,this.w,this.w);
					break;
				case State.OPENED:
					g.FillRectangle(Brushes.Green,this.pos.x,this.pos.y,this.w,this.w);
					break;
				case State.CLOSED:
					g.FillRectangle(Brushes.Red,this.pos.x,this.pos.y,this.w,this.w);
					break;
				case State.PATH:
					g.FillRectangle(Brushes.Blue,this.pos.x,this.pos.y,this.w,this.w);
					break;
				default:
					throw new Exception("Invalid value for State");
			}
			//g.DrawString(Convert.ToString(fScore), new Font("Times New Roman",12),Brushes.Black,pos.x+3,pos.y+w/2);
			
	    }
	
	    public static int d(Cell a, Cell b){
	        int i=Math.Abs(a.i-b.i);
	        int k=Math.Abs(a.k-b.k);
	        if(k+i==1){
	            return 10;
	        }
	        else{
	            return 14;
	        }
	    }
		
		public int CompareTo(object obj)
		{
			int result = this.fScore.CompareTo(((Cell)obj).fScore);
			if(result==0){
				int res = this.hScore.CompareTo(((Cell)obj).hScore);
				if(res == 0){
					return 1;	
				}else {
				return res;
				}
			}else{
				return result;
			}
		}
		public override string ToString()
		{
			return string.Format("[Cell Open={0}, I={1}, K={2}, FScore={3}]",Open,i,k,fScore);
		}

	}
	
	
}
