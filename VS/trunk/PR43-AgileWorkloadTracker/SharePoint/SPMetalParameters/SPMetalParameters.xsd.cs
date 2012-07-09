using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace CAS.SharePoint.SPMetalParameters
{
  public partial class PRWeb
  {
    public static void ImportDocument(Stream documetStream, PRWeb _object)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PRWeb));
      serializer.Serialize(documetStream, _object);
    }
    /// <summary>
    /// Imports the data model.
    /// </summary>
    /// <param name="_URL">The URL of the site collection.</param>
    /// <returns>An object of <see cref="PRWeb"/> type containing SPMetal configuration parameters.</returns>
    public static PRWeb ImportDataModel(string _URL, string _className)
    {
      PRWeb _ret = null;
      using (SPSite site = new SPSite(_URL))
      {
        using (SPWeb _wb = site.RootWeb)
        {
          List<PRContentType> _cts = new List<PRContentType>();
          List<PRList> _lists = new List<PRList>();
          foreach (SPList _lix in _wb.Lists)
          {
            PRList _nlist = new PRList() { Name = _lix.Title };
            _lists.Add(_nlist);
          }
          foreach (SPContentType _contentType in _wb.AvailableContentTypes)
          {
            if (!_contentType.Group.Contains("CAS") && !_contentType.Name.Contains("Element"))
              continue;
            List<PRColumn> _columns = new List<PRColumn>();
            foreach (SPField _fx in _contentType.Fields)
            {
              if (_fx.Hidden)
                continue;
              PRColumn _newColumn = new PRColumn()
              {
                Name = _fx.InternalName,
                Member = _fx.InternalName,
                TypeSpecified = false
              };
              _columns.Add(_newColumn);
            }
            PRContentType _newCT = new PRContentType
            {
              Name = _contentType.Name,
              Class = _contentType.Name,
              Column = _columns.ToArray()
            };
            _cts.Add(_newCT);
          }
          _ret = new PRWeb()
          {
            AccessModifier = PRAccessModifier.Internal,
            Class = _className,
            ContentType = _cts.ToArray(),
            List = _lists.ToArray()
          };
        }
      }
      return _ret;
    }
  }
}
