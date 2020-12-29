using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace ManySimpleTool.Service
{
    public class QRCodeService
    {
        public byte[] Create(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return null;
            }
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            BitMatrix bitMatrix = qrCodeWriter.encode(msg, BarcodeFormat.QR_CODE, 600, 600);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Options = new EncodingOptions()
            {
                Margin = 0,
            };
            Bitmap bitmap = barcodeWriter.Write(bitMatrix);
            return BitmapToArray(bitmap);
        }

        private byte[] BitmapToArray(Bitmap bmp)
        {
            byte[] byteArray = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                byteArray = stream.GetBuffer();
            }
            return byteArray;
        }
    }
}
