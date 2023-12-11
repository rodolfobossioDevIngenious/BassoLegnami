using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions.ModelBinders
{
	public class FloatModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			object actualValue = null;
			if (string.IsNullOrEmpty(valueResult.FirstValue))
			{
				actualValue = bindingContext.ModelMetadata.IsNullableValueType ? null : (decimal)0;
			}
			else
			{
				try
				{
					actualValue = Convert.ToSingle(valueResult.FirstValue.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
				}
				catch
				{
				}
			}
			bindingContext.Result = ModelBindingResult.Success(actualValue);
			return Task.CompletedTask;
		}
	}
}
