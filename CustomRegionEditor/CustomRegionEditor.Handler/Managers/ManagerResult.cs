using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler
{
    public class ManagerResult<T>
    {
        public ValidationModel ValidationResult { get; private set; }
        public T Object { get; private set; }

        public ManagerResult(ValidationModel validationResult, T obj)
        {
            this.ValidationResult = validationResult;
            this.Object = obj;
        }
    }
}