using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MoviesApi.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var properyName = bindingContext.ModelName;
            var valuesProvider = bindingContext.ValueProvider.GetValue(properyName);

            if (valuesProvider == null) return Task.CompletedTask;

            try
            {
                var deserializedValue = JsonConvert.DeserializeObject<T>(valuesProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedValue);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(properyName, "Invalid Value");
            }

            return Task.CompletedTask;
        }
    }
}
