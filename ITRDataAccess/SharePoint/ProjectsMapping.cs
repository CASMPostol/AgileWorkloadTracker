using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.AgileWorkloadTracker.Linq;

namespace CAS.ITRDataAccess.SharePoint
{
  internal class ProjectsMapping
  {
    internal static Dictionary<int, ProjectType> m_ProjectTypeMapping = new Dictionary<int, ProjectType>() 
    { 
      { 1, ProjectType.ProjectCommercial },
      { 2, ProjectType.ProjectInternal},
      { 3, ProjectType.Marketing},
      { 5, ProjectType.Office},
      { 8, ProjectType.AfterSalesServices},
      { 9, ProjectType.ProjectCommercial},
      {11, ProjectType.ProjectConception}
    };
    internal static Dictionary<int, int> MappingTable = new Dictionary<int, int>()
    {
      {49,	1},
      {105,	1},
      {135,	1},
      {153,	1},
      {168,	1},
      {50,	2},
      {106,	2},
      {136,	2},
      {152,	2},
      {58,	3},
      {107,	3},
      {137,	3},
      {151,	3},
      {176,	3},
      {41,	5},
      {109,	5},
      {108,	6},
      {115,	7},
      {138,	7},
      {150,	7},
      {177,	7},
      {29,	8},
      {51,	8},
      {114,	8},
      {36,	9},
      {56,	9},
      {113,	9},
      {8,	10},
      {20,	10},
      {21,	10},
      {53,	10},
      {54,	10},
      {96,	10},
      {122,	10},
      {2,	11},
      {13,	11},
      {25,	11},
      {76,	11},
      {98,	11},
      {117,	11},
      {118,	11},
      {126,	11},
      {35,	12},
      {48,	12},
      {104,	12},
      {134,	12},
      {154,	12},
      {181,	12},
      {38,	13},
      {43,	13},
      {100,	13},
      {120,	13},
      {130,	13},
      {142,	13},
      {143,	13},
      {148,	13},
      {158,	13},
      {170,	13},
      {180,	13},
      {141,	15},
      {149,	15},
      {173,	15},
      {174,	15},
      {175,	15},
      {183,	16}
    };
  }
}
