using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines
				.Skip(1)
				.Select(x => x.Split(';'))
				.Where(x => SlideCheck(x))
				.Where(x => {
                    try
                    {
						GetSlide(x[1]);
						return true;
                    }
					catch
                    {
						return false;
                    }
				})
				.ToDictionary(x => Convert.ToInt32(x[0]), x => new SlideRecord(Convert.ToInt32(x[0]), GetSlide(x[1]), x[2]));
		}

		private static SlideType GetSlide(string slideType)
        {
            switch (slideType.ToLower())
            {
				case "theory":
					return SlideType.Theory;
				case "quiz":
					return SlideType.Quiz;
				case "exercise":
					return SlideType.Exercise;
				default:
					throw new FormatException();
            }
        }

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines.Skip(1).Select(x => ParseLine(x, slides));
		}

		public static VisitRecord ParseLine(string line, IDictionary<int, SlideRecord> slides)
        {
			try
			{
				var data = line.Split(';');
				var parsedDate = data[2].Split('-').Select(y => Convert.ToInt32(y)).ToArray();
				var parsedTime = data[3].Split(':').Select(y => Convert.ToInt32(y)).ToArray();
				var dateTime = new DateTime(
					parsedDate[0], parsedDate[1], parsedDate[2],
					parsedTime[0], parsedTime[1], parsedTime[2]
				);
				var slideId = Convert.ToInt32(data[1]);
				if (!slides.ContainsKey(slideId))
					throw new FormatException();
				var slideType = slides[slideId].SlideType;

				return new VisitRecord(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), dateTime, slideType);
			}
			catch (Exception e)
			{
				throw new FormatException("Wrong line [" + line + "]", e);
			}
		}

		public static bool SlideCheck(string[] slide)
        {
			if (slide.Length != 3)
				return false;

			int id;
			if (!int.TryParse(slide[0], out id))
				return false;

			SlideType type;
			return Enum.TryParse(slide[1], true, out type);
		}
	}
}