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
        
    public abstract class UserRecipeBase : EntityBase
    {
        [Key]
        public virtual int UserRecipeID { get; set; }
        public virtual int UserID { get; set; }
        public virtual int RecipeID { get; set; }
        public virtual bool IsBookmarked { get; set; }
        public virtual float? Rating { get; set; }
        public virtual string Notes { get; set; }
        public virtual bool AllowEdit { get; set; }
    
        public Recipe Recipe { get; set; }
        public User User { get; set; }
    
    	public override string ToString()
    	{
    		return Notes;
    	}
    }
}
