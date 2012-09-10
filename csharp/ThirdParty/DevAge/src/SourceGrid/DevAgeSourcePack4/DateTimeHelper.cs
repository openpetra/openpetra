using System;

namespace DevAge
{
	/// <summary>
	/// Summary description for DateTimeHelper.
	/// </summary>
	public class DateTimeHelper
	{
		/// <summary>
		/// Calculate the difference in years of 2 dates. Usually used for age calculations. dateA - dateB
		/// </summary>
		/// <param name="dateA"></param>
		/// <param name="dateB"></param>
		/// <returns></returns>
		public static int YearsDifference(DateTime dateA, DateTime dateB)
		{
			return MonthsDifference(dateA, dateB) / 12;
		}

		/// <summary>
		/// Calculate the difference in months of 2 dates. dateA - dateB
		/// </summary>
		/// <param name="dateA"></param>
		/// <param name="dateB"></param>
		/// <returns></returns>
		public static int MonthsDifference(DateTime dateA, DateTime dateB)
		{
			if (dateA == dateB)
				return 0;
			else if (dateA > dateB)
			{
				int years = dateA.Year - dateB.Year;
				int totMonthAge = years * 12 + (dateA.Month - dateB.Month);
				if (totMonthAge == 0)
					return totMonthAge;

				DateTime shiftedDate = dateB.AddMonths(totMonthAge);
				bool isPast = (dateA - shiftedDate).Ticks >= 0;

				if (isPast)
					return totMonthAge;
				else
					return totMonthAge - 1;
			}
			else
				return -MonthsDifference(dateB, dateA);
		}
	}
}
