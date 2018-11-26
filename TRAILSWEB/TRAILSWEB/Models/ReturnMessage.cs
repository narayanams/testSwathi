using System;
using System.Collections.Generic;

namespace TRAILSWEB.Models
{
    public enum MessageType
    {
        Error = 1,
        Information = 2,
        Question = 3,
        Warning = 4
    }

    public class ReturnValue
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class ReturnMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public List<ReturnValue> ReturnValues { get; set; }

        public ReturnMessage()
        {
            this.Message = "Success";
            this.MessageType = MessageType.Information;
            this.Title = Enum.GetName(typeof(MessageType), this.MessageType);
            this.ReturnValues = null;
        }

        public ReturnMessage(string messageText, MessageType messageType, string messageTitle = null, List<ReturnValue> returnValues = null)
        {
            this.Message = messageText;
            this.MessageType = messageType;

            if (messageTitle != null)
            {
                this.Title = messageTitle;
            }
            else
            {
                this.Title = Enum.GetName(typeof(MessageType), this.MessageType);
            }

            this.ReturnValues = returnValues;
        }
    }
}
