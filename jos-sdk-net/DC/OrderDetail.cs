using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class OrderDetail : JdObject{


         [XmlElement("goodsNo")]
public  		string
              goodsNo
 { get; set; }


         [XmlElement("price")]
public  		double
              price
 { get; set; }


         [XmlElement("quantity")]
public  		int
              quantity
 { get; set; }


         [XmlElement("batAttrs")]
public  		string
              batAttrs
 { get; set; }


         [XmlElement("isvLotattrs")]
public  		string
              isvLotattrs
 { get; set; }


         [XmlElement("packBatchNo")]
public  		string
              packBatchNo
 { get; set; }


         [XmlElement("poNo")]
public  		string
              poNo
 { get; set; }


         [XmlElement("productionDate")]
public  		string
              productionDate
 { get; set; }


         [XmlElement("expirationDate")]
public  		string
              expirationDate
 { get; set; }


         [XmlElement("lot")]
public  		string
              lot
 { get; set; }


}
}
