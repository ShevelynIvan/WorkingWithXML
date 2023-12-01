namespace WorkingWithXML
{
    public class Car
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Number { get; set; }

        public Car(string name, int price, string number)
        {
            Name = name;
            Price = price;
            Number = number;
        }
    }
}
