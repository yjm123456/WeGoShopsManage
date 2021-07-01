using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Tools.WebEnums;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class PaymentDropDownList : DropDownList
	{
		private bool allowNull = true;

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue));
				}
				return result;
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
				}
				else
				{
					base.SelectedValue = value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			if (SettingsManager.GetMasterSettings(false).EnableAlipayRequest)
			{
				IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes();
				foreach (PaymentModeInfo current in paymentModes)
				{
					base.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.ModeId.ToString()));
				}
			}
			if (SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
			{
                var id = PayMentEnums.PayWayEnum.weixinrequest;
                base.Items.Add(new ListItem(MemberHelper.GetEnumDescription(id), id.ToString()));
			}
			if (SettingsManager.GetMasterSettings(false).EnableOffLineRequest)
            {
                var id = PayMentEnums.PayWayEnum.offlinerequest;
                base.Items.Add(new ListItem(MemberHelper.GetEnumDescription(id), id.ToString()));
            }
			if (SettingsManager.GetMasterSettings(false).EnablePodRequest)
			{
				base.Items.Add(new ListItem("货到付款", "-1"));
			}
		}

	}
}
