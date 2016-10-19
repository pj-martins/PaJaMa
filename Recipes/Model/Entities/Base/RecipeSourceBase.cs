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
        
    public abstract class RecipeSourceBase : EntityBase
    {
    	public virtual bool SaveRecipes { get { return false; } }
        public RecipeSourceBase()
        {
            this.Recipes = new HashSet<Recipe>();
        }
    
        [Key]
        public virtual int RecipeSourceID { get; set; }
        public virtual string RecipeSourceName { get; set; }
    
        public virtual ICollection<Recipe> Recipes { get; set; }
    
    	public override string ToString()
    	{
    		return RecipeSourceName;
    	}
    }
}