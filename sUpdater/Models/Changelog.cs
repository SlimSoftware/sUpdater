﻿namespace sUpdater.Models
{
    public class Changelog
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
    }
}