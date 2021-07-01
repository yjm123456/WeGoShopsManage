using System;
using System.Collections.Generic;
using Jd.Api.Response;
using Jd.Api.Util;

namespace Jd.Api.Request
{
    public class EclpOrderAddOrderRequest : IJdRequest<EclpOrderAddOrderResponse>
    {

        public string
                       isvUUID
        { get; set; }




        public string
                       isvSource
        { get; set; }




        public string
                       shopNo
        { get; set; }




        public string
                       bdOwnerNo
        { get; set; }




        public string
                       departmentNo
        { get; set; }




        public string
                       warehouseNo
        { get; set; }




        public string
                       shipperNo
        { get; set; }




        public string
                       salesPlatformOrderNo
        { get; set; }




        public string
                       salePlatformSource
        { get; set; }




        public Nullable<DateTime>
                       salesPlatformCreateTime
        { get; set; }




        public string
                       soType
        { get; set; }




        public string
                       consigneeName
        { get; set; }




        public string
                       consigneeMobile
        { get; set; }




        public string
                       consigneePhone
        { get; set; }




        public string
                       consigneeEmail
        { get; set; }




        public Nullable<DateTime>
                       expectDate
        { get; set; }




        public string
                       addressProvince
        { get; set; }




        public string
                       addressCity
        { get; set; }




        public string
                       addressCounty
        { get; set; }




        public string
                       addressTown
        { get; set; }




        public string
                       consigneeAddress
        { get; set; }




        public string
                       consigneePostcode
        { get; set; }




        public Nullable<double>
                       receivable
        { get; set; }




        public string
                       consigneeRemark
        { get; set; }




        public string
                       orderMark
        { get; set; }




        public string
                       thirdWayBill
        { get; set; }




        public string
                       packageMark
        { get; set; }




        public string
                       businessType
        { get; set; }




        public string
                       destinationCode
        { get; set; }




        public string
                       destinationName
        { get; set; }




        public string
                       sendWebsiteCode
        { get; set; }




        public string
                       sendWebsiteName
        { get; set; }




        public string
                       sendMode
        { get; set; }




        public string
                       receiveMode
        { get; set; }




        public string
                       appointDeliveryTime
        { get; set; }




        public string
                       insuredPriceFlag
        { get; set; }




        public Nullable<double>
                       insuredValue
        { get; set; }




        public string
                       thirdPayment
        { get; set; }




        public string
                       monthlyAccount
        { get; set; }




        public string
                       shipment
        { get; set; }




        public string
                       sellerRemark
        { get; set; }




        public string
                       thirdSite
        { get; set; }




        public string
                       gatherCenterName
        { get; set; }




        public string
                       customsStatus
        { get; set; }




        public string
                       customerName
        { get; set; }




        public string
                       invoiceTitle
        { get; set; }




        public string
                       invoiceContent
        { get; set; }




        public string
                       goodsType
        { get; set; }




        public string
                       goodsLevel
        { get; set; }




        public string
                       customsPort
        { get; set; }




        public string
                       billType
        { get; set; }




        public Nullable<double>
                       orderPrice
        { get; set; }




        public string
                       wlyInfo
        { get; set; }




        public string
                       customerId
        { get; set; }




        public Nullable<int>
                       urgency
        { get; set; }




        public string
                       customerNo
        { get; set; }




        public string
                       storeName
        { get; set; }




        public string
                       invoiceState
        { get; set; }




        public string
                       invoiceType
        { get; set; }




        public string
                       invoiceNo
        { get; set; }




        public string
                       invoiceTax
        { get; set; }




        public string
                       bankName
        { get; set; }




        public string
                       bankAccount
        { get; set; }




        public string
                       address
        { get; set; }




        public string
                       phoneNumber
        { get; set; }




        public string
                       signType
        { get; set; }




        public string
                       signIDCode
        { get; set; }




        public string
                       supplierNo
        { get; set; }




        public string
                       agingType
        { get; set; }




        public string
                       sellerNote
        { get; set; }




        public string
                       supervisionCode
        { get; set; }




        public string
                       invoiceChecker
        { get; set; }




        public string
                       paymentType
        { get; set; }




        public string
                       saleType
        { get; set; }




        public string
                       inStorageNo
        { get; set; }




        public Nullable<DateTime>
                       inStorageTime
        { get; set; }




        public string
                       inStorageRemark
        { get; set; }




        public string
                       grossReturnName
        { get; set; }




        public string
                       grossReturnPhone
        { get; set; }




        public string
                       grossReturnMobile
        { get; set; }




        public string
                       grossReturnAddress
        { get; set; }




        public string
                       isvPackTypeNo
        { get; set; }




        public string
                       addrAnalysis
        { get; set; }




        public string
                       printExtendInfo
        { get; set; }




        public string
                       logicParam
        { get; set; }




        public string
                       combineNo
        { get; set; }




        public string
                       activationService
        { get; set; }




        public Nullable<int>
                       randomInspection
        { get; set; }




        public string
                       VIPDeliWarehouse
        { get; set; }




        public string
                       customField
        { get; set; }




        public Nullable<double>
                       longitude
        { get; set; }




        public Nullable<double>
                       latitude
        { get; set; }




        public Nullable<int>
                       agingProductType
        { get; set; }




        public string
                       crossDockPriority
        { get; set; }




        public string
                       isvCompanyNo
        { get; set; }




        public string
                       orderPriority
        { get; set; }




        public string
                       orderBatchNo
        { get; set; }




        public Nullable<int>
                       orderBatchQty
        { get; set; }




        public string
                       productCode
        { get; set; }




        public string
                       vehicleType
        { get; set; }



        public string
goodsNo
        { get; set; }
        public string
skuGoodsLevel
        { get; set; }
        public string
goodsName
        { get; set; }
        public string
type
        { get; set; }
        public string
unit
        { get; set; }
        public string
remark
        { get; set; }
        public string
rate
        { get; set; }
        public string
amount
        { get; set; }
        public string
price
        { get; set; }
        public string
quantity
        { get; set; }
        public string
pAttributes
        { get; set; }
        public string
isvLotattrs
        { get; set; }
        public string
isvGoodsNo
        { get; set; }
        public string
installVenderId
        { get; set; }
        public string
orderLine
        { get; set; }
        public string
batAttrs
        { get; set; }
        public string
productionDate
        { get; set; }
        public string
expirationDate
        { get; set; }
        public string
packBatchNo
        { get; set; }
        public string
poNo
        { get; set; }
        public string
lot
        { get; set; }

        private IDictionary<string, string> otherParameters;

        public string GetApiName()
        {
            return "jingdong.eclp.order.addOrder";
        }

        public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
            parameters.Add("isvUUID", this.isvUUID
);
            parameters.Add("isvSource", this.isvSource
);
            parameters.Add("shopNo", this.shopNo
);
            parameters.Add("bdOwnerNo", this.bdOwnerNo
);
            parameters.Add("departmentNo", this.departmentNo
);
            parameters.Add("warehouseNo", this.warehouseNo
);
            parameters.Add("shipperNo", this.shipperNo
);
            parameters.Add("salesPlatformOrderNo", this.salesPlatformOrderNo
);
            parameters.Add("salePlatformSource", this.salePlatformSource
);
            parameters.Add("salesPlatformCreateTime", this.salesPlatformCreateTime
);
            parameters.Add("soType", this.soType
);
            parameters.Add("consigneeName", this.consigneeName
);
            parameters.Add("consigneeMobile", this.consigneeMobile
);
            parameters.Add("consigneePhone", this.consigneePhone
);
            parameters.Add("consigneeEmail", this.consigneeEmail
);
            parameters.Add("expectDate", this.expectDate
);
            parameters.Add("addressProvince", this.addressProvince
);
            parameters.Add("addressCity", this.addressCity
);
            parameters.Add("addressCounty", this.addressCounty
);
            parameters.Add("addressTown", this.addressTown
);
            parameters.Add("consigneeAddress", this.consigneeAddress
);
            parameters.Add("consigneePostcode", this.consigneePostcode
);
            parameters.Add("receivable", this.receivable
);
            parameters.Add("consigneeRemark", this.consigneeRemark
);
            parameters.Add("orderMark", this.orderMark
);
            parameters.Add("thirdWayBill", this.thirdWayBill
);
            parameters.Add("packageMark", this.packageMark
);
            parameters.Add("businessType", this.businessType
);
            parameters.Add("destinationCode", this.destinationCode
);
            parameters.Add("destinationName", this.destinationName
);
            parameters.Add("sendWebsiteCode", this.sendWebsiteCode
);
            parameters.Add("sendWebsiteName", this.sendWebsiteName
);
            parameters.Add("sendMode", this.sendMode
);
            parameters.Add("receiveMode", this.receiveMode
);
            parameters.Add("appointDeliveryTime", this.appointDeliveryTime
);
            parameters.Add("insuredPriceFlag", this.insuredPriceFlag
);
            parameters.Add("insuredValue", this.insuredValue
);
            parameters.Add("thirdPayment", this.thirdPayment
);
            parameters.Add("monthlyAccount", this.monthlyAccount
);
            parameters.Add("shipment", this.shipment
);
            parameters.Add("sellerRemark", this.sellerRemark
);
            parameters.Add("thirdSite", this.thirdSite
);
            parameters.Add("gatherCenterName", this.gatherCenterName
);
            parameters.Add("customsStatus", this.customsStatus
);
            parameters.Add("customerName", this.customerName
);
            parameters.Add("invoiceTitle", this.invoiceTitle
);
            parameters.Add("invoiceContent", this.invoiceContent
);
            parameters.Add("goodsType", this.goodsType
);
            parameters.Add("goodsLevel", this.goodsLevel
);
            parameters.Add("customsPort", this.customsPort
);
            parameters.Add("billType", this.billType
);
            parameters.Add("orderPrice", this.orderPrice
);
            parameters.Add("wlyInfo", this.wlyInfo
);
            parameters.Add("customerId", this.customerId
);
            parameters.Add("urgency", this.urgency
);
            parameters.Add("customerNo", this.customerNo
);
            parameters.Add("storeName", this.storeName
);
            parameters.Add("invoiceState", this.invoiceState
);
            parameters.Add("invoiceType", this.invoiceType
);
            parameters.Add("invoiceNo", this.invoiceNo
);
            parameters.Add("invoiceTax", this.invoiceTax
);
            parameters.Add("bankName", this.bankName
);
            parameters.Add("bankAccount", this.bankAccount
);
            parameters.Add("address", this.address
);
            parameters.Add("phoneNumber", this.phoneNumber
);
            parameters.Add("signType", this.signType
);
            parameters.Add("signIDCode", this.signIDCode
);
            parameters.Add("supplierNo", this.supplierNo
);
            parameters.Add("agingType", this.agingType
);
            parameters.Add("sellerNote", this.sellerNote
);
            parameters.Add("supervisionCode", this.supervisionCode
);
            parameters.Add("invoiceChecker", this.invoiceChecker
);
            parameters.Add("paymentType", this.paymentType
);
            parameters.Add("saleType", this.saleType
);
            parameters.Add("inStorageNo", this.inStorageNo
);
            parameters.Add("inStorageTime", this.inStorageTime
);
            parameters.Add("inStorageRemark", this.inStorageRemark
);
            parameters.Add("grossReturnName", this.grossReturnName
);
            parameters.Add("grossReturnPhone", this.grossReturnPhone
);
            parameters.Add("grossReturnMobile", this.grossReturnMobile
);
            parameters.Add("grossReturnAddress", this.grossReturnAddress
);
            parameters.Add("isvPackTypeNo", this.isvPackTypeNo
);
            parameters.Add("addrAnalysis", this.addrAnalysis
);
            parameters.Add("printExtendInfo", this.printExtendInfo
);
            parameters.Add("logicParam", this.logicParam
);
            parameters.Add("combineNo", this.combineNo
);
            parameters.Add("activationService", this.activationService
);
            parameters.Add("randomInspection", this.randomInspection
);
            parameters.Add("VIPDeliWarehouse", this.VIPDeliWarehouse
);
            parameters.Add("customField", this.customField
);
            parameters.Add("longitude", this.longitude
);
            parameters.Add("latitude", this.latitude
);
            parameters.Add("agingProductType", this.agingProductType
);
            parameters.Add("crossDockPriority", this.crossDockPriority
);
            parameters.Add("isvCompanyNo", this.isvCompanyNo
);
            parameters.Add("orderPriority", this.orderPriority
);
            parameters.Add("orderBatchNo", this.orderBatchNo
);
            parameters.Add("orderBatchQty", this.orderBatchQty
);
            parameters.Add("productCode", this.productCode
);
            parameters.Add("vehicleType", this.vehicleType
);
            parameters.Add("goodsNo", this.goodsNo
);
            parameters.Add("skuGoodsLevel", this.skuGoodsLevel
);
            parameters.Add("goodsName", this.goodsName
);
            parameters.Add("type", this.type
);
            parameters.Add("unit", this.unit
);
            parameters.Add("remark", this.remark
);
            parameters.Add("rate", this.rate
);
            parameters.Add("amount", this.amount
);
            parameters.Add("price", this.price
);
            parameters.Add("quantity", this.quantity
);
            parameters.Add("pAttributes", this.pAttributes
);
            parameters.Add("isvLotattrs", this.isvLotattrs
);
            parameters.Add("isvGoodsNo", this.isvGoodsNo
);
            parameters.Add("installVenderId", this.installVenderId
);
            parameters.Add("orderLine", this.orderLine
);
            parameters.Add("batAttrs", this.batAttrs
);
            parameters.Add("productionDate", this.productionDate
);
            parameters.Add("expirationDate", this.expirationDate
);
            parameters.Add("packBatchNo", this.packBatchNo
);
            parameters.Add("poNo", this.poNo
);
            parameters.Add("lot", this.lot
);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
        }

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new JdDictionary();
            }
            this.otherParameters.Add(key, value);
        }

    }
}











