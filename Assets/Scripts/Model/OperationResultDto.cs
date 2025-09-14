using System;

namespace UIProgrammerTest.Model
{
    public readonly struct OperationResultDto
    {
        public Guid Guid { get; }
        public bool Success { get; }
        public string Message { get; }

        public OperationResultDto(System.Guid guid, bool success, string message)
        {
            Guid = guid;
            Success = success;
            Message = message;
        }
    }
}
