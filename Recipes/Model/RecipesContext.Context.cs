﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaJaMa.Recipes.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    
    using AutoMapper;
    
    using PaJaMa.Data;
    using PaJaMa.Recipes.Model.Entities;
    using PaJaMa.Recipes.Model.Dto;
        
    public partial class RecipesContextBase : DbContextBase
    {
    	public RecipesContextBase()
            : base("name=RecipesContext")
        {
    	}
    
    	protected override void createMaps(IMapperConfiguration cfg)
    	{
    		Mappings.Add(typeof(Ingredient), cfg.CreateMap<Ingredient, IngredientDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.IngredientID)));
    		Mappings.Add(typeof(IngredientMeasurement), cfg.CreateMap<IngredientMeasurement, IngredientMeasurementDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.IngredientMeasurementID)));
    		Mappings.Add(typeof(IngredientMeasurementAlternate), cfg.CreateMap<IngredientMeasurementAlternate, IngredientMeasurementAlternateDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.IngredientMeasurementAlternateID)));
    		Mappings.Add(typeof(Measurement), cfg.CreateMap<Measurement, MeasurementDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.MeasurementID)));
    		Mappings.Add(typeof(Recipe), cfg.CreateMap<Recipe, RecipeDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.RecipeID)));
    		Mappings.Add(typeof(RecipeImage), cfg.CreateMap<RecipeImage, RecipeImageDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.RecipeImageID)));
    		Mappings.Add(typeof(RecipeIngredientMeasurement), cfg.CreateMap<RecipeIngredientMeasurement, RecipeIngredientMeasurementDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.RecipeIngredientMeasurementID)));
    		Mappings.Add(typeof(RecipeSource), cfg.CreateMap<RecipeSource, RecipeSourceDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.RecipeSourceID)));
    		Mappings.Add(typeof(User), cfg.CreateMap<User, UserDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.UserID)));
    		Mappings.Add(typeof(UserRecipe), cfg.CreateMap<UserRecipe, UserRecipeDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.UserRecipeID)));
    		Mappings.Add(typeof(RecipeSearch), cfg.CreateMap<RecipeSearch, RecipeSearchDto>().ForMember(x => x.ID, y => y.MapFrom(e => e.RecipeID)));
    
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
    		modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<IngredientMeasurement> IngredientMeasurements { get; set; }
        public virtual DbSet<IngredientMeasurementAlternate> IngredientMeasurementAlternates { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeImage> RecipeImages { get; set; }
        public virtual DbSet<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }
        public virtual DbSet<RecipeSource> RecipeSources { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRecipe> UserRecipes { get; set; }
        public virtual DbSet<RecipeSearch> RecipeSearches { get; set; }
    }
}
