using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductDetails : VshopTemplatedWebControl
	{
		private int productId;

		private VshopTemplatedRepeater rptProductImages;

		private System.Web.UI.WebControls.Literal litProdcutName;

		private System.Web.UI.WebControls.Literal litProdcutTag;

		private System.Web.UI.WebControls.Literal litSalePrice;

		private System.Web.UI.WebControls.Literal litMarketPrice;

		private System.Web.UI.WebControls.Literal litShortDescription;

		private System.Web.UI.WebControls.Literal litDescription;

		private System.Web.UI.WebControls.Literal litStock;

		private System.Web.UI.WebControls.Literal litSoldCount;

		private System.Web.UI.WebControls.Literal litConsultationsCount;

		private System.Web.UI.WebControls.Literal litReviewsCount;

		private System.Web.UI.WebControls.Literal litItemParams;

		private Common_SKUSelector skuSelector;

		private Common_ExpandAttributes expandAttr;

		private System.Web.UI.WebControls.HyperLink linkDescription;

		private System.Web.UI.HtmlControls.HtmlInputHidden litHasCollected;

		private System.Web.UI.HtmlControls.HtmlInputHidden litCategoryId;

		private System.Web.UI.HtmlControls.HtmlInputHidden litproductid;

		private System.Web.UI.HtmlControls.HtmlInputHidden litTemplate;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litProdcutTag = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutTag");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litMarketPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarketPrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.linkDescription = (System.Web.UI.WebControls.HyperLink)this.FindControl("linkDescription");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
			this.litHasCollected = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litHasCollected");
			this.litCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litCategoryId");
			this.litproductid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litproductid");
			this.litTemplate = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litTemplate");
			ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
			if (product != null)
			{
				this.litproductid.Value = this.productId.ToString();
				this.litTemplate.Value = product.FreightTemplateId.ToString();
				if (product == null)
				{
					base.GotoResourceNotFound("此商品已不存在");
				}
				if (product.SaleStatus != ProductSaleStatus.OnSale)
				{
					base.GotoResourceNotFound(ErrorType.前台商品下架, "此商品已下架");
				}
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					SlideImage[] source = new SlideImage[]
					{
						new SlideImage(product.ImageUrl1, locationUrl),
						new SlideImage(product.ImageUrl2, locationUrl),
						new SlideImage(product.ImageUrl3, locationUrl),
						new SlideImage(product.ImageUrl4, locationUrl),
						new SlideImage(product.ImageUrl5, locationUrl)
					};
					this.rptProductImages.DataSource = from item in source
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
				}
				string mainCategoryPath = product.MainCategoryPath;
				if (!string.IsNullOrEmpty(mainCategoryPath))
				{
					this.litCategoryId.Value = mainCategoryPath.Split(new char[]
					{
						'|'
					})[0];
				}
				else
				{
					this.litCategoryId.Value = "0";
				}
				string productName = product.ProductName;
				string text = ProductBrowser.GetProductTagName(this.productId);
				if (!string.IsNullOrEmpty(text))
				{
					this.litProdcutTag.Text = "<div class='y-shopicon'>" + text.Trim() + "</div>";
					text = "<span class='producttag'>【" + System.Web.HttpContext.Current.Server.HtmlEncode(text) + "】</span>";
				}
				this.litProdcutName.Text = text + productName;
				if (product.MinSalePrice != product.MaxSalePrice)
				{
					this.litSalePrice.Text = product.MinSalePrice.ToString("F2") + "~" + product.MaxSalePrice.ToString("F2");
				}
				else
				{
					this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
				}
				if (product.MarketPrice.HasValue)
				{
					if (product.MarketPrice > 0m)
					{
                       
                        this.litMarketPrice.Text = "<del class=\"text-muted font-s\">¥" + product.MarketPrice.Value.ToString("F2") + "</del>";
                        //加入显示参数：佣金...
                        //this.litMarketPrice.Text = "<del class=\"text-muted font-s\">¥" + product.MarketPrice.Value.ToString("F2") + "</del>&nbsp&nbsp<span style='color:red;'>佣金："+(product.MinShowPrice * product.SecondCommission / 100).ToString("F2")+"</span>";
                    }
                }
				this.litShortDescription.Text = product.ShortDescription;
				string text2 = product.Description;
				if (!string.IsNullOrEmpty(text2))
				{
					text2 = System.Text.RegularExpressions.Regex.Replace(text2, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + System.Web.HttpContext.Current.Server.HtmlEncode(productName) + "' src='$2' />", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				}
				if (this.litDescription != null)
				{
					this.litDescription.Text = text2;
				}
				this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
				this.litStock.Text = product.Stock.ToString();
				this.skuSelector.ProductId = this.productId;
				if (this.expandAttr != null)
				{
					this.expandAttr.ProductId = this.productId;
				}
				if (this.linkDescription != null)
				{
					this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
				}
				int num = ProductBrowser.GetProductConsultationsCount(this.productId, false);
				this.litConsultationsCount.SetWhenIsNotNull(num.ToString());
				num = ProductBrowser.GetProductReviewsCount(this.productId);
				this.litReviewsCount.SetWhenIsNotNull(num.ToString());
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				bool flag = false;
				if (currentMember != null)
				{
					flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
				}
				this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
				ProductBrowser.UpdateVisitCounts(this.productId);
				PageTitle.AddSiteNameTitle(productName);
				PageTitle.AddSiteDescription(product.ShortDescription);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string objStr = "";
				if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
				{
					objStr = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
				}
				this.litItemParams.Text = string.Concat(new string[]
				{
					Globals.GetReplaceStr(objStr, "|", "｜"),
					"|",
					Globals.GetReplaceStr(masterSettings.GoodsName, "|", "｜"),
					"|",
					Globals.GetReplaceStr(masterSettings.GoodsDescription, "|", "｜"),
					"$",
					Globals.HostPath(System.Web.HttpContext.Current.Request.Url).Replace("|", "｜"),
					Globals.GetReplaceStr(product.ImageUrl1, "|", "｜"),
					"|",
					Globals.GetReplaceStr(product.ProductName, "|", "｜"),
					"|",
					Globals.GetReplaceStr(product.ShortDescription, "|", "｜"),
					"|",
					System.Web.HttpContext.Current.Request.Url.ToString().Replace("|", "｜")
				});
			}
			else
			{
				System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
				System.Web.HttpContext.Current.Response.End();
			}
		}
	}
}
