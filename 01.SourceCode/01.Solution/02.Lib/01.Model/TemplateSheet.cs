using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
	/// <summary>
	/// This object represents the properties and methods of a TemplateSheet.
	/// </summary>
    [ORTableMapping("dbo.TemplateSheet")]
    public class TemplateSheet:BaseModel
	{
	 
		#region Public Properties 
		 
        [ORFieldMapping("TemplateID")]
		public string TemplateID { get;set;}		
        
        [ORFieldMapping("TemplateName")]
		public string TemplateName { get;set;}	

        
        [ORFieldMapping("TemplateSheetName")]
		public string TemplateSheetName { get;set;}
		 

        
        [ORFieldMapping("TemplateSheetTitle")]
		public string TemplateSheetTitle{get;set;}
		 

        
        [ORFieldMapping("TemplateSheetRemark")]
		public string TemplateSheetRemark{get;set;}
		 

        
        [ORFieldMapping("Status")]
		public int Status{get;set;}


        [ORFieldMapping("RowNum")]
        public int RowNum { get; set; }



        [ORFieldMapping("ColumnNum")]
        public int ColumnNum { get; set; }
        #endregion


    } 
}

