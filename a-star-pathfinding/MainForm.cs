using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using SelfBalancedTree;


namespace a_star_pathfinding
{
	public enum State {UNDEFINED, OPENED, CLOSED,PATH};
	public enum SolveState{FIND,TRACE}
	public partial class MainForm : Form
	{
		
		private int h = 600;
		private int w = 600;
		private Cell[,] grid;
		private Cell target;
		private int m=6;
		private int gridSize;
		private AVLTree<Cell> OpenSet;
		private Cell trace;

		private SolveState solveState;
		public MainForm()
		{
			
			InitializeComponent();
			//SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered=true;
			OpenSet = new AVLTree<Cell>();
			solveState = SolveState.FIND;
			gridSize=w/m;			
			grid = new Cell[gridSize,gridSize];
			this.ClientSize = new Size(w,h);
			Random rnd = new Random();
			
			for(int i=0;i<gridSize;i++){
        		for(int k = 0;k<gridSize;k++){
					grid[i,k] = new Cell(i,k,m,(rnd.Next(1, 100)>35),new Vec2(i,k));
        		}
    		}
			
			
			target=grid[gridSize-1,gridSize-1];
		    grid[0,0].walkable=true;
		    grid[gridSize-1,gridSize-1].walkable=true;
		
		    grid[0,0].gScore=0;
		    grid[0,0].Open=State.OPENED;
		    grid[0,0].fScore=grid[0,0].hScore+grid[0,0].gScore;

		    OpenSet.Add(grid[0,0]);
		    
			tick_timer.Start();
		}
		
		void Tick_timerTick(object sender, EventArgs e)
		{
			for(int i=0;i<10;i++)
			if (solveState == SolveState.FIND) {
			
					
				Cell current;
				if(!OpenSet.GetMin(out current)){
					tick_timer.Stop();
					return;
				}
			    current.Open=State.CLOSED;
			    OpenSet.DeleteMin();
	
				List<Cell> neighbours = getNeighbours(current.i, current.k);
				neighbours.ForEach(n => {
		            int newGScore = current.gScore + Cell.d(current,n);
		            if(newGScore<n.gScore){
			            n.gScore=newGScore;
			            n.fScore=n.hScore+n.gScore;
			            n.parent=current;
			            if(n.Open==State.UNDEFINED){
			                n.Open=State.OPENED;
			                OpenSet.Add(n);
			            }
			        }
	          	});
				
				
				if(current==target){
			        solveState=SolveState.TRACE;
			        trace=target;
			        trace.Open=State.PATH;
			    }
			}else{
				if(trace.parent!=null){
					trace.parent.Open=State.PATH;
					trace = trace.parent;
				}else{
					tick_timer.Stop();
				}
				
			}
			this.Refresh();

		}
		
		private List<Cell> getNeighbours(int _i, int _k){
			List<Cell> neighbours = new List<Cell>();
		    for(int i = -1; i <= 1; i++){
		        for(int k = -1; k <= 1;k++){
		            if(i!=0 || k!=0){
						int x = i+_i;
						int y = k+_k;
		                if(x>=0&&x<gridSize&&y>=0&&y<gridSize){
							
		                    if(grid[x,y].walkable&&grid[x,y].Open!=State.CLOSED){
		                        neighbours.Add(grid[x,y]);
		                    }
		                }
		            }
		        }
		    }
		    return neighbours;
		}
		
		protected override void OnPaint( PaintEventArgs e )
 		{
			
			Graphics g = e.Graphics;
			
			g.FillRectangle(Brushes.Gray,0,ClientSize.Width,0,ClientSize.Height);
			
			for(int i=0;i<gridSize;i++){
		        for(int k = 0;k<gridSize;k++){
		            grid[i,k].show(g);
		        }
		    }
		    
			for(int i=0;i<gridSize;i++){
				g.DrawLine(Pens.Black,0,i*m,w,i*m);
				g.DrawLine(Pens.Black,i*m,0,i*m,w);
			}
			

		}

		
		void MainFormKeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar==' '){
				regenerateTerrain();
			}
		}
		
		void regenerateTerrain(){
			tick_timer.Stop();
			//SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			OpenSet = new AVLTree<Cell>();
			solveState = SolveState.FIND;
			gridSize=w/m;			
			grid = new Cell[gridSize,gridSize];
			this.ClientSize = new Size(w,h);
			Random rnd = new Random();
			
			for(int i=0;i<gridSize;i++){
        		for(int k = 0;k<gridSize;k++){
					grid[i,k] = new Cell(i,k,m,(rnd.Next(1, 100)>35),new Vec2(i,k));
        		}
    		}
			
			
			target=grid[gridSize-1,gridSize-1];
		    grid[0,0].walkable=true;
		    grid[gridSize-1,gridSize-1].walkable=true;
		
		    grid[0,0].gScore=0;
		    grid[0,0].Open=State.OPENED;
		    grid[0,0].fScore=grid[0,0].hScore+grid[0,0].gScore;

		    OpenSet.Add(grid[0,0]);
		    
			tick_timer.Start();
		}
	}
}
