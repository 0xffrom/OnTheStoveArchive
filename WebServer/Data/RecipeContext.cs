using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NFX.Serialization.Slim;
using ObjectsLibrary;

namespace WebServer.Data
{
    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public RecipeContext()
        {
            Database.EnsureCreated();
        }
        
        public RecipeFull ByteToRecipe(byte[] arrBytes)
        {
            var memoryStream = new MemoryStream();
            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            
            SlimSerializer slimSerializer = new SlimSerializer();
            return (RecipeFull) slimSerializer.Deserialize(memoryStream);
        }

        public byte[] RecipeToByte(RecipeFull recipeFull)
        {
            var memoryStream = new MemoryStream();
            SlimSerializer slimSerializer = new SlimSerializer(); 
            slimSerializer.Serialize(memoryStream, recipeFull);
            
            return memoryStream.ToArray();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;UserId=root;Password=password;database=recipes;");
        }
    }
}
