using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

/// <summary>
/// Returns values of XML attributes
/// </summary>
public class AttributeFactory
{
    // Retrieves the value of a specific attribute
    protected static T GetAttributes<T>(string value, XmlAttributeCollection attributeCollection)
    {
        string attribute = attributeCollection[value].Value;
        return (T)Convert.ChangeType(attribute, typeof(T));
    }


    // This one gets called only if we are trying to get the height value
    protected static T GetWayAttributes<T>(string value, XmlAttributeCollection attributeCollection)
    {
        string attribute = attributeCollection[value].Value;

        // True if attribute contains letters
        bool containsLetter = attribute.Any(x => char.IsLetter(x));
        bool containsFeetAndInches = attribute.Contains("'");
        bool containsSemicolon = attribute.Contains(";");
        bool containsSignedNumber = attribute.Contains("-");

        if (containsLetter)
        {
            // If attribute contains letters remove them
            attribute = Regex.Replace(attribute, "[A-Za-z ]", "");
            attribute.Trim();
            if(attribute.Equals(null))
            {
                return (T)Convert.ChangeType(0, typeof(float));
            }
        }
        if (containsFeetAndInches || containsSemicolon || containsSignedNumber)
        {
            return (T)Convert.ChangeType(0, typeof(float));
        }

        return (T)Convert.ChangeType(attribute, typeof(T));
    }
}