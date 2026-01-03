using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guidex_Backend.Infrastructure.Prompts
{
    public class PromptTemplate
{
    private readonly string _systemMessage;
    private List<Message> _history = new();
    private List<string> _contexts = new();

    public PromptTemplate(string systemMessage)
    {
        _systemMessage = systemMessage;
    }

    // Optional: add past messages
    public void AddMessage(List<Message> messages)
    {
        _history = messages;
    }
    // Optional: add context
    public void AddContext(List<string> contexts)
    {
        _contexts = contexts;
    }

    // Required: current user input
    public string Render(string currentUserMessage)
    {
        var sb = new StringBuilder();

        // system
        sb.AppendLine("<system>");
        sb.AppendLine(_systemMessage);
        sb.AppendLine("</system>");
        sb.AppendLine();

        sb.AppendLine("<context>");
        foreach (var context in _contexts)
        {
            sb.AppendLine(context);
        }
        sb.AppendLine("</context>");

        // history (optional)
        sb.AppendLine("<history>");
        foreach (var msg in _history)
        {
            sb.AppendLine($"<{msg.Role}>");
            sb.AppendLine(msg.Content);
            sb.AppendLine($"</{msg.Role}>");
            sb.AppendLine();
        }
        sb.AppendLine("</history>");

        // current user
        sb.AppendLine("<user>");
        sb.AppendLine(currentUserMessage);
        sb.AppendLine("</user>");
        sb.AppendLine();

        // assistant start
        sb.AppendLine("<assistant>");

        return sb.ToString();
    }
}

}