using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class BatchDetailResult : JdObject{


         [XmlElement("batchQty")]
public  		int
              batchQty
 { get; set; }


         [XmlElement("goodsNo")]
public  		string
              goodsNo
 { get; set; }


         [XmlElement("isvGoodsNo")]
public  		string
              isvGoodsNo
 { get; set; }


         [XmlElement("orderLine")]
public  		string
              orderLine
 { get; set; }


         [XmlElement("batAttrList")]
public  		List<string>
              batAttrList
 { get; set; }


}
}
