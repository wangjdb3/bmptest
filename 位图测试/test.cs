using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 位图测试
{
    class Test
    {
        public static void ReadBitmap(PaintEventArgs e)
        {
            int width = 100;
            int height = 200;
            //Bitmap bmp = new Bitmap("测试.bmp");
            ////BitmapData bitMapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            Graphics g = e.Graphics;
            //for(int i = 0; i < 50; i++)
            //{
            //    g.DrawImage(bmp, i, 0);
            //}

            //byte[] data;
            //data = Bitmap2Byte(bmp);
            //byte[] a = data.Skip(54).Take(400).ToArray();

            byte[] binchar;
            string filePath = "maichong.bin";
            FileStream Myfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binreader = new BinaryReader(Myfile);
            int file_len = (int)Myfile.Length;//获取bin文件长度
            binchar = binreader.ReadBytes(file_len);
            Myfile.Close();

            Bitmap bmp2 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData dataB = bmp2.LockBits( new Rectangle(0, 0, bmp2.Width, bmp2.Height),ImageLockMode.WriteOnly,PixelFormat.Format8bppIndexed);

            int stride = dataB.Stride;  // 扫描线的宽度
            int offset = stride - width;  // 显示宽度与扫描线宽度的间隙
            IntPtr iptr = dataB.Scan0;  // 获取bmpData的内存起始位置
            int scanBytes = stride * height;   // 用stride宽度，表示这是内存区域的大小

            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组
            int posScan = 0, posReal = 0;   // 分别设置两个位置指针，指向源数组和目标数组
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存
            for (int x = 0; x < height; x++)
            {
                //// 下面的循环节是模拟行扫描
                for (int y = 0; y < width; y++)
                {
                    pixelValues[posScan++] = binchar[posReal++];
                }
                posScan += offset;  //行扫描结束，要将目标位置指针移过那段“间隙”
            }

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, dataB.Scan0, pixelValues.Length);
            bmp2.UnlockBits(dataB);//解锁
            ColorPalette tempPalette;
            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                tempPalette = tempBmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp2.Palette = tempPalette;
            bmp2.RotateFlip(RotateFlipType.Rotate90FlipNone);
            g.DrawImage(bmp2, 0, 0);
        }
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
    }
}
