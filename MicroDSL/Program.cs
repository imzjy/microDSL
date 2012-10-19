using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

namespace MicroDSL
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmp = CreateBitmap(400, 400);
            bmp.Save("test" + DateTime.Now.Ticks.ToString() + ".bmp");

            Console.WriteLine("generated!");
            Console.ReadKey();
        }

        static private Bitmap CreateBitmap(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                }
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                DrawContentByControlConf(g);
            }

            return bmp;
        }

        static private void DrawContentByControlConf(Graphics g)
        {
            List<string> lstCmds = LoadDrawConf();
            Console.WriteLine("draw.conf load {0} records", lstCmds.Count);

            foreach (string cmd in lstCmds)
            {
                if (cmd.ToLower().StartsWith("text"))
                {
                    Regex regex = new Regex(@"Text\(\s*(?<text>[^,]+)\s*,\s*(?<fontName>[^,]+)\s*,\s*(?<fontSize>\d+)\s*,\s*(?<xPos>\d+)\s*,\s*(?<yPos>\d+)\s*\)", RegexOptions.IgnoreCase);
                    Match m = regex.Match(cmd);
                    if (m.Success)
                    {
                        g.DrawString(m.Groups["text"].Value.Trim('\"'),
                            new Font(m.Groups["fontName"].Value.Trim('\"'), int.Parse(m.Groups["fontSize"].Value)),  //Font Name & Size
                            new SolidBrush(Color.White),
                            int.Parse(m.Groups["xPos"].Value), int.Parse(m.Groups["yPos"].Value));
                    }
                }
                else if (cmd.ToLower().StartsWith("rect"))
                {
                    Regex regex = new Regex(@"Rect\(\s*(?<xPos>\d+)\s*,\s*(?<yPos>\d+)\s*,\s*(?<width>\d+)\s*,\s*(?<height>\d+)\s*\)", RegexOptions.IgnoreCase);
                    Match m = regex.Match(cmd);
                    if (m.Success)
                    {
                        g.DrawRectangle(new Pen(Color.White),
                            int.Parse(m.Groups["xPos"].Value), int.Parse(m.Groups["yPos"].Value),
                            int.Parse(m.Groups["width"].Value), int.Parse(m.Groups["height"].Value));
                    }
                }
                else if (cmd.ToLower().StartsWith("line"))
                {
                    Regex regex = new Regex(@"Line\(\s*(?<startX>\d+)\s*,\s*(?<startY>\d+)\s*,\s*(?<endX>\d+)\s*,\s*(?<endY>\d+)\s*\)", RegexOptions.IgnoreCase);
                    Match m = regex.Match(cmd);
                    if (m.Success)
                    {
                        g.DrawLine(new Pen(Color.White),
                            int.Parse(m.Groups["startX"].Value), int.Parse(m.Groups["startY"].Value),
                            int.Parse(m.Groups["endX"].Value), int.Parse(m.Groups["endY"].Value));
                    }
                }
            }

        }

        static private List<string> LoadDrawConf()
        {
            List<string> lstCmds = new List<string>();

            try
            {
                string[] commands = File.ReadAllLines("draw.conf");
                foreach (string cmd in commands)
                {
                    if (string.IsNullOrEmpty(cmd) || cmd.Trim().StartsWith("#"))
                        continue;
                    lstCmds.Add(cmd.Trim());
                }
            }
            catch (Exception) {}
            return lstCmds;
        }

    }
}
