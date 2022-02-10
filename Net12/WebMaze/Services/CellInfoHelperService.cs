using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public byte[] CreateDeveloperChallenge()
        {
            using (var ms = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    List<string> namesTypeOfCell = GetNamesTypeOfCell(); ;

                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());
                    var para = body.AppendChild(new Paragraph());
                    var runTitle = para.AppendChild(new Run());
                    var prop = runTitle.AppendChild(new RunProperties());

                    prop.AppendChild(new FontSize { Val = "28" });

                    var text = runTitle.AppendChild(new Text());
                    text.Text = $"Developer Challenge: Design and add description for the following cells:\u00A0";

                    foreach (var item in namesTypeOfCell)
                    {
                        var paraName = body.AppendChild(new Paragraph());
                        var runTitleName = para.AppendChild(new Run());
                        var propName = runTitle.AppendChild(new RunProperties());
                        var textName = runTitle.AppendChild(new Text());
                        textName.Text = $"{item},\u00A0";
                    }

                    wordDocument.Close();
                }

                var answer = ms.ToArray();

                return answer;
            }
        }
    }
}
