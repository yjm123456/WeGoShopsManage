using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class OrderStatus : JdObject{


         [XmlElement("soStatusCode")]
public  	int[]
              soStatusCode
 { get; set; }


         [XmlElement("soStatusName")]
public  		string
              soStatusName
 { get; set; }


         [XmlElement("operateTime")]
public  		string
              operateTime
 { get; set; }


         [XmlElement("operateUser")]
public  		string
              operateUser
 { get; set; }


}
}
