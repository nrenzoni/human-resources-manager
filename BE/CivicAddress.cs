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

        static public List<string> Cities = new List<string> { "עפולה", "עכו", "ערד", "אריאל", "אשדוד", "אשקלון", "בת ים", "באר שבע", "בית שאן", "בית שמש", "ביתר עילית", "בני ברק", "דימונה", "אילת", "גבעתיים", "חדרה", "חיפה", "הרצליה", "הוד השרון", "חולון", "ירושלים", "כרמיאל", "כפר סבא", "קרית אתא", "קרית ביאליק", "קרית גת", "קרית מלאכי", "קרית מוצקין", "קרית אונו", "קרית שמונה", "קרית ים", "לוד", "מעלה אדומים", "מעלות-תרשיחא", "מגדל העמק", "מודיעין-מכבים-רעות", "נצרת", "נצרת עילית", "נס ציונה", "נשר", "נתניה", "נתיבות", "אופקים", "אור עקיבא", "אור יהודה", "פתח תקוה", "רעננה", "רמת השרון", "רמת גן", "רמלה", "רחובות", "ראשון לציון", "ראש העין", "שדרות", "תל אביב-יפו", "טבריה", "טירת כרמל", "צפת", "יבנה", "יהוד-מונוסון" };

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
