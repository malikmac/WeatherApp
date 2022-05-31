namespace WeatherApp.ApiService
{
    public class ServiceResponse
    {
        protected IReadOnlyDictionary<string, string?> _errors = new Dictionary<string, string?> { };

        public static ServiceResponse Success => new() { Succeeded = true };

        public static ServiceResponse Failed(string? key, string? value)
            => ServiceResponse.Failed(new Dictionary<string, string?> { { key??string.Empty, value } });

        public static ServiceResponse Failed(IReadOnlyDictionary<string, string?>? errors = null)
        {
            var result = new ServiceResponse { Succeeded = false };
            if (errors != null) result._errors = errors;
            return result;
        }

        public bool Succeeded { get; set; }

        public IReadOnlyDictionary<string, string?> Errors => _errors;
    }

    public class ServiceResponse<TResult> : ServiceResponse
    {
        public static ServiceResponse<TResult> Success(TResult data)
        {
            var result = new ServiceResponse<TResult> { Succeeded = true };
            if (data != null) result.Result = data;
            return result;
        }

        public static ServiceResponse<TResult> Failed(string? key, string? value)
            => ServiceResponse<TResult>.Failed(new Dictionary<string, string?> { { key ?? string.Empty, value } });

        public static ServiceResponse<TResult> Failed(IReadOnlyDictionary<string, string?>? errors = null)
        {
            var result = new ServiceResponse<TResult> { Succeeded = false };
            if (errors != null) result._errors = errors;
            return result;
        }

        public TResult? Result { get; set; }
    }
}
