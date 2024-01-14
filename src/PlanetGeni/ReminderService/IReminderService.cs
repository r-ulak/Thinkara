using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ReminderService
{
    [ServiceContract]
    public interface IReminderService
    {
        [OperationContract(IsOneWay = true)]
        [WebInvoke(Method = "POST",
                    BodyStyle = WebMessageBodyStyle.Bare
                        , UriTemplate = "/RunTaskReminder")]
        void RunTaskReminder();


        [OperationContract(IsOneWay = true)]
        [WebInvoke(Method = "POST",
                    BodyStyle = WebMessageBodyStyle.Bare
                        , UriTemplate = "/RunTaskReminderByTaskType")]
        void RunTaskReminderByType(int taskType);


    }
}
