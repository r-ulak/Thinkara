using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserTaskDetailsDTORepository : IUserTaskDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public UserTaskDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public UserTaskDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public UserTask GetTaskById(Guid taskId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<UserTask>(dictionary);
        }

        public bool IsIncomepleteTask(Guid taskId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmTaskId", taskId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPCheckIncompleteTask, dictionary, "cnt"));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public IQueryable<UserTaskDetailsDTO> GetTaskList(int userId, Guid? lastTaskId = null,
            DateTime? lastCreatedAt = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmlastTaskId", lastTaskId);
            dictionary.Add("lastCreatedAt", lastCreatedAt);
            dictionary.Add("parmLimit", AppSettings.TaskLimit);

            IEnumerable<UserTaskDetailsDTO> userTaskList =
                spContext.GetSqlData<UserTaskDetailsDTO>(AppSettings.SPUserTaskList, dictionary);
            return userTaskList.AsQueryable();
        }


        public int SaveTask(UserTask task)
        {
            try
            {

                return spContext.Add(task);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save task");
                return -1;
            }
        }
        public TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder startpartyreminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 8
            };

            return startpartyreminder;
        }

        public int SaveReminder(TaskReminder taskreminder)
        {
            try
            {

                return spContext.AddUpdate(taskreminder);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save task reminder");
                return -1;
            }
        }

        public int GetTaskCountById(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.GetTaskCountById, dictionary, "cnt"));
            return count;
        }
        public void SendBulkTaskAndReminder(UserTask task, int[] userId, TaskReminder taskReminder)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", task.TaskId);
            dictionary.Add("parmAssignerUserId", task.AssignerUserId);
            dictionary.Add("parmCompletionPercent", task.CompletionPercent);
            dictionary.Add("parmFlagged", task.Flagged);
            dictionary.Add("parmStatus", task.Status);
            dictionary.Add("parmParms", task.Parms);
            dictionary.Add("parmTaskTypeId", task.TaskTypeId);
            dictionary.Add("parmDueDate", task.DueDate);
            dictionary.Add("parmDefaultResponse", task.DefaultResponse);
            dictionary.Add("parmPriority", task.Priority);
            dictionary.Add("parmCreatedAt", task.CreatedAt);
            dictionary.Add("parmReminderFrequency", taskReminder.ReminderFrequency);
            dictionary.Add("parmReminderTransPort", taskReminder.ReminderTransPort);
            dictionary.Add("parmStartDate", taskReminder.StartDate);
            dictionary.Add("parmEndDate", taskReminder.EndDate);


            dictionary.Add("parmUserIdList", string.Join(",", userId));
            spContext.ExecuteStoredProcedure(AppSettings.SPSendBulkTaskAndReminder, dictionary);
        }
        public void SendBulkTaskListAndReminder(UserTask task, int[] userId, TaskReminder taskReminder, Guid[] taskIds)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskIdList", string.Join(",", taskIds));
            dictionary.Add("parmAssignerUserId", task.AssignerUserId);
            dictionary.Add("parmCompletionPercent", task.CompletionPercent);
            dictionary.Add("parmFlagged", task.Flagged);
            dictionary.Add("parmStatus", task.Status);
            dictionary.Add("parmParms", task.Parms);
            dictionary.Add("parmTaskTypeId", task.TaskTypeId);
            dictionary.Add("parmDueDate", task.DueDate);
            dictionary.Add("parmDefaultResponse", task.DefaultResponse);
            dictionary.Add("parmPriority", task.Priority);
            dictionary.Add("parmCreatedAt", task.CreatedAt);
            dictionary.Add("parmReminderFrequency", taskReminder.ReminderFrequency);
            dictionary.Add("parmReminderTransPort", taskReminder.ReminderTransPort);
            dictionary.Add("parmStartDate", taskReminder.StartDate);
            dictionary.Add("parmEndDate", taskReminder.EndDate);


            dictionary.Add("parmUserIdList", string.Join(",", userId));
            spContext.ExecuteStoredProcedure(AppSettings.SPSendBulkTaskListAndReminder, dictionary);
        }

        public IEnumerable<UserTaskDTO> GetIncompletePastDueTask()
        {

            return
                spContext.GetSqlDataNoParms<UserTaskDTO>(AppSettings.SPGetIncompletePastDueTask);
        }

    }
}
