﻿namespace Jupyter.Server
{
    using Microsoft.Extensions.Logging;
    using Jupyter.Messages;
    using NetMQ.Sockets;
    using Newtonsoft.Json;

    public class KernelInfoHandler : IMessageHandler
    {
        private readonly ILogger _logger;

        private readonly MessageSender _sender;

        public KernelInfoHandler(ILogger logger, MessageSender messageSender)
        {
            _logger = logger;
            _sender = messageSender;
        }

        public void HandleMessage(Message message, RouterSocket serverSocket, PublisherSocket ioPub)
        {
            Message replyMessage = new Message(MessageType.KernelInfoReply, CreateKernelInfoReply(), message.Header);

            _logger.LogInformation("Sending kernel_info_reply");
            _sender.Send(replyMessage, serverSocket);
        }

        private KernelInfoReplyContent CreateKernelInfoReply()
        {
            KernelInfoReplyContent kernelInfoReply = new KernelInfoReplyContent()
            {
                ProtocolVersion = "5.0",
                Implementation = "iPowerShell",
                ImplementationVersion = "0.0.1",
                LanguageInfo = new LanguageInfoContent()
                {
                    Name = "PowerShell",
                    Version = "5.0",
                    MimeType = "text/powershell",
                    FileExtension = ".ps1",
                    PygmentsLexer = "powershell",
                    CodemirrorMode = "powershell"
                }
            };

            return kernelInfoReply;
        }
    }
}