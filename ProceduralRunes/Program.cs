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

            //for (var x = 0; x < WIDTH_POINTS; x++)
            //{
            //    for (var y = 0; y < HEIGHT_POINTS; y++)
            //    {
            //        sb.AppendLine(DrawCircle(x * WIDTH_GAP, y * HEIGHT_GAP));
            //    }
            //}

            // Wavy Line
            // sb.AppendLine("<path d=\"M 10 80 C 40 10, 65 10, 95 80 S 150 150, 180 80\" stroke=\"black\" fill=\"transparent\" />");

            // Do Rune-y stuff in here.
            //PureRandom(rnd, sb);

            // Create a List of Points
            var points = new List<Tuple<int, int>>();
            var vertical = new List<int>() { 0, 1 };

            // Create a Long Line
            vertical = vertical.TakeRandom(out var point1Y, rnd).ToList();
            points.Add(
                new Tuple<int, int>(
                    new int[] { 0, 1, 2 }.Random(rnd),
                    point1Y
                )
            );
            var length = rnd.Next(2, 4);
            for (var i = 1; i <= length; i++)
            {
                points.Add(
                new Tuple<int, int>(
                    points[0].Item1,
                    point1Y + i
                )
            );
            }
            var lastPoint = points.Last();
            sb.AppendLine(DrawLine(points[0].Item1 * WIDTH_GAP, points[0].Item2 * HEIGHT_GAP, lastPoint.Item1 * WIDTH_GAP, lastPoint.Item2 * HEIGHT_GAP));

            // Choose a random point
            for(int p = 0; p < 5; p++)
            {
                var options = new List<string>() 
                {
                    "curve",
                    "dot",
                    "middot",
                    "middash",
                    "midslash",
                };
                // Start From a Random Point
                var randomPoint = points.Random(rnd);
                switch (options.Random(rnd))
                {
                    case "curve":
                        GetDiagonalPoint(rnd, randomPoint.Item1, randomPoint.Item2, out var curveX, out var curveY);
                        Console.WriteLine($"Curve: {randomPoint.Item1}, {randomPoint.Item2} -> {curveX}, {curveY}");
                        sb.AppendLine(DrawCurve(rnd, randomPoint.Item1 * WIDTH_GAP, randomPoint.Item2 * HEIGHT_GAP, curveX * WIDTH_GAP, curveY * HEIGHT_GAP));
                        points.Add(new Tuple<int, int>(curveX, curveY));
                        break;
                    case "dot":
                        Console.WriteLine($"Dot: {randomPoint.Item1}, {randomPoint.Item2}");
                        sb.AppendLine(DrawCircle(randomPoint.Item1 * WIDTH_GAP, randomPoint.Item2 * HEIGHT_GAP, 3, "black"));
                        break;
                    case "middot":
                        GetAdjacentPoint(rnd, randomPoint.Item1, randomPoint.Item2, out var point2x, out var point2y);
                        GetHalfWayPoint(randomPoint.Item1, randomPoint.Item2, point2x, point2y, out var halfx, out var halfy);
                        Console.WriteLine($"Mid-Dot: {randomPoint.Item1}, {randomPoint.Item2} -> {point2x}, {point2y}");
                        sb.AppendLine(DrawCircle((int)(halfx * WIDTH_GAP), (int)(halfy * HEIGHT_GAP), 3, "black"));
                        break;
                    case "middash":
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, randomPoint.Item1, randomPoint.Item2, out point2x, out point2y);
                        Console.WriteLine($"Mid-Dash: {randomPoint.Item1}, {randomPoint.Item2} -> {point2x}, {point2y}");
                        sb.AppendLine(DrawMidPointDash(randomPoint.Item1 * WIDTH_GAP, randomPoint.Item2 * HEIGHT_GAP, point2x * WIDTH_GAP, point2y * HEIGHT_GAP));
                        break;
                    case "midslash":
                        // Choose a start point, choose an end point
                        GetAdjacentPoint(rnd, randomPoint.Item1, randomPoint.Item2, out point2x, out point2y);
                        Console.WriteLine($"Mid-Slash: {randomPoint.Item1}, {randomPoint.Item2} -> {point2x}, {point2y}");
                        sb.AppendLine(DrawMidPointSlash(randomPoint.Item1 * WIDTH_GAP, randomPoint.Item2 * HEIGHT_GAP, point2x * WIDTH_GAP, point2y * HEIGHT_GAP));
                        break;
                }


            }

            foreach (var point in points)
            {
                sb.AppendLine(DrawCircle(point.Item1 * WIDTH_GAP, point.Item2 * HEIGHT_GAP, 0.45f, "black"));
                Console.WriteLine($"{point.Item1}, {point.Item2}");
            }

            sb.AppendLine("</svg>");



            File.WriteAllText($"{DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.svg", sb.ToString());

            Console.WriteLine("Finished");
            Console.ReadLine();
        }


        private static void PureRandom(Random rnd, StringBuilder sb)
        {
            for (var i = 0; i < 10; i++)
            {
                var doThis = rnd.Next(0, 100);
                int point1x, point1y, point2x, point2y;
                float halfx, halfy;
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
                        sb.AppendLine(DrawLine(point1x * WIDTH_GAP, point1y, point2x * WIDTH_GAP, point2y));
                        #endregion
                        break;
                    case int a when a > 80:
                        #region Short Line : Between 2 points of Width/Height/Both
                        // Choose a start point, choose an end point                        
                        GetRandomAdjacentPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        sb.AppendLine(DrawLine(point1x, point1y, point2x, point2y));
                        #endregion
                        break;
                    case int a when a > 70:
                        #region Tiny Line : 1/2 Line between 2 points
                        // Choose a start point, choose an end point
                        GetRandomAdjacentPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        GetHalfWayPoint(point1x, point1y, point2x, point2y, out halfx, out halfy);
                        sb.AppendLine(DrawLine(point1x * WIDTH_GAP, point1y * HEIGHT_GAP, (int)(halfx * WIDTH_GAP), (int)(halfy * HEIGHT_GAP)));
                        #endregion
                        break;
                    case int a when a > 60:
                        #region Mid Point Dash : ~40% of mid path
                        // Choose a start point, choose an end point
                        GetRandomAdjacentPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        sb.AppendLine(DrawMidPointDash(point1x, point1y, point2x, point2y));
                        #endregion
                        break;
                    case int a when a > 50:
                        #region Mid Point Slash : ~40% of mid path , but perpendicular
                        // Choose a start point, choose an end point
                        GetRandomAdjacentPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        sb.AppendLine(DrawMidPointSlash(point1x * WIDTH_GAP, point1y * HEIGHT_GAP, point2x * WIDTH_GAP, point2y * HEIGHT_GAP));
                        #endregion
                        break;
                    case int a when a > 40:
                        #region Dot : Dot on a point
                        point1x = rnd.Next(0, WIDTH_POINTS);
                        point1y = rnd.Next(0, HEIGHT_POINTS);
                        sb.AppendLine(DrawCircle((int)(point1x * WIDTH_GAP), (int)(point1y * HEIGHT_GAP), 3, "black"));
                        #endregion
                        break;
                    case int a when a > 30:
                        #region Mid Point Dot : Dot 50% between the points
                        // Choose a start point, choose an end point
                        GetRandomAdjacentPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        //sb.AppendLine($"<path d=\"M {WIDTH_BUFFER + (point1x * WIDTH_GAP)} {HEIGHT_BUFFER + (point1y * HEIGHT_GAP)} " +
                        //    $"L {WIDTH_BUFFER + (point2x * WIDTH_GAP)} {HEIGHT_BUFFER + (point2y * HEIGHT_GAP)}\" stroke=\"red\" fill=\"transparent\" />");
                        GetHalfWayPoint(point1x, point1y, point2x, point2y, out halfx, out halfy);
                        sb.AppendLine(DrawCircle((int)(halfx * WIDTH_GAP), (int)(halfy * HEIGHT_GAP), 3, "black"));
                        #endregion
                        break;
                    case int a when a > 20:
                        #region Curve : Curved Line between 3 Points
                        GetRandomDiagonalPoints(rnd, out point1x, out point1y, out point2x, out point2y);
                        sb.AppendLine(DrawCurve(rnd, point1x, point1y, point2x, point2y));
                        #endregion
                        break;
                }
            }
        }
        private static void GetHalfWayPoint(int point1x, int point1y, int point2x, int point2y, out float halfx, out float halfy)
        {
            halfx = (point1x + (float)point2x) / 2f;
            halfy = (point1y + (float)point2y) / 2f;
        }

        private static string DrawLine(int point1x, int point1y, int point2x, int point2y)
        {
            return $"<path d=\"M {WIDTH_BUFFER + point1x} {HEIGHT_BUFFER + (point1y)} " +
                                        $"L {WIDTH_BUFFER + (point2x)} {HEIGHT_BUFFER + (point2y)}\" stroke=\"black\" fill=\"transparent\" />";
        }
        private static string DrawCircle(int x, int y, float size = 2f, string colour = "red")
        {
            return ($"<circle cx=\"{WIDTH_BUFFER + x}\" cy=\"{HEIGHT_BUFFER + y}\" r=\"{size}\" fill=\"{colour}\"/>");
        }
        private static string DrawMidPointDash(int point1x, int point1y, int point2x, int point2y)
        {
            GetHalfWayPoint(point1x, point1y, point2x, point2y, out var halfmdx, out var halfmdy);
            if (point1x == point2x) // Vertical Line
            {
                return ($"<path d=\"M {WIDTH_BUFFER + (halfmdx)} {HEIGHT_BUFFER + (halfmdy) - 5} " +
                    $"L {WIDTH_BUFFER + (halfmdx)} {HEIGHT_BUFFER + (halfmdy) + 5}\" stroke=\"black\" fill=\"transparent\" />");
            }
            else if (point1y == point2y) // Horizontal Line
            {
                return ($"<path d=\"M {WIDTH_BUFFER + halfmdx - 5} {HEIGHT_BUFFER + halfmdy} " +
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
                    return ($"<path d=\"M {WIDTH_BUFFER + halfmdx - 4} {HEIGHT_BUFFER + halfmdy - 4} " +
                        $"L {WIDTH_BUFFER + halfmdx + 4} {HEIGHT_BUFFER + halfmdy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                }
                else // /
                {
                    return ($"<path d=\"M {WIDTH_BUFFER + halfmdx + 4} {HEIGHT_BUFFER + halfmdy - 4} " +
                        $"L {WIDTH_BUFFER + halfmdx - 4} {HEIGHT_BUFFER + halfmdy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                }
            }
        }
        private static string DrawMidPointSlash(int point1x, int point1y, int point2x, int point2y)
        {
            GetHalfWayPoint(point1x, point1y, point2x, point2y, out var halfmsx, out var halfmsy);
            if (point1x == point2x) // Vertical Line
            {
                return ($"<path d=\"M {WIDTH_BUFFER + halfmsx - 5} {HEIGHT_BUFFER + halfmsy} " +
                    $"L {WIDTH_BUFFER + halfmsx + 5} {HEIGHT_BUFFER + halfmsy}\" stroke=\"black\" fill=\"transparent\" />");
            }
            else if (point1y == point2y) // Horizontal Line
            {
                return ($"<path d=\"M {WIDTH_BUFFER + halfmsx} {HEIGHT_BUFFER + halfmsy - 5} " +
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
                    return ($"<path d=\"M {WIDTH_BUFFER + halfmsx + 4} {HEIGHT_BUFFER + halfmsy - 4} " +
                        $"L {WIDTH_BUFFER + halfmsx - 4} {HEIGHT_BUFFER + halfmsy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                }
                else // /
                {
                    return ($"<path d=\"M {WIDTH_BUFFER + halfmsx - 4} {HEIGHT_BUFFER + halfmsy - 4} " +
                        $"L {WIDTH_BUFFER + halfmsx + 4} {HEIGHT_BUFFER + halfmsy + 4}\" stroke=\"black\" fill=\"transparent\" />");
                }
            }
        }
        private static string DrawCurve(Random rnd, int point1x, int point1y, int point2x, int point2y)
        {
            int curveX, curveY;
            if (rnd.Next(0, 2) == 0)
            {
                curveX = WIDTH_BUFFER + point1x;
                curveY = HEIGHT_BUFFER + point2y;
            }
            else
            {
                curveX = WIDTH_BUFFER + point2x;
                curveY = HEIGHT_BUFFER + point1y;
            }

            if (point1y < point2y)
            {
                return ($"<path d=\"M {WIDTH_BUFFER + point1x} {HEIGHT_BUFFER + point1y} " +
                    $"Q {curveX} {curveY} " +
                        $"{WIDTH_BUFFER + point2x} {HEIGHT_BUFFER + point2y}\" stroke=\"black\" fill=\"transparent\"/>");
            }
            else
            {
                return ($"<path d=\"M {WIDTH_BUFFER + point2x} {HEIGHT_BUFFER + point2y} " +
                    $"Q {curveX} {curveY} " +
                        $"{WIDTH_BUFFER + point1x} {HEIGHT_BUFFER + point1y}\" stroke=\"black\" fill=\"transparent\"/>");
            }
        }


        private static void GetRandomAdjacentPoints(Random rnd, out int point1sx, out int point1sy, out int point2sx, out int point2sy)
        {
            point1sx = rnd.Next(0, WIDTH_POINTS);
            point1sy = rnd.Next(0, HEIGHT_POINTS);
            GetAdjacentPoint(rnd, point1sx, point1sy, out point2sx, out point2sy);

            Console.WriteLine($"Points ({point1sx}, {point1sy}) && ({point2sx}, {point2sy})");
        }
        private static void GetRandomDiagonalPoints(Random rnd, out int point1sx, out int point1sy, out int point2sx, out int point2sy)
        {
            point1sx = rnd.Next(0, WIDTH_POINTS);
            point1sy = rnd.Next(0, HEIGHT_POINTS);
            GetDiagonalPoint(rnd, point1sx, point1sy, out point2sx, out point2sy);

            Console.WriteLine($"Points ({point1sx}, {point1sy}) && ({point2sx}, {point2sy})");
        }



        private static void GetAdjacentPoint(Random rnd, int point1sx, int point1sy, out int point2sx, out int point2sy)
        {
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
        }
        private static void GetDiagonalPoint(Random rnd, int point1sx, int point1sy, out int point2sx, out int point2sy)
        {
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
        }
    }
}
