//<summary>
//  Title   : class HelpWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Windows;

namespace CAS.AgileWorkloadTracker.SiteManagement
{
  /// <summary>
  /// Interaction logic for HelpWindow.xaml
  /// </summary>
  public partial class HelpWindow : Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HelpWindow"/> class.
    /// </summary>
    public HelpWindow()
    {
      InitializeComponent();
      x_Version.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
  }
}
