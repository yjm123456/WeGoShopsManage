using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnSales : AdminPage
	{
		private string productName;

		private string productCode;

		private int? categoryId;

		private int? tagId;

		private int? typeId;

		private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;

		private System.DateTime? startDate;

		private System.DateTime? endDate;

		protected string LocalUrl = string.Empty;

		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected System.Web.UI.WebControls.Literal LitOnSale;

		protected System.Web.UI.WebControls.Literal LitOnStock;

		protected System.Web.UI.WebControls.Literal LitZero;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected System.Web.UI.WebControls.TextBox txtSKU;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnDelete;

		protected System.Web.UI.WebControls.HyperLink btnDownTaobao;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected System.Web.UI.WebControls.Button btnUnSale;

		protected System.Web.UI.WebControls.Button btnSetFreeShip;

		protected FreightTemplateDownList FreightTemplateDownList1;

		protected System.Web.UI.WebControls.Button BtnTemplate;

		protected ProductTagsLiteral litralProductTag;

		protected System.Web.UI.WebControls.Button btnUpdateProductTags;

		protected TrimTextBox txtProductTag;

		protected System.Web.UI.WebControls.Button btnInStock;

		protected System.Web.UI.WebControls.Button btnUpSale;

		protected System.Web.UI.WebControls.Button btnCancelFreeShip;

		protected ProductOnSales() : base("m02", "spp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.BtnTemplate.Click += new System.EventHandler(this.BtnTemplate_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnUpSale.Click += new System.EventHandler(this.btnUpSale_Click);
			this.btnUnSale.Click += new System.EventHandler(this.btnUnSale_Click);
			this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
			this.btnCancelFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
			this.btnSetFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
			this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
			this.grdProducts.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.IsUnclassified = true;
                //??????????????????
				this.dropCategories.DataBind();
                //????????????
				this.dropBrandList.DataBind();
				this.FreightTemplateDownList1.DataBind();
				this.litralProductTag.DataBind();
				this.btnDownTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", base.Request.Url.Host, Globals.ApplicationPath);
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}

		private void btnSetFreeShip_Click(object sender, System.EventArgs e)
		{
			bool flag = ((System.Web.UI.WebControls.Button)sender).ID == "btnSetFreeShip";
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("???????????????????????????????????????", false);
				return;
			}
			int num = ProductHelper.SetFreeShip(text, flag);
			if (num > 0)
			{
				this.ShowMsg("??????" + (flag ? "??????" : "??????") + "?????????????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg((flag ? "??????" : "??????") + "???????????????????????????????????????", false);
		}

		private void BtnTemplate_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("???????????????????????????????????????", false);
				return;
			}
			if (string.IsNullOrEmpty(this.FreightTemplateDownList1.SelectedValue.ToString()) || this.FreightTemplateDownList1.SelectedValue == 0)
			{
				this.ShowMsg("?????????????????????", false);
				return;
			}
			int num = ProductHelper.UpdateProductFreightTemplate(text, int.Parse(this.FreightTemplateDownList1.SelectedValue.ToString()));
			if (num > 0)
			{
				this.ShowMsg("????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("????????????", false);
		}

		private void btnCancelFreeShip_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
				if (literal.Text == "1")
				{
					literal.Text = "?????????";
				}
				else if (literal.Text == "2")
				{
					literal.Text = "?????????";
				}
				else
				{
					literal.Text = "?????????";
				}
				if (string.IsNullOrEmpty(literal2.Text))
				{
					literal2.Text = "-";
				}
			}
		}

		private void grdProducts_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (ProductHelper.RemoveProduct(e.CommandArgument.ToString()) > 0)
				{
					try
					{
						this.myNotifier.updateAction = UpdateAction.ProductUpdate;
						this.myNotifier.actionDesc = "??????????????????";
						this.myNotifier.RecDateUpdate = System.DateTime.Today;
						this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
						this.myNotifier.UpdateDB();
					}
					catch (System.Exception)
					{
					}
					this.ShowMsg("??????????????????", true);
					this.ReloadProductOnSales(false);
					return;
				}
			}
			else if (e.CommandName == "UnSaleProduct")
			{
				int num = ProductHelper.OffShelf(e.CommandArgument.ToString());
				if (num > 0)
				{
					this.ShowMsg("????????????????????????????????????????????????????????????????????????????????????????????????", true);
					this.BindProducts();
					return;
				}
				this.ShowMsg("?????????????????????????????????", false);
			}
		}

		private void btnUpSale_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("??????????????????????????????", false);
				return;
			}
			int num = ProductHelper.UpShelf(text);
			if (num > 0)
			{
				this.ShowMsg("????????????????????????????????????????????????????????????????????????????????????????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("?????????????????????????????????", false);
		}

		private void btnUnSale_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("??????????????????????????????", false);
				return;
			}
			int num = ProductHelper.OffShelf(text);
			if (num > 0)
			{
				this.ShowMsg("????????????????????????????????????????????????????????????????????????????????????????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("?????????????????????????????????", false);
		}

		private void btnInStock_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("??????????????????????????????", false);
				return;
			}
			int num = ProductHelper.InStock(text);
			if (num > 0)
			{
				this.ShowMsg("?????????????????????????????????????????????????????????????????????????????????????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("?????????????????????????????????", false);
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("??????????????????????????????", false);
				return;
			}
			int num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				try
				{
					this.myNotifier.updateAction = UpdateAction.ProductUpdate;
					this.myNotifier.actionDesc = "??????????????????";
					this.myNotifier.RecDateUpdate = System.DateTime.Today;
					this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
					this.myNotifier.UpdateDB();
				}
				catch (System.Exception)
				{
				}
				this.ShowMsg("??????????????????????????????", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("?????????????????????????????????", false);
		}

		private void btnUpdateProductTags_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("??????????????????????????????", false);
				return;
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
			{
				string text2 = this.txtProductTag.Text.Trim();
				string[] array;
				if (text2.Contains(","))
				{
					array = text2.Split(new char[]
					{
						','
					});
				}
				else
				{
					array = new string[]
					{
						text2
					};
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					list.Add(System.Convert.ToInt32(value));
				}
			}
			string[] array3;
			if (text.Contains(","))
			{
				array3 = text.Split(new char[]
				{
					','
				});
			}
			else
			{
				array3 = new string[]
				{
					text
				};
			}
			int num = 0;
			string[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				string arg_FE_0 = array4[j];
			}
			if (num > 0)
			{
				this.ShowMsg(string.Format("??????????????????{0}????????????????????????", num), true);
			}
			else
			{
				this.ShowMsg("?????????????????????????????????????????????", true);
			}
			this.txtProductTag.Text = "";
		}

		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.productName,
				ProductCode = this.productCode,
				CategoryId = this.categoryId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this.startDate,
				BrandId = (this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null),
				TypeId = this.typeId,
				SaleStatus = this.saleStatus,
				EndDate = this.endDate
			};
			if (this.categoryId.HasValue && this.categoryId > 0)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.txtSearchText.Text = productQuery.Keywords;
			this.txtSKU.Text = productQuery.ProductCode;
			this.dropCategories.SelectedValue = productQuery.CategoryId;
			System.Data.DataTable productNum = ProductHelper.GetProductNum();
			this.LitOnSale.Text = "?????????(" + productNum.Rows[0]["OnSale"].ToString() + ")";
			this.LitOnStock.Text = "?????????(" + productNum.Rows[0]["OnStock"].ToString() + ")";
			this.LitZero.Text = "?????????(" + productNum.Rows[0]["Zero"].ToString() + ")";
			this.pager.TotalRecords = products.TotalRecords;
		}

		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			nameValueCollection.Add("SaleStatus", "1");
			base.ReloadPage(nameValueCollection);
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.dropBrandList.SelectedValue = new int?(value2);
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
			{
				this.tagId = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
			{
				this.typeId = new int?(value4);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			this.txtSearchText.Text = this.productName;
			this.txtSKU.Text = this.productCode;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
		}
	}
}
