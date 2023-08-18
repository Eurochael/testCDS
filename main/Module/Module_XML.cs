using System;
using System.Xml;

namespace cds
{
    class Module_XML
    {
        static public string XML_Inidata_Read(string strPath, string strSection, string strKey)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlNode;
            string strResult;
            try
            {
                xmlDoc.Load(strPath);
                xmlNode = xmlDoc.SelectSingleNode(strSection + "/" + strKey);
                if (xmlNode == null)
                {
                    strResult = "";
                }
                else
                {
                    strResult = xmlNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                strResult = "Error : " + ex.ToString();
                //Debug.WriteLine("clsXml / XML_Inidata_Read / " + ex.Message);
            }
            return strResult;
        }

        static public bool XML_Inidata_Write(string strPath, string strSection, string strKey, string strInnerText)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlNode;
            XmlElement xmlNodeChild;
            bool bolResult = false;
            try
            {
                xmlDoc.Load(strPath);
                xmlNode = xmlDoc.SelectSingleNode(strSection + "/" + strKey);
                if (xmlNode == null)
                {
                    xmlNode = xmlDoc.SelectSingleNode(strSection);
                    xmlNodeChild = xmlDoc.CreateElement(strKey);
                    xmlNodeChild.InnerText = strInnerText;
                    xmlNode.AppendChild(xmlNodeChild);
                }
                else
                {
                    xmlNode.InnerText = strInnerText;
                }
                xmlDoc.Save(strPath);
            }
            catch (Exception ex)
            {
                bolResult = false;
                //Debug.WriteLine("clsXml / XML_Inidata_Write / " + ex.Message);
            }
            finally
            {
                bolResult = true;
            }
            return bolResult;
        }
    }
}
