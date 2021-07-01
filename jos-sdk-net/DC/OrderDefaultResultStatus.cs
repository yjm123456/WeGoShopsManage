using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class OrderDefaultResultStatus : JdObject{


         [XmlElement("eclpSoNo")]
public  		string
              eclpSoNo
 { get; set; }


         [XmlElement("isvUUID")]
public  		string
              isvUUID
 { get; set; }


         [XmlElement("orderStatusList")]
public  		List<string>
              orderStatusList
 { get; set; }


}
}
