using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APItask.Core.Models
{
    /// <summary>
    /// Represents a chatbot conversation with a model and message history.
    /// </summary>
    public class Chatbot
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "gpt-3.5-turbo";

        [JsonPropertyName("messages")]
        public List<ChatMessage> Messages { get; set; } = new();

        /// <summary>
        /// Adds a pre-formatted system message.
        /// </summary>
        public void AddSystemMessage(string content)
            => Messages.Add(ChatMessage.CreateSystemMessage(content));

        /// <summary>
        /// Adds a pre-formatted user message.
        /// </summary>
        public void AddUserMessage(string content)
            => Messages.Add(ChatMessage.CreateUserMessage(content));
    }

    /// <summary>
    /// A single message in a chatbot conversation.
    /// </summary>
    public class ChatMessage
    {
        [JsonPropertyName("role")]
        public ChatRole Role { get; init; }  // Immutable after creation

        [JsonPropertyName("content")]
        public string Content { get; init; } // Immutable after creation

        public ChatMessage(ChatRole role, string content)
        {
            Role = role;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        // Helper methods for common message types
        public static ChatMessage CreateSystemMessage(string content)
            => new(ChatRole.System, content);

        public static ChatMessage CreateUserMessage(string content)
            => new(ChatRole.User, content);
    }

    /// <summary>
    /// Roles a chat participant can have.
    /// </summary>
    public enum ChatRole
    {
        [JsonPropertyName("system")]
        System,
        [JsonPropertyName("user")]
        User,
        [JsonPropertyName("assistant")]
        Assistant
    }

    /// <summary>
    /// Response from a chatbot API call.
    /// </summary>
    public class ChatbotResponse
    {
        [JsonPropertyName("choices")]
        public List<ChatbotChoice> Choices { get; set; } = new();
    }

    /// <summary>
    /// A single response choice from the chatbot.
    /// </summary>
    public class ChatbotChoice
    {
        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; } = new(ChatRole.Assistant, string.Empty);
    }
}