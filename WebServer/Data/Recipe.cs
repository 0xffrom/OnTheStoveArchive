using ObjectsLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Data
{

    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public RecipeFull RecipeFull { get; set; }

        public Recipe()
        {
            //
        }

    }
}
