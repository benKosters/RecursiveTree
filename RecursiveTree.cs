using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreeNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     * Written as sample C# code for a CS 212 assignment -- October 2011.
     * 
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */
    class Tree
    {

        private static int TENDRILMIN = 12; // length of stems
        private static double DELTATHETA = 0.1;
        private static double SEGLENGTH = 3.0;

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Tree(double size, double redux, double turnbias, Canvas canvas)
        {
            canvas.Children.Clear();    // delete old canvas contents
                                        // draw a new fern at the center of the canvas with given parameters

            tendril((int)(canvas.Width / 2), (int)(canvas.Height - 20), size, redux, turnbias, 0, canvas);
            drawPot(canvas);
        }


        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {// this function handles the stems
            Random random = new Random();
            int x2 = x1; int y2 = y1;
            int branchx, branchy;// these are used for the starting points of each branch out of the stem,
                                 // which will become the new stems of the smaller portion the plant


            if (size > TENDRILMIN) // base case: if the size becomes too small, draw a leaf
            {
                for (int i = 0; i < TENDRILMIN; i++)
                {
                    //x2 and y2 are the next points in each segment, randomly picked(within reason)
                    direction += (random.NextDouble() > turnbias) ? DELTATHETA : -1 * DELTATHETA;
                    x2 = x1 - (int)(size / 4 * Math.Sin(direction));
                    y2 = y1 - (int)(size / 6 * Math.Cos(direction));
                    //draw the portion of the stem
                    line(x1, y1, x2, y2, 39, 61, 46, 1 + size / 80, size, redux, turnbias, direction, canvas);
                    //at every second segment, a branch on either side will be drawn. These will become the recursive subproblems
                    if (i % 2 == 0)
                    {
                        branchx = x2;
                        branchy = y2;
                        int branchsize = (int)(size / (redux));
                        //branches
                        tendril(branchx, branchy, branchsize, redux, turnbias, direction + random.NextDouble(), canvas);
                        tendril(branchx, branchy, branchsize, redux, turnbias, direction - random.NextDouble(), canvas);
                    }
                    //assign x1 and y1 to the previous endpoints so the next segment can be drawn
                    x1 = x2; y1 = y2;
                }
            }
            //if the basecase has been reached, draw a leaf
            drawLeaf(x1, y2, 5, 10, direction, canvas);
        }



        private void drawLeaf(int x, int y, double width, double height, double direction, Canvas canvas)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 50, 115, 71);// a lighter green color than the stem
            Random random = new Random();
            int leaf_prob = random.Next(0, 10);

            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = width;
            //randomly make the leaves different sizes
            if (leaf_prob > 4)
            {
                myEllipse.Height = height * 1.5;
                myEllipse.Width = width / 1.5;
            }
            else
            {
                myEllipse.Height = height;
                myEllipse.Width = width;
            }

            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        public void drawPot(Canvas canvas)
        {
            Polygon pot = new Polygon();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 94, 54, 6);
            PointCollection points = new PointCollection();
            points.Add(new Point((int)(canvas.Width / 2) + 40, (int)(canvas.Height) - 20));
            points.Add(new Point((int)(canvas.Width / 2) + 15, (int)(canvas.Height)));

            points.Add(new Point((int)(canvas.Width / 2) - 15, (int)(canvas.Height)));
            points.Add(new Point((int)(canvas.Width / 2) - 40, (int)(canvas.Height) - 20));
            pot.Points = points;
            pot.Fill = mySolidColorBrush;
            canvas.Children.Add(pot);
        }


        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            Line myLine = new Line();
            Random random = new Random();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);

        }


    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}



