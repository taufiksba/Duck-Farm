using System.Windows.Forms;

namespace DuckFarm.Classes.Factories
{
    public interface IFormFactory
    {
        Form Create(string menuId);
    }
}
