//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace PaJaMa.Recipes.Model.Dto
//{
//	public class RecipeIngredientDto : RecipeIngredientDtoBase
//	{
//		[DataMember]
//		public IngredientMeasurementAlternateDto[] Alternates { get; set; }

//		[DataMember]
//		public IngredientMeasurementAlternateDto ActiveAlternate { get; set; }

//		[DataMember]
//		public bool Exclude { get; set; }

//		public IngredientMeasurementDto GetActiveIngredient()
//		{
//			return ActiveAlternate == null ? Ingredient : ActiveAlternate.Ingredient;
//		}

//		protected override string getMeasurementName()
//		{
//			return GetActiveIngredient().MeasurementName;
//		}
//	}

//	/*
//	 * [DataContract(Name = "RecipeIngredientSearch")]
//	public class RecipeIngredientSearchDto
//	{
//		[DataMember]
//		public float Quantity { get; set; }
//		[DataMember]
//		public IngredientMeasurementDto Ingredient { get; set; }

//		public string Amount
//		{
//			get { return getFriendlyQuantity(Quantity) + " " + getMeasurementName(); }
//		}

//		protected virtual string getMeasurementName()
//		{
//			return Ingredient.MeasurementName;
//		}

//		private string getFriendlyQuantity(float qty)
//		{
//			if (qty == 0) return string.Empty;
//			var friendlyQty = string.Empty;
//			var tempQty = qty;
//			float floorQty = (float)Math.Floor(qty);
//			if (floorQty > 0)
//			{
//				friendlyQty += floorQty.ToString() + " ";
//				tempQty -= floorQty;
//				if (tempQty == 0)
//					return friendlyQty.Trim();
//			}
//			// TODO: create elegant way to convert decimals to fractions
//			if (tempQty == 0.5)
//				friendlyQty += "1/2";
//			else if (tempQty == 0.25)
//				friendlyQty += "1/4";
//			else if (tempQty == 0.75)
//				friendlyQty += "3/4";
//			else if (tempQty > 0.32 && tempQty < 0.34)
//				friendlyQty += "1/3";
//			else if (tempQty == 0.125)
//				friendlyQty += "1/8";
//			else if (tempQty >= 0.65 && tempQty <= 0.68)
//				friendlyQty += "2/3";
//			else if (tempQty == 0.2)
//				friendlyQty += "1/5";
//			else if (tempQty == 0.375)
//				friendlyQty += "3/8";
//			else if (tempQty == 0.625)
//				friendlyQty += "5/8";
//			else if (tempQty >= 0.165 && tempQty <= 0.168)
//				friendlyQty += "1/6";
//			else if (tempQty == 0.875)
//				friendlyQty += "7/8";
//			else
//				friendlyQty = qty.ToString();

//			return friendlyQty.Trim();
//		}

//	}
//	 * */
//}
