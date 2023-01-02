using Newtonsoft.Json;

namespace GetPokedexData
{
    // Classe ayant la charge de convertir les Json en Objets de différents types. 
    public static class JsonToObject
    {
        // Conversion d'un Json en Objet Pokemon.
        public static T jsonToObject<T>(string jsonString)
        {
            T DeserializedObject = JsonConvert.DeserializeObject<T>(jsonString);
            return DeserializedObject;
        }
    }
}
