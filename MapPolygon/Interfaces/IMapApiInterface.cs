using Newtonsoft.Json.Linq;

namespace MapPolygon.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с геосервисами
    /// </summary>
    public interface IMapApiInterface
    {
        /// <summary>
        /// По переданному наименованию объекта на карте возвращает массив
        /// координат вершин олигонального представления объекта 
        /// </summary>
        /// <param name="place">Название объекта на карте</param>
        /// <returns>Возвращает массив координат вершин полигонального представления объекта</returns>
        public JArray GetPolygonalAreaVertices(string place);
    }
}