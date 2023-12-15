using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Task03
{
    public class XmlDataManager
    {
        public void SaveToFile<T>(string filePath, List<T> data)
        {
            XDocument xml = new XDocument(
                new XElement("Data",
                    from item in data
                    select new XElement("Item",
                        from prop in typeof(T).GetProperties()
                        select new XElement(prop.Name, prop.GetValue(item, null))
                    )
                )
            );

            xml.Save(filePath);
        }

        public List<T> LoadFromFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                XDocument xml = XDocument.Load(filePath);

                return xml.Descendants("Item").Select(item =>
                {
                    T newItem = Activator.CreateInstance<T>();
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        XElement element = item.Element(prop.Name);
                        if (element != null)
                        {
                            object value = Convert.ChangeType(element.Value, prop.PropertyType);
                            prop.SetValue(newItem, value, null);
                        }
                    }
                    return newItem;
                }).ToList();
            }
            else
            {
                return new List<T>();
            }
        }
    }
}