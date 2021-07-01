using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Jd.Api.Domain;
namespace Jd.Api.Domain
{





[Serializable]
public class BatAttr : JdObject{


         [XmlElement("batchKey")]
public  		string
              batchKey
 { get; set; }


         [XmlElement("batchValue")]
public  		string
              batchValue
 { get; set; }


}
}
