//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaJaMa.Recipes.Model.Entities.Base
{
    using System;
    using System.Collections.Generic;
    
    using System.Linq;
    
    using PaJaMa.Common;
    using PaJaMa.Data;
    
    using System.ComponentModel.DataAnnotations;
        
    public abstract class RecipeSearchBase : EntityBase
    {
        [Key]
        public virtual int RecipeID { get; set; }
        public virtual string RecipeName { get; set; }
        public virtual string Ingredients { get; set; }
    
    	public override string ToString()
    	{
    		return RecipeName;
    	}
    }
}
