using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    /// <summary>
    /// Interface for Chatbot service to handle chatbot interactions.
    /// </summary>
    public interface IChatbotService
    {
        /// <summary>
        /// Gets the chatbot response for a given message.
        /// </summary>
        /// <param name="message">The message to send to the chatbot.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the chatbot response.</returns>
        Task<object> GetChatbotResponse(string message);
    }
}
