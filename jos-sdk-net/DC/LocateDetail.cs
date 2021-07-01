using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class LocateDetail : JdObject{


         [XmlElement("locateSkuNo")]
public  		string
              locateSkuNo
 { get; set; }


         [XmlElement("locateSkuName")]
public  		string
              locateSkuName
 { get; set; }


         [XmlElement("plannedQty")]
public  		int
              plannedQty
 { get; set; }


         [XmlElement("locateShippedQty")]
public  		int
              locateShippedQty
 { get; set; }


         [XmlElement("locateIsvLotattrs")]
public  		string
              locateIsvLotattrs
 { get; set; }


         [XmlElement("locateUnit")]
public  		string
              locateUnit
 { get; set; }


}
}
