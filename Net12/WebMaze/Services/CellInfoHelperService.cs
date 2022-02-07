using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebMaze.Controllers;

namespace WebMaze.Services
{
    public class CellInfoHelperService

    {
        public List<string> GetNamesTypeOfCell()
        {
            var typeOfCell = new List<Type>() { typeof(BaseCell) };
            typeOfCell.AddRange(TypeCollector(typeOfCell));

            var namesTypeOfCell = typeOfCell
               .Select(x => x.Name)
               .Select(x => x.ToLower())
               .ToList();

            //section on removing base and intermediate types
            var noShowTypes = new List<string>() { "BaseCell", "BaseEnemy", "Character" }
            .Select(x => x.ToLower())
            .ToList();
            namesTypeOfCell.RemoveAll(x => noShowTypes.Contains(x));

            var namesOfActions = typeof(CellInfoController)
               .GetMethods()
               .Where(x => x.ReturnType == typeof(IActionResult))
               .Select(x => x.Name)
               .Select(x => x.ToLower())
               .ToList();

            namesTypeOfCell.RemoveAll(x => namesOfActions.Contains(x));
            return namesTypeOfCell;
        }

        public List<Type> TypeCollector(List<Type> inTypes)
        {
            List<Type> outTypes = new List<Type>();
            foreach (var item in inTypes)
            {
                var heirs = Assembly.GetAssembly(typeof(BaseCell))
                .GetTypes()
                .Where(x => x.BaseType == item)
                .ToList();

                outTypes.AddRange(heirs);
            }

            if (outTypes.Any())
            {
                outTypes.AddRange(TypeCollector(outTypes));
            }
            return outTypes;
        }
    }
}
