using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
	/// <summary>
	/// This object represents the properties and methods of a TemplateConfigSelect.
	/// </summary>
    [ORTableMapping("dbo.TemplateConfigSelect")]
    public class TemplateConfigSelect:BaseModel
	{
	 
		#region Public Properties 
		 


        
        [ORFieldMapping("TemplateConfigID")]
		public string TemplateConfigID{get;set;}
		 
        

        [ORFieldMapping("TemplateID")]
        public string TemplateID { get; set; }



        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }

        [ORFieldMapping("TemplateSheetID")]
        public string TemplateSheetID { get; set; }



        [ORFieldMapping("TemplateSheetName")]
        public string TemplateSheetName { get; set; }
        
        [ORFieldMapping("SelectedValue")]
		public string SelectedValue{get;set;}
		 

        
        [ORFieldMapping("SortIndex")]
		public int SortIndex{get;set;}
		 

        
        [ORFieldMapping("Status")]
		public int Status{get;set;}
		 

		#endregion
		
		 
	} 
}

