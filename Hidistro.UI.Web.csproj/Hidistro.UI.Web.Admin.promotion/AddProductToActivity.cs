using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddProductToActivity : AdminPage
	{
		protected ProductSaleStatus _status = ProductSaleStatus.OnSale;

		protected int _id;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblJoin;

		protected System.Web.UI.WebControls.Label lbsaleNumber;

		protected System.Web.UI.WebControls.Label lbwareNumber;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minPrice;

		protected System.Web.UI.WebControls.TextBox txt_maxPrice;

		protected System.Web.UI.WebControls.Button btnQuery;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected AddProductToActivity() : base("m08", "yxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && !this.bInt(base.Request["id"].ToString(), ref this._id))
			{
				this._id = 0;
			}
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			if (!base.IsPostBack)
			{
				this.BindProducts(this._id);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.BindProducts(this._id);
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private void BindProducts(int actId)
		{
			System.Data.DataTable selectedProducts = this.GetSelectedProducts();
			string text = this.txt_name.Text;
			string text2 = this.txt_minPrice.Text;
			string text3 = this.txt_maxPrice.Text;
			decimal? minPrice = null;
			decimal? maxPrice = null;
			decimal value = 0m;
			if (!this.bDecimal(text2, ref value))
			{
				minPrice = null;
			}
			else
			{
				minPrice = new decimal?(value);
			}
			if (!this.bDecimal(text3, ref value))
			{
				maxPrice = null;
			}
			else
			{
				maxPrice = new decimal?(value);
			}
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = text,
				ProductCode = "",
				CategoryId = null,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = null,
				BrandId = null,
				EndDate = null,
				TypeId = null,
				SaleStatus = this._status,
				minPrice = minPrice,
				maxPrice = maxPrice
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			dataTable.Columns.Add("seledStatus");
			dataTable.Columns.Add("canSelStatus");
			dataTable.Columns.Add("canChkStatus");
			dataTable.Columns.Add("btnCode");
			if (dataTable != null)
			{
				if (dataTable.Rows.Count > 0)
				{
					string str = "0";
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						int num = int.Parse(dataTable.Rows[i]["ProductId"].ToString());
						System.Data.DataRow[] array = selectedProducts.Select(string.Format("ProductId={0}", num));
						if (array.Length > 0)
						{
							bool flag = false;
							System.Data.DataRow[] array2 = array;
							for (int j = 0; j < array2.Length; j++)
							{
								System.Data.DataRow dataRow = array2[j];
								if (dataRow["ActivitiesId"].ToString() == this._id.ToString())
								{
									flag = true;
									break;
								}
								str = dataRow["ActivitiesId"].ToString();
							}
							if (flag)
							{
								dataTable.Rows[i]["btnCode"] = "<button type=\"button\" class=\"btn btn-success resetSize\" name=\"selectBtn\" disabled>已加入本次活动</button>";
							}
							else
							{
								dataTable.Rows[i]["btnCode"] = "<a title=\"点击新窗口打开该活动\" href=\"AddProductToActivity.aspx?id=" + str + "\" target=\"_blank\" class=\"btn btn-warning resetSize\">已加入其他活动</button>";
							}
						}
						else
						{
							dataTable.Rows[i]["btnCode"] = "<input type=\"button\" class=\"btn btn-info resetSize\" name=\"selectBtn\" value=\"选取加入\" onclick=\"var obj=this;HiTipsShow('<strong>确定要加入该活动吗？</strong><p>加入该活动后，其他满减活动将不能选择该商品！</p>', 'confirmII', function () {saveSingle(" + dataTable.Rows[i]["ProductId"].ToString() + ")});\"/>";
						}
					}
				}
				else if (dataTable.Rows.Count > 0)
				{
					for (int k = 0; k < dataTable.Rows.Count; k++)
					{
					}
				}
			}
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = products.TotalRecords;
			this.lbsaleNumber.Text = products.TotalRecords.ToString();
			System.Data.DataTable selectedProducts2 = this.GetSelectedProducts(actId);
			this.lblJoin.Text = ((selectedProducts2 != null) ? selectedProducts2.Rows.Count.ToString() : "0");
			this.setInStock();
		}

		private void setInStock()
		{
			System.Data.DataTable productNum = ProductHelper.GetProductNum();
			this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
		}

		private System.Data.DataTable GetSelectedProducts(int actId)
		{
			return ActivityHelper.QueryProducts(actId);
		}

		private System.Data.DataTable GetSelectedProducts()
		{
			return ActivityHelper.QuerySelProducts();
		}
	}
}
