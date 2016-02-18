using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOLib {
    public class MailClass {
        public void processMail() {
            Logger.info("Проверка почты: ");
            try {
                Pop3Client client = new Pop3Client();
                client.Connect(Settings.Single.smtpServer, 110, false);
                client.Authenticate(Settings.Single.smtpUser, Settings.Single.smtpPassword);
                List<string> msgs = client.GetMessageUids();
                Logger.info(String.Format("Получено {0} сообщений ", msgs.Count));
                for (int i = 0; i < msgs.Count; i++) {
                    try {
                        Logger.info("Чтение письма " + i.ToString());
                        Message msg = client.GetMessage(i);
                        List<MessagePart> files = msg.FindAllAttachments();
                        Logger.info(String.Format("Прикреплено {0} файлов", files.Count));
                        if (files.Count == 1) {
                            Logger.info(String.Format("Обработка файла {0}", files[0].FileName));
                            MessagePart file = files[0];
                            string pbrData = file.GetBodyAsText();
                            PBR.getFromText(pbrData);
                        }
                    }
                    catch (Exception e) {
                        Logger.error("Ошибка при обработке письма " + i);
                    }
                }
            }
            catch (Exception e) {
                Logger.error("Ошибка при работе с почтой " + e.ToString());
            }
        }
    }
}
