using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagementSystem_Kylosov.Classes
{
    public class TypeOpertation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public TypeOpertation(int _id, string _name, string _description)
        {
            this.id = _id;
            this.name = _name;
            this.description = _description;
        }

        public static List<TypeOpertation> AllTypeOpertation()
        {
            List<TypeOpertation> allTypeOpertation = new List<TypeOpertation>();
            allTypeOpertation.Add(new TypeOpertation(1, "Печать", ""));
            allTypeOpertation.Add(new TypeOpertation(2, "Копия", ""));
            allTypeOpertation.Add(new TypeOpertation(3, "Сканирование", ""));
            allTypeOpertation.Add(new TypeOpertation(4, "Ризограф", ""));
            return allTypeOpertation;
        }
    }
}
