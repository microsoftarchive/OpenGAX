using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CreateImageList
{
    class CreateImageListApp
    {
        ImageList imageList;
        const int ImageSize = 16;
        TextWriter csMappingFile;
        TextWriter ctcFile;
        int iconCounter;
        string moduleFileName;
        string moduleName;
        string baseDirectory;
        Guid guidPackage;
        IntPtr hModule;

        private TextWriter CreateTextFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }
            return File.CreateText(fileName);
        }

        public CreateImageListApp(Guid package, string moduleName, string baseDirectory, string vsVersion)
        {
            this.guidPackage = package;
            var subkeyName = @"SOFTWARE\Microsoft\VisualStudio\" + vsVersion + @"_Config\Packages\" + package.ToString("b");
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(subkeyName, false))
            {
                if (key == null)
                {
                    Console.WriteLine("ERROR! Could not find module \"{0}\" at \"HKLM\\{1}\"", moduleName, subkeyName);
                }
                else
                {
                    this.moduleFileName = (string)key.GetValue("InprocServer32");
                }
            }
            this.imageList = new ImageList();
            this.imageList.ImageSize = new Size(ImageSize, ImageSize);
            this.imageList.ColorDepth = ColorDepth.Depth24Bit;
            this.imageList.TransparentColor = Color.Transparent;
            this.baseDirectory = baseDirectory;
            this.moduleName = moduleName;
            this.csMappingFile = CreateTextFile(Path.Combine(this.baseDirectory, "ShellCmdDef.Designer." + this.moduleName + ".cs"));
            this.ctcFile = CreateTextFile(Path.Combine(this.baseDirectory, this.moduleName + ".ctc"));
            this.iconCounter = 1;
        }

        internal partial class ShellCmdDef
        {
            Dictionary<int, int> mapCSharpPackage;
            private Dictionary<int, int> MapCSharpPackage
            {
                get
                {
                    if (mapCSharpPackage == null)
                    {
                        mapCSharpPackage = new Dictionary<int, int>();
                        mapCSharpPackage.Add(4425, 1);
                    }
                    return mapCSharpPackage;
                }
            }
        }

        private void PrintGuid(Guid guid, TextWriter textWriter)
        {
            byte[] bytes = guid.ToByteArray();
            object[] oBytes = new object[bytes.Length];
            int i = 0;
            foreach (byte b in bytes)
            {
                oBytes[i++] = b;
            }
            textWriter.Write(
                String.Format(
                    CultureInfo.InvariantCulture,
                    "{{ 0x{3:X2}{2:X2}{1:X2}{0:X2}, 0x{5:X2}{4:X2}, 0x{7:X2}{6:X2}, {{ 0x{8:X2}, 0x{9:X2}, 0x{10:X2}, 0x{11:X2}, 0x{12:X2}, 0x{13:X2}, 0x{14:X2}, 0x{15:X2} }} }}",
                    oBytes
                )
            );
        }

        private void WriteHeader()
        {
            this.csMappingFile.WriteLine("namespace Microsoft.Practices.RecipeFramework.VisualStudio.CTC");
            this.csMappingFile.WriteLine("{");
            this.csMappingFile.WriteLine("	internal partial class ShellCmdDef");
            this.csMappingFile.WriteLine("	{");
            this.csMappingFile.WriteLine("		public static System.Guid guid{0}", this.moduleName);
            this.csMappingFile.WriteLine("		{");
            this.csMappingFile.WriteLine("			get");
            this.csMappingFile.WriteLine("			{");
            this.csMappingFile.WriteLine("				return new System.Guid(\"{0}\");", this.guidPackage.ToString("b"));
            this.csMappingFile.WriteLine("			}");
            this.csMappingFile.WriteLine("		}");
            this.csMappingFile.WriteLine("		private static System.Collections.Generic.Dictionary<int, int> map{0} = Map{0};", this.moduleName);
            this.csMappingFile.WriteLine("		private static System.Collections.Generic.Dictionary<int, int> Map{0}", this.moduleName);
            this.csMappingFile.WriteLine("		{");
            this.csMappingFile.WriteLine("			get");
            this.csMappingFile.WriteLine("			{");
            this.csMappingFile.WriteLine("				if (map{0} == null)", this.moduleName);
            this.csMappingFile.WriteLine("				{");
            this.csMappingFile.WriteLine("					map{0} = new System.Collections.Generic.Dictionary<int, int>();", this.moduleName);
            this.csMappingFile.WriteLine("					ProjectPackages.Add(guid{0},map{0});", this.moduleName);
            this.ctcFile.Write("        ");
            this.PrintGuid(this.guidPackage, this.ctcFile);
            this.ctcFile.WriteLine(":IDB_{0}", this.moduleName);
        }
        private void WriteFooter()
        {
            this.csMappingFile.WriteLine("				}");
            this.csMappingFile.WriteLine("				return map{0};", this.moduleName);
            this.csMappingFile.WriteLine("			}");
            this.csMappingFile.WriteLine("		}");
            this.csMappingFile.WriteLine("	}");
            this.csMappingFile.WriteLine("}");
            this.csMappingFile.Dispose();
            this.csMappingFile = null;
            this.ctcFile.WriteLine("            {0}; // End", this.iconCounter);
            this.ctcFile.Dispose();
            this.ctcFile = null;
            Console.WriteLine("Processed {0} icons for package {1}", this.imageList.Images.Count, this.moduleName);
        }

        private Bitmap Bitmap
        {
            get
            {
                if (this.imageList.Images.Count == 0)
                {
                    return null;
                }
                Bitmap bitmap = new Bitmap(imageList.Images.Count * ImageSize, ImageSize);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    int x = 0;
                    foreach (Image image in imageList.Images)
                    {
                        g.DrawImage(image, x, 0);
                        x += ImageSize;
                    }
                }
                bitmap.MakeTransparent();
                return bitmap;
            }
        }

        private int EnumRes(IntPtr hModule, NativeMethods.RT rtType, IntPtr lpName, IntPtr lParam)
        {
            try
            {
                NativeMethods.LR lr = NativeMethods.LR.LR_DEFAULTCOLOR | NativeMethods.LR.LR_VGACOLOR;
                int id = (int)NativeMethods.GET_RESOURCE_ID(lpName);
                IntPtr hIcon = NativeMethods.LoadImage(hModule,
                    lpName, NativeMethods.IMAGE.IMAGE_ICON,
                    ImageSize, ImageSize,
                    lr);
                if (hIcon != IntPtr.Zero)
                {
                    this.ctcFile.WriteLine("            /* 0x{0:X8} */ {1},", id, this.iconCounter);
                    this.csMappingFile.WriteLine("				    map{0}.Add(0x{1:X8}, 0x{2:X3});",
                        this.moduleName, id, this.iconCounter++);
                    this.imageList.Images.Add(Icon.FromHandle(hIcon));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return NativeMethods.TRUE;
        }

        public void Run()
        {
            if (this.moduleFileName == null)
            {
                return;
            }
            this.hModule = NativeMethods.LoadLibrary(this.moduleFileName);
            try
            {
                this.WriteHeader();
                //NativeMethods.EnumResourceNames(this.hModule,
                //	NativeMethods.RT.ICON,
                //	new NativeMethods.ENUMRESNAMEPROC(EnumRes),
                //	IntPtr.Zero);
                for (int i = 0; i < 5000; i++)
                {
                    this.EnumRes(hModule, NativeMethods.RT.ICON, new IntPtr(i), IntPtr.Zero);
                }
                Bitmap bitmap = this.Bitmap;
                string bitmapFileName = Path.Combine(this.baseDirectory, this.moduleName + ".bmp");
                if (File.Exists(bitmapFileName))
                {
                    File.SetAttributes(bitmapFileName, FileAttributes.Normal);
                    File.Delete(bitmapFileName);
                }
                bitmap.Save(bitmapFileName, ImageFormat.Bmp);
            }
            finally
            {
                NativeMethods.FreeLibrary(this.hModule);
                this.hModule = IntPtr.Zero;
                this.WriteFooter();
            }
        }

        static void Main(string[] args)
        {
            // Determine the Visual Studio version to use. When not specified on the command-line, use 10.0 for VS2010.
            var vsVersion = (args.Length > 3 ? args[3] : "10.0");
            CreateImageListApp app = new CreateImageListApp(new Guid(args[0]), args[1], args[2], vsVersion);
            app.Run();
        }
    }
}
