using System.Collections.Generic;

namespace MiddlewareLibrary.Models
{
    public class CarMarkModel 
    {
        public string Name { get; set; }

        public List<CarTypeModel> TypeModels { get; set; }
    }
}