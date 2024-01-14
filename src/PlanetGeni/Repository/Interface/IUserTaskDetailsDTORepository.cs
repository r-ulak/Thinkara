using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserTaskDetailsDTORepository
    {
        IQueryable<UserTaskDetailsDTO> GetTaskList(int userId, Guid? lastTaskId = null,
   DateTime? lastCreatedAt = null);
        int SaveTask(UserTask task);
        UserTask GetTaskById(Guid taskId, int userId);
        bool IsIncomepleteTask(Guid taskId, int userId);
        TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId);
        int SaveReminder(TaskReminder taskreminder);
        int GetTaskCountById(Guid taskId);
        void SendBulkTaskAndReminder(UserTask task, int[] userId, TaskReminder taskReminder);
        void SendBulkTaskListAndReminder(UserTask task, int[] userId, TaskReminder taskReminder, Guid[] taskIds);
         IEnumerable<UserTaskDTO> GetIncompletePastDueTask();
    }
}
