using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.MessageHandlers
{
    internal class BloFinRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = BloFinExchange._serializerContext;

        public BloFinRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not BloFinResponse bloFinResponse)
                return null;

            if (bloFinResponse.Code >= 0 && bloFinResponse.Code <= 2)
                return null;

            return new ServerError(bloFinResponse.Code, _errorMapping.GetErrorInfo(bloFinResponse.Code.ToString(), bloFinResponse.Message));
        }

        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            if (httpStatusCode == 401 || httpStatusCode == 403)
                return new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized"));

            using var streamReader = new StreamReader(responseStream);
            return new ServerError(ErrorInfo.Unknown with { Message = await streamReader.ReadToEndAsync().ConfigureAwait(false) });
        }
    }
}
