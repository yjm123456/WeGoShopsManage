using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace WebSite
{
    public partial class QrCodeCreate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url= Request.Params["Url"].ToString().Trim();
            string Name = Request.Params["Name"].ToString().Trim();
            Response.Write(GetDimensionalCode(url,Name));
        }

        private string GetDimensionalCode(string link, string Name)

        {

            Bitmap bmp = null;

            string path = Server.MapPath("~/Admin/QrCodeImg/" + Name + ".png");
            try

            {

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

                qrCodeEncoder.QRCodeScale = 4;

                //int version = Convert.ToInt16(cboVersion.Text);

                qrCodeEncoder.QRCodeVersion = 7;

                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                bmp = qrCodeEncoder.Encode(link);
                bmp.Save(path);

            }

            catch (Exception ex)

            {
               return "Fail";

            }

            return "Success";

        }
    }
}