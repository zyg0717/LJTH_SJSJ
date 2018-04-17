using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;
 

namespace Lib.Model
{
	/// <summary>
	/// This object represents the properties and methods of a Enum.
	/// </summary>
    [ORTableMapping("dbo.Enums")]
    public class Enum:BaseModel
	{
	 
		#region Public Properties       

        
        [ORFieldMapping("EnumType")]
		public string EnumType{get;set;}
		 

        
        [ORFieldMapping("EnumCode")]
		public string EnumCode{get;set;}
		 

        
        [ORFieldMapping("EnumName")]
		public string EnumName{get;set;}
		 

        
        [ORFieldMapping("DisplayOrder")]
		public int DisplayOrder{get;set;}
		 

		#endregion
		
		 
	} 
}

