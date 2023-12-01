using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace WorkingWithXML
{
    internal class Program
    {
        /// <summary>
        /// Files to saving list of cars. It places in program's folder 
        /// </summary>
        private const string _xmlListOfCars = "ListOfCars.xml";
        private const string _xmlListOfCarsReflection = "ListOfCarsRef.xml";
        
        static void Main(string[] args)
        {
            List<Car> cars = new List<Car>()
            {
                new Car("VW",9000,"AX5555"),
                new Car("BMW",13000,"AX7777"),
                new Car("Audi",8500,"VV1234"),
                new Car("Opel",4000,"BB9599"),
            };
            
            //Attribute value can't be null, so i added in methods check for null and it work correctly!
            //cars[0].Name = null;

            SaveCollectionInXmlFile(cars);
            SaveCollectionInXmlFileWithReflection(cars);

            SetNewCarPriceByNumber("VV1234", 777);
        }

        private static void SaveCollectionInXmlFile(IEnumerable<Car> cars)
        {
            var document = new XDocument();
            var rootElement = new XElement("listOfCars");

            foreach (var car in cars)
            {
                var carElement = new XElement("car");
                var carNameAttr = new XAttribute("name", car.Name ?? "");
                var carPrice = new XElement("price", car.Price);
                var carNumber = new XElement("number", car.Number);

                carElement.Add(carNameAttr);
                carElement.Add(carPrice);
                carElement.Add(carNumber);

                rootElement.Add(carElement);
            }

            document.Add(rootElement);
            document.Save(_xmlListOfCars);

        }

        static void SaveCollectionInXmlFileWithReflection(IEnumerable<Car> cars)
        {
            var document = new XDocument();
            var rootElement = new XElement("listOfCars");
            
            Type carType = cars.GetType().GetGenericArguments()[0];
            PropertyInfo[] properties = carType.GetProperties();

            foreach (var car in cars)
            {
                var carElement = new XElement(carType.Name.ToLower());

                foreach (var prop in properties)
                {
                    if (prop.Name.ToLower() == "name")
                    {
                        var carAttribute = new XAttribute(prop.Name.ToLower(), prop.GetValue(car) ?? "");
                        carElement.Add(carAttribute);
                    }
                    else
                    {
                        var carProperty = new XElement(prop.Name.ToLower(), prop.GetValue(car));
                        carElement.Add(carProperty);
                    }
                }

                rootElement.Add(carElement);
            }
            document.Add(rootElement);
            document.Save(_xmlListOfCarsReflection);
        }

        static void SetNewCarPriceByNumber(string carNumber, int newPrice)
        {
            var document = XElement.Load(_xmlListOfCars);

            document.Descendants("car")
                .Where(x => x.Element("number")?.Value.ToLower() == carNumber.ToLower())
                .FirstOrDefault()?
                .SetElementValue("price", newPrice);

            document.Save(_xmlListOfCars);
        }
    }
}