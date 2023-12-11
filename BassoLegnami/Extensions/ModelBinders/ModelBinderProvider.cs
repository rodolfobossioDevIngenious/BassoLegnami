using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions.ModelBinders
{
	public class ModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
			{
				return new DecimalModelBinder();
			}

			if (context.Metadata.ModelType == typeof(double) || context.Metadata.ModelType == typeof(double?))
			{
				return new DoubleModelBinder();
			}

			if (context.Metadata.ModelType == typeof(float) || context.Metadata.ModelType == typeof(float?))
			{
				return new FloatModelBinder();
			}

            if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeModelBinder();
            }

            return null;
		}
	}
}
