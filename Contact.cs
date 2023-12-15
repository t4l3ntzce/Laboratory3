namespace Task03
{
    public class Contact
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}\nSurname: {Surname}\nPhone: {Phone}\nE-mail: {Email}\n";
        }
    }
}