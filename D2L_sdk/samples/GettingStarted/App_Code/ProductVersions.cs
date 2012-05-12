using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 

[Serializable]
public class ProductVersions
{
    public String LatestVersion;
    public String ProductCode;
    public String[] SupportedVersions;
	public ProductVersions()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}