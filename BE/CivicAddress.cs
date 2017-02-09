using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
    public class CivicAddress
    {
        [XmlElement("address")]
        public string Address { get; set; }

        [XmlElement("city")]
        public string City { get; set; }
        public override string ToString()
            => (!string.IsNullOrEmpty(Address) ? "Address: " + Address + ", " : "") + "City: " + City;

        static public List<string> Cities = new List<string> { "אופקים", "אור יהודה", "אור עקיבא", "אילת", "אריאל", "אשדוד", "אשקלון", "באר שבע", "בית שאן", "בית שמש", "ביתר עילית", "בני ברק", "בת ים", "גבעתיים", "דימונה", "הוד השרון", "הרצליה", "חדרה", "חולון", "חיפה", "טבריה", "טירת כרמל", "יבנה", "יהוד-מונוסון", "ירושלים", "כפר סבא", "כרמיאל", "לוד", "מגדל העמק", "מודיעין-מכבים-רעות", "מעלה אדומים", "מעלות-תרשיחא", "נס ציונה", "נצרת", "נצרת עילית", "נשר", "נתיבות", "נתניה", "עכו", "עפולה", "ערד", "פתח תקוה", "צפת", "קרית אונו", "קרית אתא", "קרית ביאליק", "קרית גת", "קרית ים", "קרית מוצקין", "קרית מלאכי", "קרית שמונה", "ראש העין", "ראשון לציון", "רחובות", "רמלה", "רמת גן", "רמת השרון", "רעננה", "שדרות", "תל אביב-יפו" };

        // method accepts XElement and returns CivicAddress
        public static explicit operator CivicAddress(XElement XRoot)
        {
            CivicAddress civicAddress = new CivicAddress();
            civicAddress.Address = (string)XRoot.Element("address");
            civicAddress.City = (string)XRoot.Element("city");

            return civicAddress;
        }

        public override bool Equals(object other)
            => Address == (other as CivicAddress)?.Address && City == (other as CivicAddress)?.City;
    }
}
