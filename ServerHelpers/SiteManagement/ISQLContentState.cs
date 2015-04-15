using System;
//<summary>
//  Title   : SQLContentState
//  System  : Microsoft VisualStudio 2013 / C#
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

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  /// <summary>
  /// Interface SQLContentState - contains information about the Shepherd SQL database accessibility and content state.
  /// </summary>
  public interface ISQLContentState
  {
    /// <summary>
    /// Gets or sets the cleanup last run by.
    /// </summary>
    /// <value>The user name.</value>
    string CleanupLastRunBy { get; set; }
    /// <summary>
    /// Gets or sets the synchronization last run by.
    /// </summary>
    /// <value>The user name.</value>
    string SyncLastRunBy { get; set; }
    /// <summary>
    /// Gets or sets the archiving last run by.
    /// </summary>
    /// <value>The user name.</value>
    string ArchivingLastRunBy { get; set; }
    /// <summary>
    /// Gets or sets the cleanup last run date.
    /// </summary>
    /// <value>The last cleanup <see cref="DateTime"/>.</value>
    DateTime? CleanupLastRunDate { get; set; }
    /// <summary>
    /// Gets or sets the synchronize last run date.
    /// </summary>
    /// <value>The las synchronize <see cref="DateTime"/>.</value>
    DateTime? SyncLastRunDate { get; set; }
    /// <summary>
    /// Gets or sets the archiving last run date.
    /// </summary>
    /// <value>The last archiving <see cref="DateTime"/>.</value>
    DateTime? ArchivingLastRunDate { get; set; }
    /// <summary>
    /// Gets or sets the SQL connection string.
    /// </summary>
    /// <value>The SQL connection string.</value>
    string SQLConnectionString { get; }
    /// <summary>
    /// Gets or sets a value indicating whether the SQL database can be accessed using the <see cref="ISQLContentState.SQLConnectionString"/>.
    /// </summary>
    /// <value><c>true</c> if SQL successfully connected and gathered all required data about the content state; otherwise, <c>false</c>.</value>
    bool SQLConnected { get; set; }

  }
}
