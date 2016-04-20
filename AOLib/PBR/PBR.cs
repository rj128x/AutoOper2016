using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOLib
{
	public class PBR
	{
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
					} catch (Exception e) {
						Logger.error("Ошибка при анализе строки " + line);
						Logger.error(e.ToString());
					}
				}
			} catch (Exception e) {
				Logger.error("Ошибка при разборе ПБР: " + e.ToString());
			}
			return result;
		}

		public void TimeInterpolate(int stepMin) {
			SortedDictionary<DateTime, double> result = new SortedDictionary<DateTime, double>();
			DateTime ds;
			foreach (KeyValuePair<DateTime, double> rec in PBRData) {
				result.Add(rec.Key, rec.Value);
			}

			ds = PBRData.First().Key;

			foreach (KeyValuePair<DateTime, double> rec in PBRData) {
				if (rec.Key != ds) {
					DateTime de = rec.Key;

					double p1 = PBRData[ds];
					double p2 = rec.Value;

					double mins = (de.Ticks - ds.Ticks) / 10000000 * 60;
					double step = (p2 - p1) / mins;

					DateTime date = ds.AddMinutes(0);
					double sumPLow = 0;
					double sumTLow = 0;
					while (date < de) {
						if (!result.ContainsKey(date)) {
							double min = (date.Ticks - ds.Ticks) / 10000000 * 60;
							double p = p1 + step * min;
							if (p > 0 && p < 35) {
								sumPLow += p*stepMin;
								sumTLow += stepMin;								
							}
							result.Add(date, p);
						}
						date = date.AddMinutes(stepMin);
					}

					if (sumPLow > 0) {
						double needTime = sumPLow/35/60;
						if (p1 < p2) {
							DateTime start35 = ds.AddMinutes(sumTLow - needTime);
							foreach (KeyValuePair<DateTime,double> r in result) {
								if (r.Key>=ds && r.Key < start35) {
									result[r.Key] = 0;
								}
							}
							result.Add(start35, 35);
						}else {
							DateTime end35 = de.AddMinutes(-sumTLow + needTime);
							foreach (KeyValuePair<DateTime, double> r in result) {
								if (r.Key >= end35 && r.Key <= de) {
									result[r.Key] = 0;
								}								
							}
							result.Add(end35, 35);
						}
					}

					ds = rec.Key;
				}
			}
		}

		public void WriteToDB() {

		}
	}
}
