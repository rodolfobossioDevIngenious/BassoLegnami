using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static In.Core.Configuration.Job;

namespace BassoLegnami.Jobs.Tx
{
	[DisableConcurrentExecution(int.MaxValue)]
	public class EmailTx : In.Core.Models.IJob
	{
		public string JobID => nameof(EmailTx);

		private readonly Model.Data.IUnitOfWork _unitOfWork;

		public EmailTx(Model.Data.IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[DisableConcurrentExecution(timeoutInSeconds: int.MaxValue)]
		public bool Execute(IEnumerable<JobDataRow> data)
		{
			if (!_unitOfWork.EmailMessagesRepository.Any(d => d.Status == Model.Models.EmailMessage.EmailMessageStatusEnum.ToSend))
			{
				return true;
			}

			bool test = (_unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.TestMode").Value ?? "0") == "1";
			string smtpServer = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.Server").Value ?? string.Empty;
			int smtpServerPort = Convert.ToInt32(_unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.ServerPort").Value ?? "25");
			string smtpUsername = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.Username").Value ?? string.Empty;
			string smtpPassword = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.Password").Value ?? string.Empty;
			bool smtpUseSSL = (_unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.UseSSL").Value ?? "0") == "1";
			string smtpSenderName = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.SenderName").Value ?? string.Empty;
			string smtpSenderEmail = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.SenderEmail").Value ?? string.Empty;
			string smtpTestEmail = _unitOfWork.SettingsRepository.GetByKey("TxRx.Tx.EmailTx.TestEmail").Value ?? string.Empty;

			using (MailKit.Net.Smtp.SmtpClient smtp = new())
			using (MailKit.Net.Smtp.SmtpClient certifiedSmtp = new())
			{
				smtp.ServerCertificateValidationCallback = (_, __, ___, ____) => true;
				smtp.Connect(smtpServer, smtpServerPort, smtpUseSSL ? MailKit.Security.SecureSocketOptions.Auto : MailKit.Security.SecureSocketOptions.None);
				smtp.Authenticate(smtpUsername, smtpPassword);

				List<Model.Models.EmailMessage> emails = _unitOfWork.EmailMessagesRepository.GetAll()
					.Where(r => r.Status == Model.Models.EmailMessage.EmailMessageStatusEnum.ToSend)
					.Include(r => r.Attachments)
					.OrderBy(r => r.CreatedOn)
					.ToList();

				foreach (Model.Models.EmailMessage email in emails)
				{
					try
					{
						System.Net.Mail.MailMessage message = new()
						{
							IsBodyHtml = true,
							Subject = email.Subject,
							From = new System.Net.Mail.MailAddress(smtpSenderEmail, smtpSenderName),
							Body = email.Message
						};
						if (test)
						{
							message.To.Add(smtpTestEmail);
						}
						else
						{
							email.RecipientEmails.Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(message.To.Add);
							email.CCRecipientEmails.Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(message.CC.Add);
							email.BCCRecipientEmails.Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(message.Bcc.Add);
						}
						foreach (Model.Models.Support.File attachment in email.Attachments)
						{
							System.IO.MemoryStream attachmentStream = new(_unitOfWork.FilesRepository.GetFile(attachment));
							attachmentStream.Seek(0, System.IO.SeekOrigin.Begin);
							message.Attachments.Add(new System.Net.Mail.Attachment(attachmentStream, attachment.GetFilenameForDownload(), "application/octet-stream"));
						}

						smtp.Send((MimeKit.MimeMessage)message);
						email.SendDate = DateTime.Now;
						email.Status = Model.Models.EmailMessage.EmailMessageStatusEnum.Sended;
					}
					catch (Exception ex)
					{
						email.ErrorMessage = ex.ToString();
						email.Status = Model.Models.EmailMessage.EmailMessageStatusEnum.NotSended;
					}
					_unitOfWork.EmailMessagesRepository.Update(email, email.EmailMessageID);
				}
				if (emails.Count > 0)
				{
					_unitOfWork.Save();
				}
			}

			return true;
		}
	}
}
