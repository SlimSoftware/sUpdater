namespace sUpdater.Models
{
    class Credit
    {
        public string Name { get; }
        public string Author { get; }
        public string URL { get; }

        public Credit(string name, string author, string url)
        {
            Name = name;
            Author = author;
            URL = url;
        }
    }
}
