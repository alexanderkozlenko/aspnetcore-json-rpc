// © Alexander Kozlenko. Licensed under the MIT License.

using System;

using Anemonis.AspNetCore.JsonRpc.Resources;

using Microsoft.Extensions.Logging;

namespace Anemonis.AspNetCore.JsonRpc
{
    internal static class JsonRpcLoggerExtensions
    {
        private static readonly Action<ILogger, Exception> _logDataIsMessage =
            LoggerMessage.Define(
                LogLevel.Trace,
                new EventId(1000, "JSONRPC_DATA_IS_MESSAGE"),
                Strings.GetString("handler.data_is_message"));
        private static readonly Action<ILogger, int, Exception> _logDataIsBatch =
            LoggerMessage.Define<int>(
                LogLevel.Trace,
                new EventId(1001, "JSONRPC_DATA_IS_BATCH"),
                Strings.GetString("handler.data_is_batch"));
        private static readonly Action<ILogger, int, Exception> _logHandledNotificationSuccessfully =
            LoggerMessage.Define<int>(
                LogLevel.Debug,
                new EventId(1100, "JSONRPC_HANDLED_NOTIFICATION_SUCCESSFULLY"),
                Strings.GetString("handler.handled_notification_successfully"));
        private static readonly Action<ILogger, int, Exception> _logHandledRequestWithResultSuccessfully =
            LoggerMessage.Define<int>(
                LogLevel.Debug,
                new EventId(1110, "JSONRPC_HANDLED_REQUEST_WITH_RESULT_SUCCESSFULLY"),
                Strings.GetString("handler.handled_request_with_result_successfully"));
        private static readonly Action<ILogger, long, int, Exception> _logHandledRequestWithErrorSuccessfully =
            LoggerMessage.Define<long, int>(
                LogLevel.Debug,
                new EventId(1111, "JSONRPC_HANDLED_REQUEST_WITH_ERROR_SUCCESSFULLY"),
                Strings.GetString("handler.handled_request_with_error_successfully"));
        private static readonly Action<ILogger, int, Exception> _logHandledRequestWithResultAsNotification =
            LoggerMessage.Define<int>(
                LogLevel.Information,
                new EventId(1200, "JSONRPC_HANDLED_REQUEST_WITH_RESULT_AS_NOTIFICATION"),
                Strings.GetString("handler.handled_request_with_result_as_notification"));
        private static readonly Action<ILogger, long, int, Exception> _logHandledRequestWithErrorAsNotification =
            LoggerMessage.Define<long, int>(
                LogLevel.Information,
                new EventId(1201, "JSONRPC_HANDLED_REQUEST_WITH_ERROR_AS_NOTIFICATION"),
                Strings.GetString("handler.handled_request_with_error_as_notification"));
        private static readonly Action<ILogger, int, Exception> _logHandledNotificationAsRequest =
            LoggerMessage.Define<int>(
                LogLevel.Warning,
                new EventId(1300, "JSONRPC_HANDLED_NOTIFICATION_AS_REQUEST"),
                Strings.GetString("handler.handled_notification_as_request"));
        private static readonly Action<ILogger, Exception> _logDataIsInvalid =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1400, "JSONRPC_DATA_INVALID"),
                Strings.GetString("handler.data_invalid"));
        private static readonly Action<ILogger, int, Exception> _logRequestIsInvalid =
            LoggerMessage.Define<int>(
                LogLevel.Error,
                new EventId(1401, "JSONRPC_REQUEST_INVALID"),
                Strings.GetString("handler.request_invalid"));
        private static readonly Action<ILogger, Exception> _logBatchHasDuplicateIdentifiers =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1402, "JSONRPC_BATCH_HAS_DUPLICATE_IDENTIFIERS"),
                Strings.GetString("handler.batch_has_duplicate_identifiers"));

        public static void LogDataIsMessage(this ILogger logger)
        {
            _logDataIsMessage.Invoke(logger, null);
        }

        public static void LogDataIsBatch(this ILogger logger, int count)
        {
            _logDataIsBatch.Invoke(logger, count, null);
        }

        public static void LogHandledNotificationSuccessfully(this ILogger logger, int index)
        {
            _logHandledNotificationSuccessfully.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithResultSuccessfully(this ILogger logger, int index)
        {
            _logHandledRequestWithResultSuccessfully.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithErrorSuccessfully(this ILogger logger, long errorCode, int index)
        {
            _logHandledRequestWithErrorSuccessfully.Invoke(logger, errorCode, index, null);
        }

        public static void LogHandledRequestWithResultAsNotification(this ILogger logger, int index)
        {
            _logHandledRequestWithResultAsNotification.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithErrorAsNotification(this ILogger logger, long errorCode, int index)
        {
            _logHandledRequestWithErrorAsNotification.Invoke(logger, errorCode, index, null);
        }

        public static void LogHandledNotificationAsRequest(this ILogger logger, int index)
        {
            _logHandledNotificationAsRequest.Invoke(logger, index, null);
        }

        public static void LogDataIsInvalid(this ILogger logger, Exception exception)
        {
            _logDataIsInvalid.Invoke(logger, exception);
        }

        public static void LogRequestIsInvalid(this ILogger logger, int index, Exception exception)
        {
            _logRequestIsInvalid.Invoke(logger, index, exception);
        }

        public static void LogBatchHasDuplicateIdentifiers(this ILogger logger)
        {
            _logBatchHasDuplicateIdentifiers.Invoke(logger, null);
        }
    }
}
