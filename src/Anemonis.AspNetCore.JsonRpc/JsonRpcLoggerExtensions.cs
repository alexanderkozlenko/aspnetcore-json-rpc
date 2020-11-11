// © Alexander Kozlenko. Licensed under the MIT License.

using System;

using Anemonis.AspNetCore.JsonRpc.Resources;

using Microsoft.Extensions.Logging;

namespace Anemonis.AspNetCore.JsonRpc
{
    internal static class JsonRpcLoggerExtensions
    {
        private static readonly Action<ILogger, int, Exception> s_logHandledNotificationSuccessfully =
            LoggerMessage.Define<int>(
                LogLevel.Debug,
                new(1100, "JSONRPC_HANDLED_NOTIFICATION_SUCCESSFULLY"),
                Strings.GetString("handler.handled_notification_successfully"));
        private static readonly Action<ILogger, int, Exception> s_logHandledRequestWithResultSuccessfully =
            LoggerMessage.Define<int>(
                LogLevel.Debug,
                new(1110, "JSONRPC_HANDLED_REQUEST_WITH_RESULT_SUCCESSFULLY"),
                Strings.GetString("handler.handled_request_with_result_successfully"));
        private static readonly Action<ILogger, long, int, Exception> s_logHandledRequestWithErrorSuccessfully =
            LoggerMessage.Define<long, int>(
                LogLevel.Debug,
                new(1111, "JSONRPC_HANDLED_REQUEST_WITH_ERROR_SUCCESSFULLY"),
                Strings.GetString("handler.handled_request_with_error_successfully"));
        private static readonly Action<ILogger, int, Exception> s_logHandledRequestWithResultAsNotification =
            LoggerMessage.Define<int>(
                LogLevel.Information,
                new(1200, "JSONRPC_HANDLED_REQUEST_WITH_RESULT_AS_NOTIFICATION"),
                Strings.GetString("handler.handled_request_with_result_as_notification"));
        private static readonly Action<ILogger, long, int, Exception> s_logHandledRequestWithErrorAsNotification =
            LoggerMessage.Define<long, int>(
                LogLevel.Information,
                new(1201, "JSONRPC_HANDLED_REQUEST_WITH_ERROR_AS_NOTIFICATION"),
                Strings.GetString("handler.handled_request_with_error_as_notification"));
        private static readonly Action<ILogger, int, Exception> s_logHandledNotificationAsRequest =
            LoggerMessage.Define<int>(
                LogLevel.Warning,
                new(1300, "JSONRPC_HANDLED_NOTIFICATION_AS_REQUEST"),
                Strings.GetString("handler.handled_notification_as_request"));
        private static readonly Action<ILogger, Exception> s_logDataIsInvalid =
            LoggerMessage.Define(
                LogLevel.Error,
                new(1400, "JSONRPC_DATA_INVALID"),
                Strings.GetString("handler.data_invalid"));
        private static readonly Action<ILogger, int, Exception> s_logRequestIsInvalid =
            LoggerMessage.Define<int>(
                LogLevel.Error,
                new(1401, "JSONRPC_REQUEST_INVALID"),
                Strings.GetString("handler.request_invalid"));
        private static readonly Action<ILogger, Exception> s_logBatchHasDuplicateIdentifiers =
            LoggerMessage.Define(
                LogLevel.Error,
                new(1402, "JSONRPC_BATCH_HAS_DUPLICATE_IDENTIFIERS"),
                Strings.GetString("handler.batch_has_duplicate_identifiers"));

        public static void LogHandledNotificationSuccessfully(this ILogger logger, int index)
        {
            s_logHandledNotificationSuccessfully.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithResultSuccessfully(this ILogger logger, int index)
        {
            s_logHandledRequestWithResultSuccessfully.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithErrorSuccessfully(this ILogger logger, long errorCode, int index)
        {
            s_logHandledRequestWithErrorSuccessfully.Invoke(logger, errorCode, index, null);
        }

        public static void LogHandledRequestWithResultAsNotification(this ILogger logger, int index)
        {
            s_logHandledRequestWithResultAsNotification.Invoke(logger, index, null);
        }

        public static void LogHandledRequestWithErrorAsNotification(this ILogger logger, long errorCode, int index)
        {
            s_logHandledRequestWithErrorAsNotification.Invoke(logger, errorCode, index, null);
        }

        public static void LogHandledNotificationAsRequest(this ILogger logger, int index)
        {
            s_logHandledNotificationAsRequest.Invoke(logger, index, null);
        }

        public static void LogDataIsInvalid(this ILogger logger, Exception exception)
        {
            s_logDataIsInvalid.Invoke(logger, exception);
        }

        public static void LogRequestIsInvalid(this ILogger logger, int index, Exception exception)
        {
            s_logRequestIsInvalid.Invoke(logger, index, exception);
        }

        public static void LogBatchHasDuplicateIdentifiers(this ILogger logger)
        {
            s_logBatchHasDuplicateIdentifiers.Invoke(logger, null);
        }
    }
}
