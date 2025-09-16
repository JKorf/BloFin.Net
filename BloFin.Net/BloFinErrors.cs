using CryptoExchange.Net.Objects.Errors;

namespace BloFin.Net
{
    internal static class BloFinErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
               new ErrorInfo(ErrorType.SystemError, true, "Internal error", "103013"),

               new ErrorInfo(ErrorType.Unauthorized, false, "Unauthorized", "401"),
               new ErrorInfo(ErrorType.Unauthorized, false, "API key does not exist", "152401"),
               new ErrorInfo(ErrorType.Unauthorized, false, "API key expired", "152402"),
               new ErrorInfo(ErrorType.Unauthorized, false, "IP address not whitelisted", "152406"),
               new ErrorInfo(ErrorType.Unauthorized, false, "Invalid passphrase", "152408"),
               new ErrorInfo(ErrorType.Unauthorized, false, "Signature error", "152409"),

               new ErrorInfo(ErrorType.InvalidTimestamp, false, "Timestamp invalid", "152405"),

               new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", "429"),

               new ErrorInfo(ErrorType.MissingParameter, false, "Missing parameter", "152001"),

               new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter value", "152002", "152005", "152009", "80001"),
               new ErrorInfo(ErrorType.InvalidParameter, false, "One of parameters is required", "152003"),

               new ErrorInfo(ErrorType.InvalidQuantity, false, "Order quantity too large", "542", "102014", "102015"),

               new ErrorInfo(ErrorType.InvalidPrice, false, "Order price not within limits", "102064", "102065"),

               new ErrorInfo(ErrorType.UnknownSymbol, false, "Symbol does not exist", "152014"),

               new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "150004", "103003"),

               new ErrorInfo(ErrorType.InvalidStopParameters, false, "TP trigger price too low", "102037", "102052"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "SL trigger price too high", "102038", "102051"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "TP trigger price too high", "102039", "102053", "102054"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "SL trigger price too low", "102040", "102055"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "Stop loss trigger price should be higher than the order price", "102047"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "Stop loss trigger price must be higher than the best bid price", "102048"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "Take profit trigger price should be lower than the order price", "102049"),
               new ErrorInfo(ErrorType.InvalidStopParameters, false, "Stop loss trigger price must be lower than the best ask price", "102050"),

               new ErrorInfo(ErrorType.UnknownOrder, false, "Order not found or not cancelable", "102068", "1000"),

            ]
            );
    }
}
