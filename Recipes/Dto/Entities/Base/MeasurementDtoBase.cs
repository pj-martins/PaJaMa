//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaJaMa.Recipes.Dto.Entities.Base
{
    using PaJaMa.Common;
    using PaJaMa.Dto;
    using System;
    using System.Collections.Generic;
    
    public abstract class MeasurementDtoBase : EntityDtoBase
    {
        public virtual string MeasurementName { get; set; }
    	
    	// must be overridden to be exposed
        // public virtual ICollection<IngredientMeasurementDto> IngredientMeasurements { get; set; }
    
    	public override string ToString()
    	{
    		return MeasurementName;
    	}
    }
}