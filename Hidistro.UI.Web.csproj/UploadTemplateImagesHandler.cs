using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using System;
using System.IO;
using System.Linq;
using System.Web;

public class UploadTemplateImagesHandler : Handler
{
	public UploadConfig UploadConfig
	{
		get;
		private set;
	}

	public UploadResult Result
	{
		get;
		private set;
	}

	public UploadTemplateImagesHandler(System.Web.HttpContext context, UploadConfig config) : base(context)
	{
		this.UploadConfig = config;
		this.Result = new UploadResult
		{
			State = UploadState.Unknown
		};
	}

	public override void Process()
    {
        byte[] array = null;
		string text = null;
		int fileSize = 0;
		if (this.UploadConfig.Base64)
		{
			text = this.UploadConfig.Base64Filename;
			array = System.Convert.FromBase64String(base.Request[this.UploadConfig.UploadFieldName]);
		}
		else
		{
			System.Web.HttpPostedFile httpPostedFile = base.Request.Files[this.UploadConfig.UploadFieldName];
			text = httpPostedFile.FileName;
			if (!this.CheckFileType(text))
			{
				this.Result.State = UploadState.TypeNotAllow;
				this.WriteResult();
				return;
			}
			if (!this.CheckFileSize(httpPostedFile.ContentLength))
			{
				this.Result.State = UploadState.SizeLimitExceed;
				this.WriteResult();
				return;
			}
			fileSize = httpPostedFile.ContentLength;
			array = new byte[httpPostedFile.ContentLength];
			try
			{
				httpPostedFile.InputStream.Read(array, 0, httpPostedFile.ContentLength);
			}
			catch (System.Exception)
			{
				this.Result.State = UploadState.NetworkError;
				this.WriteResult();
			}
		}
		this.Result.OriginFileName = text;
		string text2 = PathFormatter.Format(text, this.UploadConfig.PathFormat);
		string path = base.Server.MapPath(text2);
        string TenantID = ManagerHelper.GetCurrentTenantID();
      
        try
		{
			if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			}
			GalleryHelper.AddPhote(Globals.RequestFormNum("cate_id"), this.Result.OriginFileName, text2, fileSize, TenantID);
			System.IO.File.WriteAllBytes(path, array);
			this.Result.Url = text2;
			this.Result.State = UploadState.Success;
		}
		catch (System.Exception ex)
		{
			this.Result.State = UploadState.FileAccessError;
			this.Result.ErrorMessage = ex.Message;
		}
		finally
		{
			this.WriteResult();
		}
	}

	private void WriteResult()
	{
		base.WriteJson(new
		{
			state = this.GetStateMessage(this.Result.State),
			url = this.Result.Url,
			title = this.Result.OriginFileName,
			original = this.Result.OriginFileName,
			error = this.Result.ErrorMessage
		});
	}

	private string GetStateMessage(UploadState state)
	{
		switch (state)
		{
		case UploadState.NetworkError:
			return "????????????";
		case UploadState.FileAccessError:
			return "??????????????????????????????????????????";
		case UploadState.TypeNotAllow:
			return "????????????????????????";
		case UploadState.SizeLimitExceed:
			return "?????????????????????????????????";
		case UploadState.Success:
			return "SUCCESS";
		default:
			return "????????????";
		}
	}

	private bool CheckFileType(string filename)
	{
		string value = System.IO.Path.GetExtension(filename).ToLower();
		return (from x in this.UploadConfig.AllowExtensions
		select x.ToLower()).Contains(value);
	}

	private bool CheckFileSize(int size)
	{
		return size < this.UploadConfig.SizeLimit;
	}
}
