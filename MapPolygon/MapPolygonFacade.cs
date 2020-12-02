using MapPolygon.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace MapPolygon
{
    /// <summary>
    /// Фасадный класс для работы с геосервисом
    /// </summary>
    public class MapPolygonFacade
    {
        /// <summary>
        /// Интерфейс для взаимодействия с геосервисом
        /// </summary>
        public IMapApiInterface MapApiInterface { get; }

        public MapPolygonFacade(IMapApiInterface mapApiInterface)
        {
            MapApiInterface = mapApiInterface;
        }

        /// <summary>
        /// Получает полигон вершин по наименованию объекта и частотой выборки вершин,
        /// записывает массив вершин в файл с указанным именем
        /// </summary>
        /// <param name="place">Наименование объекта на карте</param>
        /// <param name="fileName">Наименование файла для экспорта вершин полигона</param>
        /// <param name="splittingRate">Частота, с которой выбираются вершины из полигона</param>
        public void GetPolygonVerticesAndWriteToFile(string place, string fileName, int splittingRate = 1)
        {
            JArray verticesArray;
            try
            {
                verticesArray = MapApiInterface.GetPolygonalAreaVertices(place);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return;
            }

            if (verticesArray == null)
            {
                Console.WriteLine("Указанное место не найдено на карте. ");
                return;
            }

            string writePath = AppDomain.CurrentDomain.BaseDirectory + $"{fileName}.txt";
            JArray finalVerticesArray =
                splittingRate == 1 ? verticesArray : ReducePolygonPrecision(verticesArray, splittingRate);

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    streamWriter.WriteLine(finalVerticesArray);
                }

                Console.WriteLine($"Запись выполнена в файл {writePath}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Произошла ошибка при записи в файл. {exception}");
            }
        }

        /// <summary>
        /// Уменьшает точность полигона путем удаления вершин
        /// </summary>
        /// <param name="verticesArray">Массив вершин полигонального представления объекта</param>
        /// <param name="splittingRate">Частота, с которой выбираются вершины из полигона</param>
        /// <returns>Возвращает новый массив с меньшим количеством вершин</returns>
        public JArray ReducePolygonPrecision(JArray verticesArray, int splittingRate)
        {
            JArray finalVerticesArray = new JArray();

            foreach (JArray subAreaVerticesArray in verticesArray)
            {
                JArray finalSubAreaVerticesArray = new JArray();
                foreach (var item in subAreaVerticesArray.Values().Select((value, i) => (value, i)))
                {
                    if (item.i % splittingRate == 0)
                    {
                        finalSubAreaVerticesArray.Add(item.value);
                    }
                }
                finalVerticesArray.Add(finalSubAreaVerticesArray);
            }

            return finalVerticesArray;
        }
    }
}