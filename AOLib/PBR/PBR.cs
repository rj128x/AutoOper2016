using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOLib {
    public class PBR {
        public SortedDictionary<DateTime, double> PBRData { get; set; }

        public static Dictionary<int, PBR> getFromText(string text) {
            Logger.info("Разбор ПБР: ");
            Logger.info(text);
            Dictionary<int, PBR> result = new Dictionary<int, PBR>();
            try {
                string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines) {
                    try {
                        string[] parts = line.Split(new char[] { ';' });

                        int groupID = Int32.Parse(parts[0]);

                        string datePart = parts[1];
                        string yearPart = datePart.Substring(0, 4);
                        string monthPart = datePart.Substring(4, 2);
                        string dayPart = datePart.Substring(6, 2);
                        string hourPart = datePart.Substring(8, 2);
                        string minPart = datePart.Substring(10, 2);

                        string dateStr = String.Format("{0}.{1}.{2} {3}:{4}:00", dayPart, monthPart, yearPart, hourPart, minPart);
                        DateTime date = DateTime.Parse(dateStr);

                        int power = Int32.Parse(parts[2]);

                        if (!result.ContainsKey(groupID)) {
                            result.Add(groupID, new PBR());
                        }
                        if (!result[groupID].PBRData.ContainsKey(date)) {
                            result[groupID].PBRData.Add(date, power);
                        }
                    }
                    catch (Exception e) {
                        Logger.error("Ошибка при анализе строки " + line);
                        Logger.error(e.ToString());
                    }
                }
            }
            catch (Exception e) {
                Logger.error("Ошибка при разборе ПБР: " + e.ToString());
            }
            return result;
        }

        public void WriteToDB() {

        }
    }
}
