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
        
    public abstract class MeasurementBase : EntityBase
    {
        public MeasurementBase()
        {
            this.IngredientMeasurements = new HashSet<IngredientMeasurement>();
        }
    
        [Key]
        public virtual int MeasurementID { get; set; }
        public virtual string MeasurementName { get; set; }
    
        public ICollection<IngredientMeasurement> IngredientMeasurements { get; set; }
    
    	public override string ToString()
    	{
    		return MeasurementName;
    	}
    }
}
