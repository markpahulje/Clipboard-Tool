using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace ClipboardCustomFormatEx
{
    class InfoPic
    {
        public System.Drawing.Image imgctnr { get; set; } //image container
        public string info { get; set; } //image caption
    }

    class Program
    {
        [STAThread] //TIP! Set this to get clipboard handle from a console app
        public static void Main(string[] args)
        {

            Console.WriteLine("Right-click COPY a image from your open brower. Press any key to start...");
            Console.ReadKey();

            //Let's grab clipboard - Do we have access and is there data 
            System.Windows.Forms.IDataObject iData = Clipboard.GetDataObject();

            if (iData == null)
                return;

            //check is we have standard Bitmap format available 
            if (!iData.GetDataPresent(DataFormats.Bitmap))
                return;

            Bitmap clipBMP = iData.GetData(DataFormats.Bitmap) as Bitmap;

            if (clipBMP == null)
                return;

            string imgInfo = "This image is a Bitmap " + clipBMP.PixelFormat.ToString();

            //Create on custom object to place on clibpoard. 
            InfoPic obj = new InfoPic();

            //Load object 
            obj.imgctnr = clipBMP;
            obj.info = imgInfo;

            //Some suggests we should serialize the object before placeing onto clipboard but not required

            //Set and use a custom format that represents our object with clipboard. It can be any name, using "AssemblyName"."ClassName", in this case. No need to "register" foramt ahead of time, any longer as in C using RegisterClipboardFormatA
            string myCustomFormat = "ClipboardCustomFormatEx.InfoPic";
            Clipboard.SetData(myCustomFormat, obj);

            Console.WriteLine("Success. ClipboardCustomFormatEx.InfoPic is pasted on clipboard. Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine("Okay. Let's get grab the ClipboardCustomFormatEx.InfoPic object from the clipboard.");
            
            //Let's get all data ojbects to check if clipboard is accessible and initialize object
            IDataObject clipobj = Clipboard.GetDataObject();
            if (iData == null)
                return;

            //Does custom format live on clipboard 
            if (Clipboard.ContainsData(myCustomFormat) == false)
                return;

            InfoPic myPaste = clipobj.GetData(myCustomFormat) as InfoPic; //cast is safe

            Console.WriteLine("ClipboardCustomFormatEx.info = " + myPaste.info);
            Console.WriteLine("ClipboardCustomFormatEx.imgctnr size = " + myPaste.imgctnr.Size);
            Console.WriteLine("Success. Got ClipboardCustomFormatEx.InfoPic. Press any key to repaste to overload regular formats.");
            Console.ReadKey();

            Clipboard.Clear();

            //re-paste to test using standard formats acceptable to most programs
            System.Windows.Forms.IDataObject objFormatstoPaste = new DataObject();
            objFormatstoPaste.SetData(DataFormats.Text, "Repasted Image");
            Bitmap repasteBMP = (Bitmap)myPaste.imgctnr;
            repasteBMP.RotateFlip(RotateFlipType.RotateNoneFlipY); //upside down

            objFormatstoPaste.SetData(DataFormats.Bitmap, repasteBMP);

            Clipboard.SetDataObject(objFormatstoPaste, true);

            Console.WriteLine("Success. ClipboardCustomFormatEx.InfoPic is pasted on clipboard.");
            Console.WriteLine("Open Notepad and paste (CTRL-V), you'll get text. Open MSPaint and you'll paste the image!");
            Console.ReadKey();
        }
    }
}
