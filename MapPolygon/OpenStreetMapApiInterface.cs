using MapPolygon.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MapPolygon
{
    /// <summary>
    /// Реализация интерфейса для работы с OpenStreetMap
    /// </summary>
    public class OpenStreetMapApiInterface : IMapApiInterface
    {
        /// <summary>
        /// Требуется для доступа к OSM 
        /// </summary>
        public readonly string Email;

        public OpenStreetMapApiInterface()
        {
            Email = "k9207123456@gmail.com";
        }

        public JArray GetPolygonalAreaVertices(string place)
        {
            string url = @$"https://nominatim.openstreetmap.org/search?q={place}&format=json&polygon_geojson=1&email={Email}";

            try
            {
                string responseFromServer = GetResponse(url);
                return responseFromServer == null ? null : GetVertices(responseFromServer);
            }
            catch (WebException webException)
            {
                throw new WebException("Произошла ошибка при попытке доступа к OpenStreetMap.", webException);
            }
            catch (Exception exception)
            {
                throw new Exception("Произошла неизвестная ошибка.", exception);
            }
        }

        /// <summary>
        /// Получает ответ сервиса OpenStreetMap
        /// </summary>
        /// <param name="url">Request url к OpenStreetMap</param>
        /// <returns>ответ сервиса OpenStreetMap</returns>
        private string GetResponse(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream());
            string response = reader.ReadToEnd();
            webResponse.Close();
            return response;
        }

        /// <summary>
        /// Возвращает узел с координатами полигона из response
        /// </summary>
        /// <param name="responseFromServer">Ответ сервиса OpenStreetMap</param>
        /// <returns>Узел coordinates из полученного от OpenStreetMap response</returns>
        private JArray GetVertices(string responseFromServer)
        {
            // забираем первый найденный на сервисе объект
            JObject mapObjectInfo = JsonConvert.DeserializeObject<JArray>(responseFromServer).ToObject<List<JObject>>().FirstOrDefault();

            return mapObjectInfo == null
                ? null
                : new JArray(mapObjectInfo.Descendants()
                    .Where(token => token.Type == JTokenType.Property && ((JProperty)token).Name == "coordinates")
                    .Select(property => ((JProperty)property).Value).FirstOrDefault());
        }
    }
}