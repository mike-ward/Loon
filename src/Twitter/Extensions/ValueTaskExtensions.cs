using System.Threading.Tasks;

namespace Twitter.Extensions
{
    public static class ValueTaskExtensions
    {
        public static ValueTask AsValueTask<T>(this ValueTask<T> valueTask)
        {
            return valueTask.IsCompletedSuccessfully
                ? default
                : new ValueTask(valueTask.AsTask());
        }
    }
}