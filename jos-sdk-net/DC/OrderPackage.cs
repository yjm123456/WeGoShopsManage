using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class OrderPackage : JdObject{


         [XmlElement("packageNo")]
public  		string
              packageNo
 { get; set; }


         [XmlElement("packWeight")]
public  		double
              packWeight
 { get; set; }


}
}
