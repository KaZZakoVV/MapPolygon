using System;

namespace MapPolygon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Получение полигона объекта из геосервиса.");

            string place = String.Empty;
            while (String.IsNullOrEmpty(place))
            {
                Console.Write("Введите место (область) для поиска на карте: ");
                place = Console.ReadLine();
            }

            int splittingRate = 0;
            while (splittingRate == 0)
            {
                Console.Write("Введите частоту, с которой выбираются вершины из полигона: ");
                Int32.TryParse(Console.ReadLine(), out splittingRate);
            }

            string fileName = String.Empty;
            while (String.IsNullOrEmpty(fileName))
            {
                Console.Write("Введите имя файла для сохранения массива вершин полигона: ");
                fileName = Console.ReadLine();
            }

            MapPolygonFacade mapPolygonFacade = new MapPolygonFacade(new OpenStreetMapApiInterface());
            mapPolygonFacade.GetPolygonVerticesAndWriteToFile(place, fileName, splittingRate);
        }
    }
}
