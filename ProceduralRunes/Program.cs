using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralRunes
{
    class Program
    {
        // Number of points to generate the runes
        const int WIDTH_POINTS = 3;
        const int HEIGHT_POINTS = 5;

        // Distance between the points
        const int WIDTH_GAP = 20;
        const int HEIGHT_GAP = 20;

        // Distance between edge of points and svg
        const int WIDTH_BUFFER = 20;
        const int HEIGHT_BUFFER = 20;

        static void Main(string[] args)
        {
            var rnd = new Random();

            var svgWidth = (WIDTH_BUFFER * 2) + (WIDTH_GAP * (WIDTH_POINTS - 1));
            var svgHeight = (HEIGHT_BUFFER * 2) + (HEIGHT_GAP * (HEIGHT_POINTS - 1));

            var sb = new StringBuilder();
            sb.AppendLine($"<svg width=\"{svgWidth}\" height=\"{svgHeight}\" xmlns=\"http://www.w3.org/2000/svg\">");

            // Wavy Line
            // sb.AppendLine("<path d=\"M 10 80 C 40 10, 65 10, 95 80 S 150 150, 180 80\" stroke=\"black\" fill=\"transparent\" />");

            // Do Rune-y stuff in here.
            for (var i = 0; i < 10; i++)
            {
                var doThis = rnd.Next(0,100);
                int point1x, point1y, point2x, point2y;
                switch (doThis)
                {
                    case int a when a > 90:
                        #region Long Line : > 50% of Width/Height
                        // Choose a start point, choose an end point
                        point1x = rnd.Next(0, WIDTH_POINTS);
                        point1y = rnd.Next(0, HEIGHT_POINTS);
                        point2x = point1x;
                        point2y = point1y;
                        while (point2x == point1x && point2y == point1y)
                        {
                            point2x = rnd.Next(0, WIDTH_POINTS);
                            point2y = rnd.Next(0, HEIGHT_POINTS);
                        }
                        sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                            $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"black\" fill=\"transparent\" />");
                        #endregion
                        break;
                    case int a when a > 80:
                        #region Short Line : Between 2 points of Width/Height/Both
                        // Choose a start point, choose an end point                        
                        GetAdjacentPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                            $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"black\" fill=\"transparent\" />");
                        #endregion
                        break;
                    case int a when a > 70:
                        #region Tiny Line : 1/2 Line between 2 points
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        var halfx = ((point1x * WIDTH_GAP) + (point2x * WIDTH_GAP)) / 2;
                        var halfy = ((point1y * HEIGHT_GAP) + (point2y * HEIGHT_GAP)) / 2;
                        sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP) } {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                            $"L {WIDTH_BUFFER + halfx} {HEIGHT_BUFFER + halfy}\" stroke=\"black\" fill=\"transparent\" />");
                        #endregion
                        break;
                    case int a when a > 60:
                        #region Mid Point Dash : ~40% of mid path
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        var halfmdx = ((point1x * WIDTH_GAP) + (point2x * WIDTH_GAP)) / 2;
                        var halfmdy = ((point1y * HEIGHT_GAP) + (point2y * HEIGHT_GAP)) / 2;
                        if (point1x == point2x) // Vertical Line
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmdx} {HEIGHT_BUFFER + halfmdy - 5} " +
                                $"L {WIDTH_BUFFER + halfmdx} {HEIGHT_BUFFER + halfmdy + 5}\" stroke=\"black\" fill=\"transparent\" />");
                        }
                        else if (point1y == point2y) // Horizontal Line
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmdx - 5} {HEIGHT_BUFFER + halfmdy} " +
                                $"L {WIDTH_BUFFER + halfmdx + 5} {HEIGHT_BUFFER + halfmdy}\" stroke=\"black\" fill=\"transparent\" />");
                        }
                        else
                        {
                            var x2lessx1 = point2x < point1x;
                            var y2lessy1 = point2y < point1y;
                            // same comparison
                            // if (true & true) of (!false & !false)
                            if ((x2lessx1 & y2lessy1) || (!x2lessx1 & !y2lessy1)) // \
                            {
                                sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmdx - 4} {HEIGHT_BUFFER + halfmdy - 4} " +
                                    $"L {WIDTH_BUFFER + halfmdx + 4} {HEIGHT_BUFFER + halfmdy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                            }
                            else // /
                            {
                                sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmdx + 4} {HEIGHT_BUFFER + halfmdy - 4} " +
                                    $"L {WIDTH_BUFFER + halfmdx - 4} {HEIGHT_BUFFER + halfmdy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                            }
                        }
                        #endregion
                        break;
                    case int a when a > 50:
                        #region Mid Point Slash : ~40% of mid path , but perpendicular
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        var halfmsx = ((point1x * WIDTH_GAP) + (point2x * WIDTH_GAP)) / 2;
                        var halfmsy = ((point1y * HEIGHT_GAP) + (point2y * HEIGHT_GAP)) / 2;
                        if (point1x == point2x) // Vertical Line
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmsx - 5} {HEIGHT_BUFFER + halfmsy} " +
                                $"L {WIDTH_BUFFER + halfmsx + 5} {HEIGHT_BUFFER + halfmsy}\" stroke=\"black\" fill=\"transparent\" />");
                        }
                        else if (point1y == point2y) // Horizontal Line
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmsx} {HEIGHT_BUFFER + halfmsy - 5} " +
                                $"L {WIDTH_BUFFER + halfmsx} {HEIGHT_BUFFER + halfmsy + 5}\" stroke=\"black\" fill=\"transparent\" />");
                        }
                        else
                        {
                            var x2lessx1 = point2x < point1x;
                            var y2lessy1 = point2y < point1y;
                            // same comparison
                            // if (true & true) of (!false & !false)
                            if ((x2lessx1 & y2lessy1) || (!x2lessx1 & !y2lessy1)) // \
                            {
                                sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmsx + 4} {HEIGHT_BUFFER + halfmsy - 4} " +
                                    $"L {WIDTH_BUFFER + halfmsx - 4} {HEIGHT_BUFFER + halfmsy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                            }
                            else // /
                            {
                                sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + halfmsx - 4} {HEIGHT_BUFFER + halfmsy - 4} " +
                                    $"L {WIDTH_BUFFER + halfmsx + 4} {HEIGHT_BUFFER + halfmsy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                            }
                        }
                        #endregion
                        break;
                    case int a when a > 40:
                        #region Dot : Dot on a point
                        point1x = rnd.Next(0, WIDTH_POINTS);
                        point1y = rnd.Next(0, HEIGHT_POINTS);
                        sb.AppendLine($"<circle cx=\"{WIDTH_BUFFER + (point1x * WIDTH_GAP)}\" cy=\"{HEIGHT_BUFFER + (point1y * HEIGHT_GAP)}\" r=\"3\" fill=\"black\"/>");
                        #endregion
                        break;
                    case int a when a > 30:
                        #region Mid Point Dot : Dot 50% between the points
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        var halfdx = ((point1x * WIDTH_GAP) + (point2x * WIDTH_GAP)) / 2;
                        var halfdy = ((point1y * HEIGHT_GAP) + (point2y * HEIGHT_GAP)) / 2;
                        sb.AppendLine($"<circle cx=\"{WIDTH_BUFFER + (halfdx)}\" cy=\"{HEIGHT_BUFFER + (halfdy)}\" r=\"3\" fill=\"black\"/>");
                        #endregion
                        break;
                    case int a when a > 20:
                        #region Curve : Curved Line between 3 Points
                        GetDiagonalPoint(rnd, out point1x, out point1y, out point2x, out point2y);
                        int curveX, curveY;
                        if(rnd.Next(0, 2) == 0)
                        {
                            curveX = WIDTH_BUFFER + point1x * WIDTH_GAP;
                            curveY = HEIGHT_BUFFER + point2y * HEIGHT_GAP;
                        }
                        else
                        {
                            curveX = WIDTH_BUFFER + point2x * WIDTH_GAP;
                            curveY = HEIGHT_BUFFER + point1y * HEIGHT_GAP;
                        }

                        if(point1y < point2y)
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + point1x * WIDTH_GAP} {HEIGHT_BUFFER + point1y * HEIGHT_GAP} " +
                                $"Q {curveX} {curveY} " +
                                    $"{WIDTH_BUFFER + point2x * WIDTH_GAP} {HEIGHT_BUFFER + point2y * HEIGHT_GAP}\" stroke=\"black\" fill=\"transparent\"/>");
                        }
                        else
                        {
                            sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + point2x * WIDTH_GAP} {HEIGHT_BUFFER + point2y * HEIGHT_GAP} " +
                                $"Q {curveX} {curveY} " +
                                    $"{WIDTH_BUFFER + point1x * WIDTH_GAP} {HEIGHT_BUFFER + point1y * HEIGHT_GAP}\" stroke=\"black\" fill=\"transparent\"/>");
                        }
                        
                        
                        #endregion
                        break;
                }
            }

            //for (var x = 0; x < WIDTH_POINTS; x++)
            //{
            //    for (var y = 0; y < HEIGHT_POINTS; y++)
            //    {
            //        sb.AppendLine($"<circle cx=\"{WIDTH_BUFFER + (x * WIDTH_GAP)}\" cy=\"{HEIGHT_BUFFER + (y * HEIGHT_GAP)}\" r=\"2\" fill=\"red\"/>");
            //    }
            //}

            sb.AppendLine("</svg>");


            File.WriteAllText($"{DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.svg", sb.ToString());

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void GetAdjacentPoint(Random rnd, out int point1sx, out int point1sy, out int point2sx, out int point2sy)
        {
            point1sx = rnd.Next(0, WIDTH_POINTS);
            point1sy = rnd.Next(0, HEIGHT_POINTS);
            point2sx = point1sx;
            point2sy = point1sy;
            while (point2sx == point1sx && point2sy == point1sy)
            {
                switch (point1sx)
                {
                    case 0:
                        point2sx = rnd.Next(0, 2);
                        break;
                    case WIDTH_POINTS - 1:
                        point2sx = WIDTH_POINTS - rnd.Next(0, 2) - 1;
                        break;
                    default:
                        point2sx = point1sx + rnd.Next(0, 3) - 1;
                        break;
                }
                switch (point1sy)
                {
                    case 0:
                        point2sy = rnd.Next(0, 2);
                        break;
                    case HEIGHT_POINTS - 1:
                        point2sy = HEIGHT_POINTS - rnd.Next(0, 2) - 1;
                        break;
                    default:
                        point2sy = point1sy + rnd.Next(0, 3) - 1;
                        break;
                }
            }

            Console.WriteLine($"Points ({point1sx}, {point1sy}) && ({point2sx}, {point2sy})");
        }

        private static void GetDiagonalPoint(Random rnd, out int point1sx, out int point1sy, out int point2sx, out int point2sy)
        {
            point1sx = rnd.Next(0, WIDTH_POINTS);
            point1sy = rnd.Next(0, HEIGHT_POINTS);
            point2sx = point1sx;
            point2sy = point1sy;
            switch (point1sx)
            {
                case 0:
                    point2sx = 1;
                    break;
                case WIDTH_POINTS - 1:
                    point2sx = WIDTH_POINTS - 2;
                    break;
                default:
                    point2sx = point1sx + (1 * (rnd.Next(0, 2) == 0 ? -1 : 1));
                    break;
            }
            switch (point1sy)
            {
                case 0:
                    point2sy = 1;
                    break;
                case HEIGHT_POINTS - 1:
                    point2sy = HEIGHT_POINTS - 2;
                    break;
                default:
                    point2sy = point1sy + (1 * (rnd.Next(0, 2) == 0 ? -1 : 1));
                    break;
            }

            Console.WriteLine($"Points ({point1sx}, {point1sy}) && ({point2sx}, {point2sy})");
        }
    }
}
