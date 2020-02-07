﻿namespace PlcTag
{
    /// <summary>
    /// Status Codes for all plctaglib operations
    /// </summary>
    public enum OperationStatusCode
    {
        /// <summary>
        /// Operation in progress. Not an error.
        /// </summary>
        STATUS_PENDING = 1,

        /// <summary>
        /// No error.
        /// </summary>
        STATUS_OK = 0,

        /// <summary>
        /// The operation was aborted.
        /// </summary>
        ERR_ERR_ABORT = -1,

        /// <summary>
        /// The operation failed due to incorrect configuration.
        /// Usually returned from a remote system.
        /// </summary>
        ERR_BAD_CONFIG = -2,

        /// <summary>
        /// The connection failed for some reason.
        /// This can mean that the remote PLC was power cycled, for instance.
        /// </summary>
        ERR_BAD_CONNECTION = -3,

        /// <summary>
        /// The data received from the remote PLC was undecipherable or otherwise not able to be processed.
        /// Can also be returned from a remote system that cannot process the data sent to it.
        /// </summary>
        ERR_BAD_DATA = -4,

        /// <summary>
        /// Usually returned from a remote system when something addressed does not exist.
        /// </summary>
        ERR_BAD_DEVICE = -5,

        /// <summary>
        /// Usually returned when the library is unable to connect to a remote system.
        /// </summary>
        ERR_BAD_GATEWAY = -6,

        /// <summary>
        /// A common error return when something is not correct with the tag creation attribute string.
        /// </summary>
        ERR_BAD_PARAM = -7,

        /// <summary>
        /// Usually returned when the remote system returned an unexpected response.
        /// </summary>
        ERR_BAD_REPLY = -8,

        /// <summary>
        /// Usually returned by a remote system when something is not in a good state.
        /// </summary>
        ERR_BAD_STATUS = -9,

        /// <summary>
        /// An error occurred trying to close some resource.
        /// </summary>
        ERR_CLOSE = -10,

        /// <summary>
        /// An error occurred trying to create some internal resource.
        /// </summary>
        ERR_CREATE = -11,

        /// <summary>
        /// An error returned by a remote system when something is incorrectly duplicated
        /// (i.e. a duplicate connection ID).
        /// </summary>
        ERR_DUPLICATE = -12,

        /// <summary>
        /// An error was returned when trying to encode some data such as a tag name.
        /// </summary>
        ERR_ENCODE = -13,

        /// <summary>
        /// An internal library error.
        /// It would be very unusual to see this.
        /// </summary>
        ERR_MUTEX_DESTROY = -14,

        /// <summary>
        /// An internal library error.
        /// It would be very unusual to see this.
        /// </summary>
        ERR_MUTEX_INIT = -15,

        /// <summary>
        /// An internal library error.
        /// It would be very unusual to see this.
        /// </summary>
        ERR_MUTEX_LOCK = -16,

        /// <summary>
        /// An internal library error.
        /// It would be very unusual to see this.
        /// </summary>
        ERR_MUTEX_UNLOCK = -17,

        /// <summary>
        /// Often returned from the remote system when an operation is not permitted.
        /// </summary>
        ERR_NOT_ALLOWED = -18,

        /// <summary>
        /// Often returned from the remote system when something is not found.
        /// </summary>
        ERR_NOT_FOUND = -19,

        /// <summary>
        /// Returned when a valid operation is not implemented.
        /// </summary>
        ERR_NOT_IMPLEMENTED = -20,

        /// <summary>
        /// Returned when expected data is not present.
        /// </summary>
        ERR_NO_DATA = -21,

        /// <summary>
        /// Similar to NOT_FOUND.
        /// </summary>
        ERR_NO_MATCH = -22,

        /// <summary>
        /// Returned by the library when memory allocation fails.
        /// </summary>
        ERR_NO_MEM = -23,

        /// <summary>
        /// Returned by the remote system when some resource allocation fails.
        /// </summary>
        ERR_NO_RESOURCES = -24,

        /// <summary>
        /// Usually an internal error, but can be returned when an invalid handle is used with an API call.
        /// </summary>
        ERR_NULL_PTR = -25,

        /// <summary>
        /// Returned when an error occurs opening a resource such as a socket.
        /// </summary>
        ERR_OPEN = -26,

        /// <summary>
        /// Usually returned when trying to write a value into a tag outside of the tag data bounds.
        /// </summary>
        ERR_OUT_OF_BOUNDS = -27,

        /// <summary>
        /// Returned when an error occurs during a read operation.
        /// Usually related to socket problems.
        /// </summary>
        ERR_READ = -28,

        /// <summary>
        /// An unspecified or untranslatable remote error causes this.
        /// </summary>
        ERR_REMOTE_ERR = -29,

        /// <summary>
        /// An internal library error. If you see this, it is likely that everything is about to crash.
        /// </summary>
        ERR_THREAD_CREATE = -30,

        /// <summary>
        /// Another internal library error.
        /// It is very unlikely that you will see this.
        /// </summary>
        ERR_THREAD_JOIN = -31,

        /// <summary>
        /// An operation took too long and timed out.
        /// </summary>
        ERR_TIMEOUT = -32,

        /// <summary>
        /// More data was returned than was expected.
        /// </summary>
        ERR_TOO_LARGE = -33,

        /// <summary>
        /// Insufficient data was returned from the remote system.
        /// </summary>
        ERR_TOO_SMALL = -34,

        /// <summary>
        /// The operation is not supported on the remote system.
        /// </summary>
        ERR_UNSUPPORTED = -35,

        /// <summary>
        /// A Winsock-specific error occurred (only on Windows).
        /// </summary>
        ERR_WINSOCK = -36,

        /// <summary>
        /// An error occurred trying to write, usually to a socket.
        /// </summary>
        ERR_WRITE = -37,
    }
}
