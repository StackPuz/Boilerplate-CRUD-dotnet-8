using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            var result = context.ValueProvider.GetValue(context.ModelName);
            if (result != ValueProviderResult.None)
            {
                context.ModelState.SetModelValue(context.ModelName, result);
                var value = result.FirstValue;
                try {
                    if (context.ModelMetadata.UnderlyingOrModelType == typeof(TimeSpan)) {
                        context.Result = ModelBindingResult.Success(Util.GetTime(value));
                    }
                    else if (context.ModelMetadata.UnderlyingOrModelType == typeof(DateTime)) {
                        context.Result = ModelBindingResult.Success(value.Length == Util.dateFormat.Length ? Util.GetDate(value) : Util.GetDateTime(value));
                    }
                }
                catch (Exception e) {
                    context.ModelState.AddModelError(context.ModelName, e, context.ModelMetadata);
                }
            }
            return Task.CompletedTask;
        }
    }

    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime) || context.Metadata.UnderlyingOrModelType == typeof(TimeSpan))
            {
                return new DateTimeModelBinder();
            }
            return null;
        }
    }
}