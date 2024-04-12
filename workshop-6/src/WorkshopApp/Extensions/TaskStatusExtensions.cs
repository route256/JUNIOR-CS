using GrpcTaskStatus = WorkshopApp.Proto.Client.TaskStatus;
using BllTaskStatus = WorkshopApp.Bll.Enums.TaskStatus;

namespace WorkshopApp.Extensions;

public static class TaskStatusExtensions
{
    public static GrpcTaskStatus ToGrpc(this BllTaskStatus src)
    {
        return src switch
        {
            BllTaskStatus.Draft => GrpcTaskStatus.Draft,
            BllTaskStatus.ToDo => GrpcTaskStatus.ToDo,
            BllTaskStatus.InProgress => GrpcTaskStatus.InProgress,
            BllTaskStatus.Done => GrpcTaskStatus.Done,
            BllTaskStatus.Canceled => GrpcTaskStatus.Canceled,
            _ => throw new ArgumentOutOfRangeException(nameof(src), src, null)
        };
    }
}